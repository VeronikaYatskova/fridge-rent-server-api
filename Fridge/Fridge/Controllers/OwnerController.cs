using AutoMapper;
using Fridge.Models;
using Fridge.Models.DTOs;
using Fridge.Models.RoleBasedAuthorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Models.DTOs;
using Models.Models.RoleBasedAuthorization;
using Repositories.Repository.Interfaces;

namespace Fridge.Controllers
{
    [Produces("application/json")]
    [Route("api/owner")]
    [ApiController]
    [Authorize(Roles = UserRoles.Owner)]
    public class OwnerController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<FridgeController> _logger;
        private readonly IMapper _mapper;
        private readonly Owner _owner;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OwnerController(IRepositoryManager repository, ILogger<FridgeController> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;

            var guid = TokenInfo.GetInfo(httpContextAccessor);
            _owner = _repository.Owner.GetOwnerByConditionAsync(u => u.Id == Guid.Parse(guid), trackChanges: false).Result;
        }

        /// <summary>
        /// Returns a list of fridges that the owner has.
        /// </summary>
        /// <returns>A list of fridges that the owner has.</returns>
        /// <response code="200">Returns a list of fridges that the owner has.</response>
        [HttpGet("fridges")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetOwnersFridges()
        {
            var fridges = await _repository.Fridge.GetFridgeByConditionAsync(f => f.OwnerId == _owner.Id, trackChanges: false);

            if (fridges?.Count() == 0 || fridges is null)
            {
                _logger.LogInformation($"Owner with id {_owner.Id} doesn't have any fridges.");
                return NotFound("Owner doesn't have any fridges.");
            }

            var fridgesDto = fridges.Select(fridge => new FridgeDto
                {
                    Id = fridge.Id,
                    Model = _repository.Model.GetModelByIdAsync(fridge.ModelId, trackChanges: false).Name,
                    Owner = _repository.Owner.GetOwnerByIdAsync(fridge.OwnerId, trackChanges: false).Result.Name,
                    Producer = _repository.Producer.GetProducerByIdAsync(fridge.ProducerId, trackChanges: false).Result.Name,
                    Capacity = fridge.Capacity,
                    isRented = fridge.IsRented,
                });
            return Ok(fridgesDto);
        }

        /// <summary>
        /// Returns info about the user who rented a fridge identifier, rent start and finish date, fridge identifier.
        /// </summary>
        /// <param name="fridgeId">Fridge to see the info about.</param>
        /// <returns>Returns info about the user who rented a fridge identifier, rent start and finish date, fridge identifier.</returns>
        /// <respose code ="200">Returns info about the user who rented a fridge identifier, rent start and finish date, fridge identifier.</respose>
        [HttpGet("fridge/{fridgeId}/rent-info")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetRentedFridgeInfo(Guid fridgeId)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the object");
                return UnprocessableEntity(ModelState);
            }

            var rentDocument = new RentDocument();

            var fr = _repository.UserFridge.GetFridgeById(fridgeId, trackChanges: false).RentDocumentId;
            rentDocument = _repository.RentDocument.FindDocumentByCondition(d => d.Id == fr, trackChanges: false);

            var rentDocumentDto = new RentDocumentDto
            {
                Id = rentDocument.Id,
                RenterEmail = _repository.User.FindUserByCondition(u => u.Id == rentDocument.UserId, trackChanges: false).Email,
                OwnerName = _owner.Name,
                StartDate = rentDocument.StartDate.ToShortDateString(),
                EndDate = rentDocument.EndDate.ToShortDateString(),
                MonthCost = rentDocument.MonthCost,
            };

            return Ok(rentDocumentDto);
        }

        /// <summary>
        /// Allows to add one more fridge for renting.
        /// </summary>
        /// <param name="newFridge"></param>
        [HttpPost("fridge/add")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Post))]
        public async Task<IActionResult> AddFridge([FromBody] OwnerAddFridgeDto newFridge)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the object");
                return UnprocessableEntity(ModelState);
            }

            var fridge = new FridgeServiceDto
            {
                FridgeId = Guid.NewGuid(),
                Capacity = newFridge.Capacity,
                ModelId = newFridge.ModelId,
                OwnerId = _owner.Id,
                ProducerId = newFridge.ProducerId,
            };

            _repository.Fridge.AddFridge(fridge);
            await _repository.SaveAsync();

            return Created("api/owner/fridge/add", fridge);
        }

        /// <summary>
        /// Allows to delete a fridge.
        /// </summary>
        /// <param name="fridgeId"></param>
        /// <returns></returns>
        [HttpDelete("fridge/{fridgeId}/remove")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Delete))]
        public async Task<IActionResult> DeleteFridge(Guid fridgeId)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the ProductPicture object");
                return UnprocessableEntity(ModelState);
            }

            var fridges = await _repository.Fridge.GetFridgeByConditionAsync(f => f.Id == fridgeId && f.OwnerId == _owner.Id, trackChanges:false);

            if (fridges is null|| fridges.Count() == 0)
            {
                _logger.LogInformation($"Owner with id {_owner.Id} doesn't have a fridge with id {fridgeId}");
                return NotFound($"Owner with id {_owner.Id} doesn't have a fridge with id {fridgeId}");
            }

            var fridge = await _repository.Fridge.GetFridgeByIdAsync(fridges.First().Id, trackChanges:false);

            if (fridge!.IsRented == true)
            {
                _logger.LogInformation("Unable to delete a rented fridge.");
                return BadRequest("Unable to delete a rented fridge.");
            }

            _repository.Fridge.RemoveFridge(fridge!);
            await _repository.SaveAsync();

            return Ok();
        }
    }
}
