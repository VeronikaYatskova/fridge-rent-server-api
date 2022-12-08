using Fridge.Data.Models;
using Fridge.Data.Repositories.Interfaces;
using Fridge.Models.DTOs.FridgeDtos;
using Fridge.Services.Abstracts;


namespace Fridge.Services
{
    public class FridgeService : IFridgeService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<FridgeService> _logger;

        private readonly TokenInfo tokenInfo;
        private Owner? _owner;

        public FridgeService(IRepositoryManager repository, IHttpContextAccessor httpContextAccessor, ILogger<FridgeService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<FridgeDto>> GetFridges()
        {
            var fridges = await _repository.Fridge.GetAvailableFridgesAsync();

            if (fridges is null)
            {
                throw new ArgumentException("Fridges are not found in the database.");
            }

            if (!fridges.Any())
            {
                return new List<FridgeDto>() { };
            }

            var fridgesDto = fridges.Select(fridge => new FridgeDto
            {
                Id = fridge.Id,
                Model = _repository.Model.GetModelByIdAsync(fridge.ModelId).Name,
                Owner = _repository.Owner.GetOwnerByIdAsync(fridge.OwnerId).Result.Name,
                Producer = _repository.Producer.GetProducerByIdAsync(fridge.ProducerId).Result.Name,
                Capacity = fridge.Capacity,
            });

            return fridgesDto.ToList();
        }

        public async Task<IEnumerable<Model>> GetModels()
        {
            var models = await _repository.Model.GetAllModels();

            return models.ToList();
        }

        public async Task<IEnumerable<Producer>> GetProducers()
        {
            var producers = await _repository.Producer.GetAllProducers();

            return producers.ToList();
        }


    }
}
