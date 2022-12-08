using Fridge.Data.Models;
using Fridge.Data.Repositories.Interfaces;
using Fridge.Models.DTOs.FridgeDto;
using Fridge.Models.DTOs.FridgeDtos;
using Fridge.Models.DTOs.OwnerDtos;
using Fridge.Services.Abstracts;


namespace Fridge.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly ILogger<OwnerService> _logger;
        private readonly IRepositoryManager _repository;

        private readonly TokenInfo tokenInfo;
        private Owner? _owner;

        public OwnerService(IRepositoryManager repository, IHttpContextAccessor httpContextAccessor, ILogger<OwnerService> logger)
        {
            _repository = repository;
            _logger = logger;

            tokenInfo = new TokenInfo(repository, httpContextAccessor);
        }

        public async Task<IEnumerable<FridgeDto>> GetOwnersFridges()
        {
            _owner = await tokenInfo.GetOwner();

            var fridges = await _repository.Fridge.GetFridgeByConditionAsync(f => f.OwnerId == _owner.Id);

            if (fridges?.Count() == 0 || fridges is null)
            {
                _logger.LogInformation($"Owner with id {_owner!.Id} doesn't have any fridges.");
                throw new ArgumentException("Owner doesn't have any fridges.");
            }

            Func<Guid?, Renter?> getRenter = _repository.Renter.GetRenterById;
            
            Renter? renter = new Renter();

            var fridgesDto = fridges.Select(fridge => new FridgeDto
            {
                Id = fridge.Id,
                Model = _repository.Model.GetModelByIdAsync(fridge.ModelId).Name,
                Owner = _repository.Owner.GetOwnerByIdAsync(fridge.OwnerId).Result.Name,
                Producer = _repository.Producer.GetProducerByIdAsync(fridge.ProducerId).Result.Name,
                Renter = (renter = getRenter(fridge.RenterId)) is null ? null : renter.Email,
                Capacity = fridge.Capacity,
            });

            return fridgesDto;
        }

        public async Task<RentDocumentDto> GetRentedFridgeInfo(Guid fridgeId)
        {
            _owner = await tokenInfo.GetOwner();

            var rentDocument = new RentDocument();

            var fridge = await _repository.Fridge.GetFridgeByIdAsync(fridgeId);

            if (fridge is null)
            {
                throw new ArgumentException("The fridge is not found...");
            }

            if (fridge.RenterId is null)
            {
                throw new ArgumentException("The fridge is not rented...");
            }

            var rentedFridgeRentDocumentId = fridge.RentDocumentId;

            rentDocument = _repository.RentDocument.FindDocumentByCondition(d => d.Id == rentedFridgeRentDocumentId);

            var rentDocumentDto = new RentDocumentDto
            {
                Id = rentDocument.Id,
                RenterEmail = _repository.Renter.FindRenterByCondition(u => u.Id == rentDocument.RenterId).Email,
                OwnerName = _owner!.Name,
                StartDate = rentDocument.StartDate.ToShortDateString(),
                EndDate = rentDocument.EndDate.ToShortDateString(),
                MonthCost = rentDocument.MonthCost,
            };

            return rentDocumentDto;
        }

        public async Task<FridgeServicePartDto> AddFridge(OwnerAddFridgeDto ownerAddFridgeDto)
        {
            _owner = await tokenInfo.GetOwner();

            var fridge = new FridgeServicePartDto
            {
                FridgeId = Guid.NewGuid(),
                Capacity = ownerAddFridgeDto.Capacity,
                ModelId = ownerAddFridgeDto.ModelId,
                OwnerId = _owner!.Id,
                ProducerId = ownerAddFridgeDto.ProducerId,
            };

            _repository.Fridge.AddFridge(fridge);
            await _repository.SaveAsync();

            return fridge;
        }

        public async Task DeleteFridge(Guid fridgeId)
        {
            _owner = await tokenInfo.GetOwner();

            var fridges = await _repository.Fridge.GetFridgeByConditionAsync(f => f.Id == fridgeId && f.OwnerId == _owner.Id);

            if (fridges is null || fridges.Count() == 0)
            {
                _logger.LogInformation($"Owner with id {_owner!.Id} doesn't have a fridge with id {fridgeId}");
                throw new ArgumentException($"Owner with id {_owner!.Id} doesn't have a fridge with id {fridgeId}");
            }

            var fridge = await _repository.Fridge.GetFridgeByIdAsync(fridges.First().Id);

            if (fridge!.RenterId != null)
            {
                _logger.LogInformation("Unable to delete a rented fridge.");
                throw new ArgumentException("Unable to delete a rented fridge.");
            }

            _repository.Fridge.RemoveFridge(fridge!);
            await _repository.SaveAsync();
        }
    }
}
