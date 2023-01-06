using Fridge.Data.Models;
using Fridge.Data.Repositories.Interfaces;
using Fridge.Services.Abstracts;
using Fridge.Models.Responses;
using Fridge.Models.Requests;
using AutoMapper;
using Fridge.Models;

namespace Fridge.Services
{
    public class FridgeService : IFridgeService
    {
        private readonly IRepositoryManager repository;
        private readonly IMapper mapper;

        private readonly TokenInfo tokenInfo;
        private User? user;

        public FridgeService(IRepositoryManager repository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            this.repository = repository;
            this.mapper = mapper;

            tokenInfo = new TokenInfo(repository, httpContextAccessor, configuration);
        }

        public async Task<IEnumerable<FridgeModel>> GetFridges()
        {
            var fridges = await repository.Fridge.GetAvailableFridgesAsync();

            if (!fridges.Any() || fridges is null)
            {
                return new List<FridgeModel>() { };
            }

            var fridgesDto = fridges.Select(fridge => new FridgeModel
            {
                Id = fridge.Id,
                Model = fridge.Model.Name,
                Owner = fridge.Owner.Email,
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

        public async Task<IEnumerable<IUserFridgeModel>> GetUserFridges()
        {
            user = await tokenInfo.GetUser();

            var fridges = user.Role == UserRoles.Renter
                ? await GetRentersFridges()
                : await GetOwnersFridges();

            return fridges;
        }

        public async Task AddFridge(AddFridgeOwnerModel ownerAddFridgeDto)
        {
            user = await tokenInfo.GetUser();

            var fridge = new AddFridgeModel
            {
                FridgeId = Guid.NewGuid(),
                Capacity = ownerAddFridgeDto.Capacity,
                ModelId = new Guid(ownerAddFridgeDto.ModelId),
                OwnerId = user!.Id,
                ProducerId = new Guid(ownerAddFridgeDto.ProducerId),
            };

            repository.Fridge.AddFridge(fridge);
            await repository.SaveAsync();
        }

        public async Task RentFridge(Guid fridgeId)
        {
            user = await tokenInfo.GetUser();

            var fridge = await repository.Fridge.GetFridgeByIdAsync(fridgeId);

            if (fridge is null)
            {
                throw new ArgumentException($"Fridge with id {fridgeId} is not found.");
            }

            fridge.RenterId = user.Id;
            
            repository.Fridge.UpdateFridge(fridge);

            await repository.SaveAsync();
        }

        public async Task DeleteFridge(Guid fridgeId)
        {
            user = await tokenInfo.GetUser();

            var fridges = user?.OwnerFridges;

            if (fridges is null || fridges.Count() == 0)
            {
                throw new ArgumentException($"Owner with id {user!.Id} doesn't have a fridge with id {fridgeId}");
            }

            var fridge = await repository.Fridge.GetFridgeByIdAsync(fridges.First().Id);

            if (fridge!.RenterId != null)
            {
                throw new ArgumentException("Unable to delete a rented fridge.");
            }

            repository.Fridge.RemoveFridge(fridge!);
            await repository.SaveAsync();
        }

        public async Task ReturnFridge(Guid fridgeId)
        {
            user = await tokenInfo.GetUser();

            var fridge = await repository.Fridge.GetFridgeByIdAsync(fridgeId);

            if (fridge is null)
            {
                throw new ArgumentException($"Fridge with id {fridgeId} is not found.");
            }

            var productsInFridge = await repository.FridgeProduct.GetAllProductsInTheFridgeAsync(fridgeId);
            productsInFridge.ToList().ForEach(product => repository.FridgeProduct.DeleteProduct(product));

            fridge.RenterId = null;
            fridge.Renter = null;

            repository.Fridge.UpdateFridge(fridge);

            await repository.SaveAsync();
        }

        private async Task<IEnumerable<IUserFridgeModel>> GetOwnersFridges()
        {
            user = await tokenInfo.GetUser();

            var fridges = user?.OwnerFridges;

            if (fridges?.Count() == 0 || fridges is null)
            {
                throw new ArgumentException("Owner doesn't have any fridges.");
            }

            var fridgesDto = fridges.Select(fridge => new OwnerFridgeModel
            {
                Id = fridge.Id,
                Model = fridge.Model.Name,
                Owner = fridge.Owner.Email,
                Producer = fridge.Producer.Name,
                Renter = fridge.Renter is null ? "No renter" : fridge.Renter.Email,
                Capacity = fridge.Capacity,
            });

            return fridgesDto;
        }

        private async Task<IEnumerable<FridgeRenterModel>> GetRentersFridges()
        {
            user = await tokenInfo.GetUser();

            var renterFridges = user?.RenterFridges;

            if (!renterFridges.Any())
            {
                throw new ArgumentException("No fridges");
            }

            var fridges = renterFridges.Select(fridge => new FridgeRenterModel
            {
                Id = fridge.Id,
                Model = fridge.Model.Name,
                Owner = fridge.Owner.Email,
                Producer = fridge.Producer.Name,
                Capacity = fridge.Capacity,
                CurrentCount = repository.FridgeProduct.GetAllProductsInTheFridgeAsync(fridge.Id).Result
                                                       .Select(f => f.Count)
                                                       .Sum(),
            }).ToList();

            await repository.SaveAsync();

            return fridges;
        }
    }
}
