using Fridge.Data.Models;
using Fridge.Data.Repositories.Interfaces;
using Fridge.Models.DTOs;
using Fridge.Services.Abstracts;

namespace Fridge.Services
{
    public class FridgeService : IFridgeService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<AuthorizationService> _logger;

        public FridgeService(IConfiguration configuration, IRepositoryManager repository, ILogger<AuthorizationService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<FridgeDto>> GetFridges()
        {
            var fridges = await _repository.Fridge.GetAllFridgesAsync(trackChanges: false);
            if (fridges is null)
            {
                _logger.LogInformation("Fridges are not found in the database.");
                throw new ArgumentException("Fridges are not found in the database.");
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

            return fridgesDto;
        }

        public async Task<IEnumerable<Model>> GetModels()
        {
            var models = await _repository.Model.GetAllModels(trackChanges: false);

            if (models is null)
            {
                throw new ArgumentException("No models...");
            }

            return models;
        }

        public async Task<IEnumerable<Producer>> GetProducers()
        {
            var producers = await _repository.Producer.GetAllProducers(trackChanges: false);

            if (producers is null)
            {
                throw new ArgumentException("No producers...");
            }

            return producers;
        }
    }
}
