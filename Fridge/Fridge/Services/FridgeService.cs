using Fridge.Data.Models;
using Fridge.Data.Repositories.Interfaces;
using Fridge.Models.DTOs.FridgeDtos;
using Fridge.Services.Abstracts;


namespace Fridge.Services
{
    public class FridgeService : IFridgeService
    {
        private readonly IRepositoryManager _repository;

        public FridgeService(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<FridgeDto>> GetFridges()
        {
            var fridges = await _repository.Fridge.GetAvailableFridgesAsync(trackChanges: false);

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
                Model = _repository.Model.GetModelByIdAsync(fridge.ModelId, trackChanges: false).Name,
                Owner = _repository.Owner.GetOwnerByIdAsync(fridge.OwnerId, trackChanges: false).Result.Name,
                Producer = _repository.Producer.GetProducerByIdAsync(fridge.ProducerId, trackChanges: false).Result.Name,
                Capacity = fridge.Capacity,
                isRented = fridge.IsRented,
            });

            return fridgesDto.ToList();
        }

        public async Task<IEnumerable<Model>> GetModels()
        {
            var models = await _repository.Model.GetAllModels(trackChanges: false);

            return models.ToList();
        }

        public async Task<IEnumerable<Producer>> GetProducers()
        {
            var producers = await _repository.Producer.GetAllProducers(trackChanges: false);

            return producers.ToList();
        }
    }
}
