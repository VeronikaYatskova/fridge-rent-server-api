using Fridge.Controllers;
using Fridge.Models.DTOs.FridgeDto;
using Fridge.Models.DTOs.FridgeDtos;
using Fridge.Tests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace Fridge.Tests.Tests
{
    public class RentControllerTests
    {
        [Fact]
        public async Task GetUsersFridgesAsync_HaveFridges_ShouldReturnOk()
        {
            // Arrange

            var fakeService = new RentFakeServer();

            var controller = new RentController(fakeService.Service);

            IEnumerable<FridgeDto> actualFridges = new List<FridgeDto>()
            {
                new FridgeDto()
                {
                    Id = new Guid("203e97d9-37e4-47a1-83eb-1ef70d072c6f"),
                    Model = "Toshiba GR-RF610WE-PMS",
                    Owner = "veronika",
                    Producer = "Toshiba",
                    Capacity =  20,
                    CurrentCount =  0,
                    isRented = false
                },
                new FridgeDto()
                {
                    Id = new Guid("332ddb8c-57d6-4bbb-a3ea-4d33f5f30fc7"),
                    Model = "Toshiba GR-RF610WE-PMS",
                    Owner = "veronika",
                    Producer = "Toshiba",
                    Capacity = 20,
                    CurrentCount = 0,
                    isRented = false
                }
            };

            // Act

            fakeService.Mock.Setup(s => s.GetUsersFridges())
                .Returns(Task.FromResult(actualFridges));

            var response = await controller.GetUsersFridges();

            var okResult = response as OkObjectResult;

            var fridges = okResult?.Value as List<FridgeDto>;

            // Assert

            Assert.Equal(200, okResult?.StatusCode);
            Assert.NotEmpty(fridges);
        }

        [Fact]
        public async Task RentFridge_ValidData_ShouldReturnOk()
        {
            // Arrange

            var fakeService = new RentFakeServer();

            var controller = new RentController(fakeService.Service);

            var fridgeId = Guid.NewGuid();

            var fridgeServicePartDto = new FridgeServicePartDto()
            {
                FridgeId = fridgeId,
                ModelId = Guid.NewGuid(),
                ProducerId = Guid.NewGuid(),
                OwnerId = Guid.NewGuid(),
                Capacity = 20,
            };

            // Act

            fakeService.Mock.Setup(s => s.RentFridge(fridgeId))
                .Returns(Task.FromResult(fridgeServicePartDto));

            var response = await controller.RentFridge(fridgeId);

            var createdResult = response as OkResult;

            // Assert

            Assert.Equal(200, createdResult?.StatusCode);
        }

        [Fact]
        public async Task RemoveFridge_ValidId_ShouldReturnOk()
        {
            // Arrange

            var fakeService = new RentFakeServer();

            var controller = new RentController(fakeService.Service);

            var fridgeId = Guid.NewGuid();

            // Act

            var response = await controller.Remove(fridgeId);

            var okResult = response as OkResult;

            // Assert

            Assert.Equal(200, okResult?.StatusCode);
        }
    }
}
