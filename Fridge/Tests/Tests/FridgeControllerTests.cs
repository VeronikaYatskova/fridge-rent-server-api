using Fridge.Controllers;
using Fridge.Data.Models;
using Fridge.Models.DTOs.FridgeDtos;
using Fridge.Tests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace Fridge.Tests.Tests
{
    public class FridgeControllerTests
    {
        [Fact]
        public async Task GetFridgesAsync_HaveFridges_ShouldReturnOk()
        {
            // Arrange

            var fakeService = new FridgeFakeService();

            var controller = new FridgeController(fakeService.Service);

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

            fakeService.Mock.Setup(s => s.GetFridges())
                .Returns(Task.FromResult(actualFridges));

            var response = await controller.GetFridges();

            var okResult = response as OkObjectResult;

            var fridges = okResult?.Value as List<FridgeDto>;

            // Assert

            Assert.Equal(200, okResult?.StatusCode);
            Assert.NotEmpty(fridges);
        }

        [Fact]
        public async Task GetFridgesAsync_NoFridges_ShouldReturnOk()
        {
            // Arrange

            var fakeService = new FridgeFakeService();

            var controller = new FridgeController(fakeService.Service);

            IEnumerable<FridgeDto> actualFridges = new List<FridgeDto>() { };

            // Act

            fakeService.Mock.Setup(s => s.GetFridges())
                .Returns(Task.FromResult(actualFridges));

            var response = await controller.GetFridges();

            var okResult = response as OkObjectResult;

            var fridges = okResult.Value as List<FridgeDto>;

            // Assert

            Assert.Equal(200, okResult.StatusCode);
            Assert.Empty(fridges);
        }

        [Fact]
        public async Task GetFridgesAsync_FridgesListIsNull_ShouldReturnNotFound()
        {
            // Arrange

            var fakeService = new FridgeFakeService();

            var controller = new FridgeController(fakeService.Service);

            // Act

            fakeService.Mock.Setup(s => s.GetFridges())
                .Throws(new ArgumentException());

            var response = await controller.GetFridges();

            var notFoundResult = response as NotFoundResult;

            // Assert

            Assert.Equal(404, notFoundResult?.StatusCode);
        }

        [Fact]
        public async Task GetModelsAsync_HaveModels_ShouldReturnOk()
        {
            // Arrange

            var fakeService = new FridgeFakeService();

            var controller = new FridgeController(fakeService.Service);

            IEnumerable<Model> actualModels = new List<Model>()
            {
                new Model
                {
                    Id = new Guid("F8A3B786-D4B2-49D7-953B-578729B55A35"),
                    Name = "Indesit ITR 5200 W",
                },
                new Model
                {
                    Id = new Guid("4A645006-5621-4536-9490-E1769FAC2F53"),
                    Name = "LG GA-B379SLUL",
                },
                new Model
                {
                    Id = new Guid("44DC042A-3453-4C17-A4D1-CD8C0AC9378C"),
                    Name = "ATLANT XM-4208-000",
                },
                new Model
                {
                    Id = new Guid("2182354C-D8CC-47BF-844F-4AAFABA1DBFE"),
                    Name = "ATLANT ул 4625-101 NL",
                },
                new Model
                {
                    Id = new Guid("AF96137E-0B17-41B5-A819-A5A23DA0FD97"),
                    Name = "Toshiba GR-RF610WE-PMS",
                }
            };

            // Act

            fakeService.Mock.Setup(s => s.GetModels())
                .Returns(Task.FromResult(actualModels));

            var response = await controller.GetModels();

            var okResult = response as OkObjectResult;

            // Assert

            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task GetProducersAsync_HaveProducers_ShouldReturnOk()
        {
            // Arrange

            var fakeService = new FridgeFakeService();

            var controller = new FridgeController(fakeService.Service);

            IEnumerable<Producer> actualProducers = new List<Producer>()
            {
                new Producer
                {
                    Id = new Guid("D347DFE3-5CF9-49E8-8137-8880580F203B"),
                    Name = "ATLANT",
                    Country = "Belarus"
                },
                new Producer
                {
                    Id = new Guid("A8000178-A46B-4122-8758-2931E99C46E9"),
                    Name = "Indesit",
                    Country = "Russia"
                },
                new Producer
                {
                    Id = new Guid("38886C70-4593-47CE-9CD1-99D9831C2EB4"),
                    Name = "LG",
                    Country = "Russia"
                },
                new Producer
                {
                    Id = new Guid("0D08C561-361C-497E-BD21-06A7CE7D5516"),
                    Name = "Toshiba",
                    Country = "China"
                },
                new Producer
                {
                    Id = new Guid("8E652090-8FA2-4271-8E05-7934A0BA77A7"),
                    Name = "BEKO",
                    Country = "Russia"
                }
            };

            // Act

            fakeService.Mock.Setup(s => s.GetProducers())
                .Returns(Task.FromResult(actualProducers));

            var response = await controller.GetModels();

            var okResult = response as OkObjectResult;

            // Assert

            Assert.Equal(200, okResult.StatusCode);
        }
    }
}