using Fridge.Data.Models;
using Fridge.Data.Repositories.Interfaces;
using Fridge.Models.DTOs.FridgeDtos;
using Fridge.Services.Abstracts;


namespace Fridge.Services
{
    public class RentService : IRentService
    {
        private readonly ILogger<RentService> _logger;
        private readonly IRepositoryManager _repository;

        private readonly Renter? renter;

        public RentService(IRepositoryManager repository, IHttpContextAccessor httpContextAccessor, ILogger<RentService> logger)
        {
            _repository = repository;
            _logger = logger;

            var tokenInfo = new TokenInfo(repository, httpContextAccessor);
            renter = tokenInfo.GetUser().Result;
        }

        public async Task<IEnumerable<FridgeDto>> GetRentersFridges()
        {
            var renterFridges = _repository.RenterFridge.GetRenterFridge(renter.Id, trackChanges: false);

            if (!renterFridges.Any())
            {
                _logger.LogInformation("No fridges");
                throw new ArgumentException("No fridges");
            }

            var allFridges = await _repository.Fridge.GetFridgesAsync(trackChanges: false);
            var fr = allFridges.Join(renterFridges,
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

            await _repository.SaveAsync();

            return fridges;
        }

        public async Task RentFridge(Guid fridgeId)
        {
            var fridge = await _repository.Fridge.GetFridgeByIdAsync(fridgeId, trackChanges: false);

            if (fridge is null)
            {
                _logger.LogInformation($"Fridge with id {fridgeId} is not found.");
                throw new ArgumentException($"Fridge with id {fridgeId} is not found.");
            }

            fridge.IsRented = true;

            _repository.Fridge.UpdateFridge(fridge);

            var rentDocument = new RentDocument
            {
                Id = Guid.NewGuid(),
                RenterId = renter.Id,
                FridgeId = fridgeId,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(12),
                MonthCost = 30,
            };

            var renterFridge = new RenterFridge
            {
                Id = Guid.NewGuid(),
                RenterId = renter.Id,
                FridgeId = fridgeId,
                RentDocumentId = rentDocument.Id,
                RentDocument = rentDocument,
            };

            _repository.RentDocument.AddDocument(rentDocument);
            _repository.RenterFridge.RentFridge(renterFridge);

            await _repository.SaveAsync();
        }

        public async Task Remove(Guid fridgeId)
        {
            var fridge = await _repository.Fridge.GetFridgeByIdAsync(fridgeId, trackChanges: false);

            if (fridge is null)
            {
                _logger.LogInformation($"Fridge with id {fridgeId} is not found.");
                throw new ArgumentException($"Fridge with id {fridgeId} is not found.");
            }

            var productsInFridge = await _repository.FridgeProduct.GetAllProductsInTheFridgeAsync(fridgeId, trackChanges: false);
            productsInFridge.ToList().ForEach(product => _repository.FridgeProduct.DeleteProduct(product));

            fridge.IsRented = false;
            _repository.Fridge.UpdateFridge(fridge);

            var renterFridge = _repository.RenterFridge.GetRenterFridgeRow(renter.Id, fridgeId, trackChanges: false);
            _repository.RenterFridge.RemoveFridge(renterFridge);

            await _repository.SaveAsync();
        }
    }
}
