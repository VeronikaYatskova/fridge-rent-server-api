using Fridge.Data.Models;
using Fridge.Data.Repositories.Interfaces;
using Fridge.Services.Abstracts;
using Fridge.Models.Responses;
using Fridge.Models.Requests;
using AutoMapper;


namespace Fridge.Services
{
    public class FridgeService : IFridgeService
    {
        private readonly IRepositoryManager repository;
        private readonly ILogger<FridgeService> logger;
        private readonly IMapper mapper;

        private readonly TokenInfo tokenInfo;

        private Owner? owner;
        private Renter? renter;

        public FridgeService(IRepositoryManager repository, IMapper mapper, IHttpContextAccessor httpContextAccessor, ILogger<FridgeService> logger)
        {
            this.repository = repository;
            this.logger = logger;
            this.mapper = mapper;

            tokenInfo = new TokenInfo(repository, httpContextAccessor);
        }

        public async Task<IEnumerable<FridgeModel>> GetFridges()
        {
            var fridges = await repository.Fridge.GetAvailableFridgesAsync();

            if (fridges is null)
            {
                throw new ArgumentException("Fridges are not found in the database.");
            }

            if (!fridges.Any())
            {
                return new List<FridgeModel>() { };
            }

            var fridgesDto = fridges.Select(fridge => new FridgeModel
            {
                Id = fridge.Id,
                Model = fridge.Model.Name,
                Owner = fridge.Owner.Name,
                Producer = fridge.Producer.Name,
                Capacity = fridge.Capacity,
            });

            return fridgesDto.ToList();
        }

        public async Task<IEnumerable<FridgeModelModel>> GetModels()
        {
            var models = await repository.Model.GetAllModels();

            return mapper.Map<IEnumerable<FridgeModelModel>>(models);
        }

        public async Task<IEnumerable<FridgeProducerModel>> GetProducers()
        {
            var producers = await repository.Producer.GetAllProducers();

            return mapper.Map<IEnumerable<FridgeProducerModel>>(producers);
        }

        public async Task<IEnumerable<OwnerFridgeModel>> GetOwnersFridges()
        {
            owner = await tokenInfo.GetOwner();

            var fridges = owner?.Fridges;

            if (fridges?.Count() == 0 || fridges is null)
            {
                logger.LogInformation($"Owner with id {owner!.Id} doesn't have any fridges.");
                throw new ArgumentException("Owner doesn't have any fridges.");
            }

            var fridgesDto = fridges.Select(fridge => new OwnerFridgeModel
            {
                Id = fridge.Id,
                Model = fridge.Model.Name,
                Owner = fridge.Owner.Name,
                Producer = fridge.Producer.Name,
                Renter = fridge.Renter is null ? "No renter" : fridge.Renter.Email,
                Capacity = fridge.Capacity,
            });

            return fridgesDto;
        }

        public async Task<RentDocumentModel> GetRentedFridgeInfo(Guid fridgeId)
        {
            owner = await tokenInfo.GetOwner();

            var fridge = await repository.Fridge.GetFridgeByIdAsync(fridgeId);

            if (fridge is null)
            {
                throw new ArgumentException("The fridge is not found...");
            }

            if (fridge.RenterId is null)
            {
                throw new ArgumentException("The fridge is not rented...");
            }

            var rentDocument = fridge.RentDocument;

            var rentDocumentDto = new RentDocumentModel
            {
                Id = (Guid)fridge.RentDocumentId!,
                RenterEmail = fridge.Renter.Email,
                OwnerName = owner!.Name,
                StartDate = rentDocument.StartDate.ToShortDateString(),
                EndDate = rentDocument.EndDate.ToShortDateString(),
                MonthCost = rentDocument.MonthCost,
            };

            return rentDocumentDto;
        }

        public async Task<IEnumerable<FridgeRenterModel>> GetRentersFridges()
        {
            renter = await tokenInfo.GetUser();

            var renterFridges = renter?.Fridges;

            if (!renterFridges.Any())
            {
                logger.LogInformation("No fridges");
                throw new ArgumentException("No fridges");
            }

            var fridges = renterFridges.Select(fridge => new FridgeRenterModel
            {
                Id = fridge.Id,
                Model = fridge.Model.Name,
                Owner = fridge.Owner.Name,
                Producer = fridge.Producer.Name,
                Capacity = fridge.Capacity,
                CurrentCount = repository.FridgeProduct.GetAllProductsInTheFridgeAsync(fridge.Id).Result
                                                       .Select(f => f.Count)
                                                       .Sum(),
            }).ToList();

            await repository.SaveAsync();

            return fridges;
        }

        public async Task AddFridge(AddFridgeOwnerModel ownerAddFridgeDto)
        {
            owner = await tokenInfo.GetOwner();

            var fridge = new AddFridgeModel
            {
                FridgeId = Guid.NewGuid(),
                Capacity = ownerAddFridgeDto.Capacity,
                ModelId = ownerAddFridgeDto.ModelId,
                OwnerId = owner!.Id,
                ProducerId = ownerAddFridgeDto.ProducerId,
            };

            repository.Fridge.AddFridge(fridge);
            await repository.SaveAsync();
        }

        public async Task RentFridge(Guid fridgeId)
        {
            renter = await tokenInfo.GetUser();

            var fridge = await repository.Fridge.GetFridgeByIdAsync(fridgeId);

            if (fridge is null)
            {
                logger.LogInformation($"Fridge with id {fridgeId} is not found.");
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

            repository.RentDocument.AddDocument(rentDocument);

            fridge.RenterId = renter.Id;
            fridge.RentDocumentId = rentDocument.Id;

            repository.Fridge.UpdateFridge(fridge);

            await repository.SaveAsync();
        }

        public async Task DeleteFridge(Guid fridgeId)
        {
            owner = await tokenInfo.GetOwner();

            var fridges = owner?.Fridges;

            if (fridges is null || fridges.Count() == 0)
            {
                logger.LogInformation($"Owner with id {owner!.Id} doesn't have a fridge with id {fridgeId}");
                throw new ArgumentException($"Owner with id {owner!.Id} doesn't have a fridge with id {fridgeId}");
            }

            var fridge = await repository.Fridge.GetFridgeByIdAsync(fridges.First().Id);

            if (fridge!.RenterId != null)
            {
                logger.LogInformation("Unable to delete a rented fridge.");
                throw new ArgumentException("Unable to delete a rented fridge.");
            }

            repository.Fridge.RemoveFridge(fridge!);
            await repository.SaveAsync();
        }

        public async Task ReturnFridge(Guid fridgeId)
        {
            renter = await tokenInfo.GetUser();

            var fridge = await repository.Fridge.GetFridgeByIdAsync(fridgeId);

            if (fridge is null)
            {
                logger.LogInformation($"Fridge with id {fridgeId} is not found.");
                throw new ArgumentException($"Fridge with id {fridgeId} is not found.");
            }

            var productsInFridge = await repository.FridgeProduct.GetAllProductsInTheFridgeAsync(fridgeId);
            productsInFridge.ToList().ForEach(product => repository.FridgeProduct.DeleteProduct(product));

            repository.RentDocument.RemoveDocument(fridge.RentDocument);

            //fridge.RentDocumentId = null;
            //fridge.RentDocument = null;
            fridge.RenterId = null;
            fridge.Renter = null;

            repository.Fridge.UpdateFridge(fridge);

            await repository.SaveAsync();
        }
    }
}
