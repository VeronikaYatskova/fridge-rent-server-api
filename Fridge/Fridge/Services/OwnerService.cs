using Fridge.Controllers;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Owner _owner;

        public OwnerService(IRepositoryManager repository, IHttpContextAccessor httpContextAccessor, ILogger<OwnerService> logger)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;

            if (_owner is null)
            {
                var tokenInfo = new TokenInfo(repository, httpContextAccessor);
                _owner = tokenInfo.GetOwner().Result;
            }
        }

        public async Task<IEnumerable<FridgeDto>> GetOwnersFridges()
        {
            var fridges = await _repository.Fridge.GetFridgeByConditionAsync(f => f.OwnerId == _owner.Id, trackChanges: false);

            if (fridges?.Count() == 0 || fridges is null)
            {
                _logger.LogInformation($"Owner with id {_owner.Id} doesn't have any fridges.");
                throw new ArgumentException("Owner doesn't have any fridges.");
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

        public async Task<RentDocumentDto> GetRentedFridgeInfo(Guid fridgeId)
        {
            var rentDocument = new RentDocument();

            var rentedFridgeData = _repository.UserFridge.GetFridgeById(fridgeId, trackChanges: false);
            if (rentedFridgeData is null)
            {
                throw new ArgumentException("The fridge is not rented...");
            }

            var rentedFridgeRentDocumentId = rentedFridgeData.RentDocumentId;

            rentDocument = _repository.RentDocument.FindDocumentByCondition(d => d.Id == rentedFridgeRentDocumentId, trackChanges: false);

            var rentDocumentDto = new RentDocumentDto
            {
                Id = rentDocument.Id,
                RenterEmail = _repository.User.FindUserByCondition(u => u.Id == rentDocument.UserId, trackChanges: false).Email,
                OwnerName = _owner.Name,
                StartDate = rentDocument.StartDate.ToShortDateString(),
                EndDate = rentDocument.EndDate.ToShortDateString(),
                MonthCost = rentDocument.MonthCost,
            };

            return rentDocumentDto;
        }

        public async Task<FridgeServicePartDto> AddFridge(OwnerAddFridgeDto ownerAddFridgeDto)
        {
            var fridge = new FridgeServicePartDto
            {
                FridgeId = Guid.NewGuid(),
                Capacity = ownerAddFridgeDto.Capacity,
                ModelId = ownerAddFridgeDto.ModelId,
                OwnerId = _owner.Id,
                ProducerId = ownerAddFridgeDto.ProducerId,
            };

            _repository.Fridge.AddFridge(fridge);
            await _repository.SaveAsync();

            return fridge;
        }

        public async Task DeleteFridge(Guid fridgeId)
        {
            var fridges = await _repository.Fridge.GetFridgeByConditionAsync(f => f.Id == fridgeId && f.OwnerId == _owner.Id, trackChanges: false);

            if (fridges is null || fridges.Count() == 0)
            {
                _logger.LogInformation($"Owner with id {_owner.Id} doesn't have a fridge with id {fridgeId}");
                throw new ArgumentException($"Owner with id {_owner.Id} doesn't have a fridge with id {fridgeId}");
            }

            var fridge = await _repository.Fridge.GetFridgeByIdAsync(fridges.First().Id, trackChanges: false);

            if (fridge!.IsRented == true)
            {
                _logger.LogInformation("Unable to delete a rented fridge.");
                throw new ArgumentException("Unable to delete a rented fridge.");
            }

            _repository.Fridge.RemoveFridge(fridge!);
            await _repository.SaveAsync();
        }
    }
}
