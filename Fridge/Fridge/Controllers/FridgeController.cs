using AutoMapper;
using Fridge.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Repositories.Repository.Interfaces;

namespace Fridge.Controllers
{
    [Produces("application/json")]
    [Route("api/fridges")]
    [ApiController]
    public class FridgeController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<FridgeController> _logger;
        private readonly IMapper _mapper;
       
        public FridgeController(IRepositoryManager repository, ILogger<FridgeController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns a list of available fridges.
        /// </summary>
        /// <returns>A list of available fridges.</returns>
        /// <response code="200">Returns a list of available fridges.</response>
        [HttpGet("available-fridges")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetFridges()
        {
            var fridges = await _repository.Fridge.GetAllFridgesAsync(trackChanges: false);
            if (fridges is null)
            {
                _logger.LogInformation($"Fridges are not fiund in the database.");
                return NotFound();
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
        /// Returns a list of available models.
        /// </summary>
        /// <returns>A list of available models.</returns>
        /// <response code="200">Returns a list of available models.</response>
        [HttpGet("available-models")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetModels()
        {
            var models = await _repository.Model.GetAllModels(trackChanges: false);
            return Ok(models);
        }

        /// <summary>
        /// Returns a list of available producers.
        /// </summary>
        /// <returns>A list of available producers.</returns>
        /// <response code="200">Returns a list of available producers.</response>
        [HttpGet("available-producers")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
            nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetProducers()
        {
            var producers = await _repository.Producer.GetAllProducers(trackChanges: false);
            return Ok(producers);
        }
    }
}
