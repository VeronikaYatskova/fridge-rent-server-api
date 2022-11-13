using Fridge.Models;
using Fridge.Models.DTOs;
using Fridge.Models.RoleBasedAuthorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Models.RoleBasedAuthorization;
using Repositories.Repository.Interfaces;

namespace Fridge.Controllers
{
    [Route("api/rent")]
    [ApiController]
    [Authorize(Roles = UserRoles.Renter)]
    public class RentController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<RentController> _logger;
        private readonly User user;
        
        public RentController(ILogger<RentController> _logger, IRepositoryManager _repository, IHttpContextAccessor httpContextAccessor)
        {
            this._repository = _repository;
            this._logger = _logger;

            var guid = TokenInfo.GetInfo(httpContextAccessor);
            user = _repository.User.FindUserByCondition(u => u.Id == Guid.Parse(guid), trackChanges: false);
        }

        /// <summary>
        /// Returns a list of fridges that user rented.
        /// </summary>
        /// <returns>A list of fridges</returns>
        [HttpGet("fridges/rented")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<IEnumerable<Models.Fridge>>> GetUsersFridges()
        {
            var userFridges = _repository.UserFridge.GetUserFridge(user.Id, trackChanges:false);
            var allFridges = await _repository.Fridge.GetFridgesAsync(trackChanges: false);
            var fr = allFridges.Join(userFridges,
                                     f => f.Id, 
                                     uf => uf.FridgeId, 
                                     (f, uf) => f).ToList();

            var fridges = fr.Select(fridge => new FridgeDto
            {
                Id = fridge.Id,
                Model = _repository.Model.GetModelByIdAsync(fridge.ModelId, trackChanges: false).Name,
                Owner = _repository.Owner.GetOwnerByIdAsync(fridge.OwnerId, trackChanges: false).Result.Name,
                Producer = _repository.Producer.GetProducerByIdAsync(fridge.ProducerId, trackChanges: false).Result.Name,
                Capacity = fridge.Capacity,
                isRented = fridge.IsRented,
                CurrentCount = _repository.FridgeProduct.GetAllProductsInTheFridgeAsync(fridge.Id, trackChanges: false).Result.Select(f => f.Count).Sum(),
            }).ToList();
            
            if (!fridges.Any())
            {
                _logger.LogInformation("No fridges");
                return NotFound();
            }

            await _repository.SaveAsync();
            return Ok(fridges);
        }

        /// <summary>
        /// Method to rent a fridge.
        /// </summary>
        /// <returns>Rented fridge</returns>
        [HttpPost("fridge/{fridgeId}/rent")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        public async Task<IActionResult> RentFridge(Guid fridgeId)
        {
            var fridge = await _repository.Fridge.GetFridgeByIdAsync(fridgeId, trackChanges:false);

            if (fridge is null)
            {
                _logger.LogInformation($"Fridge with id {fridgeId} is not found.");
                return NotFound();
            }

            fridge.IsRented = true;

            _repository.Fridge.UpdateFridge(fridge);

            var rentDocument = new RentDocument
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                FridgeId = fridgeId,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(12),
                MonthCost = 30,
            };

            var userFridge = new UserFridge
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                FridgeId = fridgeId,
                RentDocumentId = rentDocument.Id,
                RentDocument = rentDocument,
            };

            _repository.RentDocument.AddDocument(rentDocument);
            _repository.UserFridge.RentFridge(userFridge);

            await _repository.SaveAsync();

            return Ok();
        }

        /// <summary>
        /// Method to return a fridge to its owner.
        /// </summary>
        /// <param name="fridgeId">Guid of a fridge to delete.</param>
        /// <returns>Status Code</returns>
        [HttpDelete("fridge/{fridgeId}/return")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Delete))]
        public async Task<IActionResult> Remove(Guid fridgeId)
        {
            var fridge = await _repository.Fridge.GetFridgeByIdAsync(fridgeId, trackChanges: false);

            if (fridge is null)
            {
                _logger.LogInformation($"Fridge with id {fridgeId} is not found.");
                return NotFound();
            }


            var productsInFridge = await _repository.FridgeProduct.GetAllProductsInTheFridgeAsync(fridgeId, trackChanges:false);
            productsInFridge.ToList().ForEach(product => _repository.FridgeProduct.DeleteProduct(product)); 
            
            fridge.IsRented = false;
            _repository.Fridge.UpdateFridge(fridge);

            var userFridge = _repository.UserFridge.GetUserFridgeRow(user.Id, fridgeId, trackChanges:false);
            _repository.UserFridge.RemoveFridge(userFridge);

            await _repository.SaveAsync();

            return Ok();
        }
    }
}
