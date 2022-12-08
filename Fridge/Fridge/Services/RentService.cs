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
        
        private readonly TokenInfo tokenInfo;
        private Renter? renter;

        public RentService(IRepositoryManager repository, IHttpContextAccessor httpContextAccessor, ILogger<RentService> logger)
        {
            _repository = repository;
            _logger = logger;

            tokenInfo = new TokenInfo(repository, httpContextAccessor);
        }

        public async Task<IEnumerable<FridgeDto>> GetRentersFridges()
        {
            renter = await tokenInfo.GetUser();
            
            var renterFridges = await _repository.Fridge.GetFridgeByConditionAsync(f => f.RenterId == renter.Id);

            if (!renterFridges.Any())
            {
                _logger.LogInformation("No fridges");
                throw new ArgumentException("No fridges");
            }

            var allFridges = await _repository.Fridge.GetFridgesAsync();

            var fridges = allFridges.Select(fridge => new FridgeDto
            {
                Id = fridge.Id,
                Model = _repository.Model.GetModelByIdAsync(fridge.ModelId).Name,
                Owner = _repository.Owner.GetOwnerByIdAsync(fridge.OwnerId).Result.Name,
                Producer = _repository.Producer.GetProducerByIdAsync(fridge.ProducerId).Result.Name,
                Renter = _repository.Renter.FindRenterByCondition(r => r.Id == renter.Id).Email,
                Capacity = fridge.Capacity,
                CurrentCount = _repository.FridgeProduct.GetAllProductsInTheFridgeAsync(fridge.Id).Result.Select(f => f.Count).Sum(),
            }).ToList();

            await _repository.SaveAsync();

            return fridges;
        }

        public async Task RentFridge(Guid fridgeId)
        {
            renter = await tokenInfo.GetUser();

            var fridge = await _repository.Fridge.GetFridgeByIdAsync(fridgeId);

            if (fridge is null)
            {
                _logger.LogInformation($"Fridge with id {fridgeId} is not found.");
                throw new ArgumentException($"Fridge with id {fridgeId} is not found.");
            }

            var rentDocument = new RentDocument
            {
                Id = Guid.NewGuid(),
                RenterId = renter.Id,
                FridgeId = fridgeId,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(12),
                MonthCost = 30,
            };

            _repository.RentDocument.AddDocument(rentDocument);

            fridge.RenterId = renter.Id;
            fridge.RentDocumentId = rentDocument.Id;

            _repository.Fridge.UpdateFridge(fridge);
            
            await _repository.SaveAsync();
        }

        public async Task Remove(Guid fridgeId)
        {
            renter = await tokenInfo.GetUser();

            var fridge = await _repository.Fridge.GetFridgeByIdAsync(fridgeId);

            if (fridge is null)
            {
                _logger.LogInformation($"Fridge with id {fridgeId} is not found.");
                throw new ArgumentException($"Fridge with id {fridgeId} is not found.");
            }

            var productsInFridge = await _repository.FridgeProduct.GetAllProductsInTheFridgeAsync(fridgeId);
            productsInFridge.ToList().ForEach(product => _repository.FridgeProduct.DeleteProduct(product));

            var document = _repository.RentDocument.FindDocumentByCondition(d => d.Id == fridge.RentDocumentId);
            _repository.RentDocument.RemoveDocument(document);

            fridge.RentDocumentId = null;
            fridge.RenterId = null;

            _repository.Fridge.UpdateFridge(fridge);

            await _repository.SaveAsync();
        }
    }
}
