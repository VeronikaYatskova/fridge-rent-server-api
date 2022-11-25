﻿using Fridge.Controllers;
using Fridge.Data.Models;
using Fridge.Data.Repositories.Interfaces;
using Fridge.Models.DTOs;
using Fridge.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace Fridge.Services
{
    public class RentService : IRentService
    {
        private readonly ILogger<RentService> _logger;
        private readonly IRepositoryManager _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly User user;

        public RentService(IRepositoryManager repository, IHttpContextAccessor httpContextAccessor, ILogger<RentService> logger)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;

            var tokenInfo = new TokenInfo(repository, httpContextAccessor);
            user = tokenInfo.GetUser().Result;
        }

        public async Task<IEnumerable<FridgeDto>> GetUsersFridges()
        {
            var userFridges = _repository.UserFridge.GetUserFridge(user.Id, trackChanges: false);

            if (!userFridges.Any())
            {
                _logger.LogInformation("No fridges");
                throw new ArgumentException("No fridges");
            }

            var allFridges = await _repository.Fridge.GetFridgesAsync(trackChanges: false);
            var fr = allFridges.Join(userFridges,
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
                UserId = user.Id,
                FridgeId = fridgeId,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(12),
                MonthCost = 30,
            };

            var userFridge = new UserFridge
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                FridgeId = fridgeId,
                RentDocumentId = rentDocument.Id,
                RentDocument = rentDocument,
            };

            _repository.RentDocument.AddDocument(rentDocument);
            _repository.UserFridge.RentFridge(userFridge);

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

            var userFridge = _repository.UserFridge.GetUserFridgeRow(user.Id, fridgeId, trackChanges: false);
            _repository.UserFridge.RemoveFridge(userFridge);

            await _repository.SaveAsync();
        }
    }
}