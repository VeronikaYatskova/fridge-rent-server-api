using Fridge.Controllers;
using Fridge.Models.DTOs.FridgeDto;
using Fridge.Models.DTOs.FridgeDtos;
using Fridge.Models.DTOs.OwnerDtos;
using Fridge.Tests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace Fridge.Tests.Tests
{
    public class OwnerControllerTests
    {
        [Fact]
        public async Task GetOwnersFridgesAsync_HaveFridges_ShouldReturnOk()
        {
            // Arrange

            var fakeService = new OwnerFakeService();

            var controller = new OwnerController(fakeService.Service);

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

            fakeService.Mock.Setup(s => s.GetOwnersFridges())
                .Returns(Task.FromResult(actualFridges));

            var response = await controller.GetOwnersFridges();

            var okResult = response as OkObjectResult;

            var fridges = okResult?.Value as List<FridgeDto>;

            // Assert

            Assert.Equal(200, okResult?.StatusCode);
            Assert.NotEmpty(fridges);
        }

        [Fact]
        public async Task GetRentedFridgeInfoAsync_ValidId_ShouldReturnOk()
        {
            // Arrange

            var fakeService = new OwnerFakeService();

            var controller = new OwnerController(fakeService.Service);

            var fridgeId = new Guid("203e97d9-37e4-47a1-83eb-1ef70d072c6f");
            
            // Act

            fakeService.Mock.Setup(s => s.GetRentedFridgeInfo(fridgeId))
                .Returns(Task.FromResult(new RentDocumentDto()
                {
                    Id = Guid.NewGuid(),
                    RenterEmail = "veronika@renter.com",
                    OwnerName = "Sasha",
                    StartDate = "12.11.2022",
                    EndDate = "12.12.2022",
                    MonthCost = 30,
                }));

            var response = await controller.GetRentedFridgeInfo(fridgeId);

            var okResult = response as OkObjectResult;

            var rentDocument = okResult?.Value as RentDocumentDto;

            // Assert

            Assert.Equal(200, okResult?.StatusCode);
            Assert.NotNull(rentDocument);
        }

        [Fact]
        public async Task AddFridge_ValidData_ShouldReturnOk()
        {
            // Arrange

            var fakeService = new OwnerFakeService();

            var controller = new OwnerController(fakeService.Service);

            var ownerAddFridgeDto = new OwnerAddFridgeDto()
            {
                ModelId = Guid.NewGuid(),
                ProducerId = Guid.NewGuid(),
                Capacity = 20,
            };

            var fridgeServicePartDto = new FridgeServicePartDto()
            {
                FridgeId = Guid.NewGuid(),
                ModelId = ownerAddFridgeDto.ModelId,
                ProducerId = ownerAddFridgeDto.ProducerId,
                OwnerId = Guid.NewGuid(),
                Capacity = 20,
            };

            // Act

            fakeService.Mock.Setup(s => s.AddFridge(ownerAddFridgeDto))
                .Returns(Task.FromResult(fridgeServicePartDto));

            var response = await controller.AddFridge(ownerAddFridgeDto);

            var createdResult = response as CreatedResult;

            var newFridge = createdResult?.Value as FridgeServicePartDto;

            // Assert

            Assert.Equal(201, createdResult?.StatusCode);
            Assert.NotNull(newFridge);
        }

        [Fact]
        public async Task DeleteFridge_ValidId_ShouldReturnOk()
        {
            // Arrange

            var fakeService = new OwnerFakeService();

            var controller = new OwnerController(fakeService.Service);

            var fridgeId = Guid.NewGuid();
            // Act

            var response = await controller.DeleteFridge(fridgeId);

            var okResult = response as OkResult;

            // Assert

            Assert.Equal(200, okResult?.StatusCode);
        }
    }
}
