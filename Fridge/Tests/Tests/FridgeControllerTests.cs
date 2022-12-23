using Fridge.Controllers;
using Fridge.Tests.Mocks;
using Microsoft.AspNetCore.Mvc;
using Fridge.Models.Responses;
using Fridge.Models.Requests;


namespace Fridge.Tests.Tests
{
    public class FridgeControllerTests
    {
        [Fact]
        public async Task GetFridgesAsync_HaveFridges_ShouldReturnOk()
        {
            var fakeFridgeService = new FridgeFakeService();
            var controller = new FridgeController(fakeFridgeService.Service);
            
            IEnumerable<FridgeModel> expectedFridges = new List<FridgeModel>()
            {
                new FridgeModel()
                {
                    Id = new Guid("203e97d9-37e4-47a1-83eb-1ef70d072c6f"),
                    Model = "Toshiba GR-RF610WE-PMS",
                    Owner = "veronika",
                    Producer = "Toshiba",
                    Capacity =  20,
                },
                new FridgeModel()
                {
                    Id = new Guid("332ddb8c-57d6-4bbb-a3ea-4d33f5f30fc7"),
                    Model = "Toshiba GR-RF610WE-PMS",
                    Owner = "veronika",
                    Producer = "Toshiba",
                    Capacity = 20,
                }
            };

            fakeFridgeService.Mock.Setup(s => s.GetFridges())
                .Returns(Task.FromResult(expectedFridges));

            var response = await controller.GetFridges();

            Assert.NotNull(response);
            
            var okResult = response as OkObjectResult;

            Assert.IsType<OkObjectResult>(okResult);
            Assert.Equal(200, okResult?.StatusCode);

            var fridges = okResult?.Value as List<FridgeModel>;

            Assert.IsType<List<FridgeModel>>(fridges);
            Assert.NotEmpty(fridges);
            Assert.Equal(new Guid("203e97d9-37e4-47a1-83eb-1ef70d072c6f"), fridges[0].Id);
            Assert.Equal("Toshiba GR-RF610WE-PMS", fridges[0].Model);
            Assert.Equal("veronika", fridges[0].Owner);
            Assert.Equal("Toshiba", fridges[0].Producer);
            Assert.Equal(20, fridges[0].Capacity);
        }

        [Fact]
        public async Task GetFridgesAsync_NoFridges_ShouldNotFoundResult()
        {
            // Arrange

            var fakeFridgeService = new FridgeFakeService();
            var controller = new FridgeController(fakeFridgeService.Service);

            // Act

            fakeFridgeService.Mock.Setup(s => s.GetFridges())
                .Throws(new ArgumentException("Fridges are not found."));

            var response = await controller.GetFridges();

            // Assert

            Assert.NotNull(response);
            Assert.IsType<NotFoundObjectResult>(response);
        }

        [Fact]
        public async Task GetFridgesAsync_FridgesListIsNull_ShouldReturnNotFound()
        {
            // Arrange

            var fakeFridgeService = new FridgeFakeService();
            var controller = new FridgeController(fakeFridgeService.Service);

            // Act

            fakeFridgeService.Mock.Setup(s => s.GetFridges())
                .Throws(new ArgumentException("Fridges are not found."));

            var response = await controller.GetFridges();

            // Assert

            Assert.NotNull(response);
            Assert.IsType<NotFoundObjectResult>(response);
        }

        [Fact]
        public async Task GetModelsAsync_HaveModels_ShouldReturnOk()
        {
            var fakeFridgeService = new FridgeFakeService();
            var controller = new FridgeController(fakeFridgeService.Service);

            IEnumerable<FridgeModelModel> actualModels = new List<FridgeModelModel>()
            {
                new FridgeModelModel
                {
                    Id = new Guid("F8A3B786-D4B2-49D7-953B-578729B55A35"),
                    Name = "Indesit ITR 5200 W",
                },
                new FridgeModelModel
                {
                    Id = new Guid("4A645006-5621-4536-9490-E1769FAC2F53"),
                    Name = "LG GA-B379SLUL",
                },
                new FridgeModelModel
                {
                    Id = new Guid("44DC042A-3453-4C17-A4D1-CD8C0AC9378C"),
                    Name = "ATLANT XM-4208-000",
                },
                new FridgeModelModel
                {
                    Id = new Guid("2182354C-D8CC-47BF-844F-4AAFABA1DBFE"),
                    Name = "ATLANT ул 4625-101 NL",
                },
                new FridgeModelModel
                {
                    Id = new Guid("AF96137E-0B17-41B5-A819-A5A23DA0FD97"),
                    Name = "Toshiba GR-RF610WE-PMS",
                }
            };

            fakeFridgeService.Mock.Setup(s => s.GetModels())
                .Returns(Task.FromResult(actualModels));

            var response = await controller.GetModels();

            Assert.NotNull(response);

            var okResult = response as OkObjectResult;

            Assert.NotNull(okResult);
            Assert.IsType<OkObjectResult>(okResult);
            Assert.Equal(200, okResult?.StatusCode);

            var models = okResult.Value as List<FridgeModelModel>;

            Assert.IsType<List<FridgeModelModel>>(models);
            Assert.NotNull(models);
            Assert.NotEmpty(models);
        }

        [Fact]
        public async Task GetProducersAsync_HaveProducers_ShouldReturnOk()
        {
            var fakeFridgeService = new FridgeFakeService();
            var controller = new FridgeController(fakeFridgeService.Service);
            
            IEnumerable<FridgeProducerModel> expectedProducers = new List<FridgeProducerModel>()
            {
                new FridgeProducerModel
                {
                    Id = new Guid("D347DFE3-5CF9-49E8-8137-8880580F203B"),
                    Name = "ATLANT",
                    Country = "Belarus"
                },
                new FridgeProducerModel
                {
                    Id = new Guid("A8000178-A46B-4122-8758-2931E99C46E9"),
                    Name = "Indesit",
                    Country = "Russia"
                },
                new FridgeProducerModel
                {
                    Id = new Guid("38886C70-4593-47CE-9CD1-99D9831C2EB4"),
                    Name = "LG",
                    Country = "Russia"
                },
                new FridgeProducerModel
                {
                    Id = new Guid("0D08C561-361C-497E-BD21-06A7CE7D5516"),
                    Name = "Toshiba",
                    Country = "China"
                },
                new FridgeProducerModel
                {
                    Id = new Guid("8E652090-8FA2-4271-8E05-7934A0BA77A7"),
                    Name = "BEKO",
                    Country = "Russia"
                }
            };

            fakeFridgeService.Mock.Setup(s => s.GetProducers())
                .Returns(Task.FromResult(expectedProducers));

            var response = await controller.GetModels();

            Assert.NotNull(response);
            Assert.IsType<OkObjectResult>(response);

            var okResult = response as OkObjectResult;
            
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult?.StatusCode);
        }

        [Fact]
        public async Task RentFridge_ValidData_ShouldReturnOk()
        {
            var fakeFridgeService = new FridgeFakeService();
            var controller = new FridgeController(fakeFridgeService.Service);
            
            var fridgeId = Guid.NewGuid();

            var addFridgeModel = new AddFridgeModel()
            {
                FridgeId = fridgeId,
                ModelId = Guid.NewGuid(),
                ProducerId = Guid.NewGuid(),
                OwnerId = Guid.NewGuid(),
                Capacity = 20,
            };

            fakeFridgeService.Mock.Setup(s => s.RentFridge(fridgeId))
                .Returns(Task.FromResult(addFridgeModel));

            var response = await controller.RentFridge(fridgeId);

            Assert.NotNull(response);
            Assert.IsType<OkResult>(response);

            var createdResult = response as OkResult;

            Assert.NotNull(createdResult);
            Assert.Equal(200, createdResult?.StatusCode);
        }

        [Fact]
        public async Task RemoveFridge_ValidId_ShouldReturnOk()
        {
            // Arrange

            var fakeFridgeService = new FridgeFakeService();
            var controller = new FridgeController(fakeFridgeService.Service);

            var fridgeId = Guid.NewGuid();

            // Act

            var response = await controller.Remove(fridgeId);

            Assert.NotNull(response);
            Assert.IsType<OkResult>(response);

            var okResult = response as OkResult;

            // Assert

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult?.StatusCode);
        }

        [Fact]
        public async Task AddFridge_ValidData_ShouldReturnOk()
        {
            // Arrange

            var fakeFridgeService = new FridgeFakeService();

            var controller = new FridgeController(fakeFridgeService.Service);

            var addFridgeOwnerModel = new AddFridgeOwnerModel()
            {
                ModelId = Guid.NewGuid().ToString(),
                ProducerId = Guid.NewGuid().ToString(),
                Capacity = 20,
            };

            var addFridgeModel = new AddFridgeModel()
            {
                FridgeId = Guid.NewGuid(),
                ModelId = new Guid(addFridgeOwnerModel.ModelId),
                ProducerId = new Guid(addFridgeOwnerModel.ProducerId),
                OwnerId = Guid.NewGuid(),
                Capacity = 20,
            };

            // Act

            fakeFridgeService.Mock.Setup(s => s.AddFridge(addFridgeOwnerModel))
                .Returns(Task.FromResult(addFridgeModel));

            var response = await controller.AddFridge(addFridgeOwnerModel);

            Assert.NotNull(response);
            Assert.IsType<CreatedResult>(response);

            var createdResult = response as CreatedResult;

            // Assert

            Assert.NotNull(createdResult);
            Assert.Equal(201, createdResult?.StatusCode);
        }

        [Fact]
        public async Task DeleteFridge_ValidId_ShouldReturnOk()
        {
            // Arrange

            var fakeFridgeService = new FridgeFakeService();
            var controller = new FridgeController(fakeFridgeService.Service);

            var fridgeId = Guid.NewGuid();

            // Act

            var response = await controller.DeleteFridge(fridgeId);

            Assert.NotNull(response);
            Assert.IsType<OkResult>(response);

            var okResult = response as OkResult;

            // Assert

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult?.StatusCode);
        }

        [Fact]
        public async Task GetOwnersFridgesAsync_HaveFridges_ShouldReturnOk()
        {
            // Arrange

            var fakeFridgeService = new FridgeFakeService();
            var controller = new FridgeController(fakeFridgeService.Service);

            IEnumerable<IUserFridgeModel> actualFridges = new List<OwnerFridgeModel>()
            {
                new OwnerFridgeModel
                {
                    Id = new Guid("203e97d9-37e4-47a1-83eb-1ef70d072c6f"),
                    Model = "Toshiba GR-RF610WE-PMS",
                    Owner = "veronika",
                    Producer = "Toshiba",
                    Capacity =  20,
                },
                new OwnerFridgeModel
                {
                    Id = new Guid("332ddb8c-57d6-4bbb-a3ea-4d33f5f30fc7"),
                    Model = "Toshiba GR-RF610WE-PMS",
                    Owner = "veronika",
                    Producer = "Toshiba",
                    Capacity = 20,
                }
            };

            // Act

            fakeFridgeService.Mock.Setup(s => s.GetUserFridges())
                .Returns(Task.FromResult(actualFridges));

            var response = await controller.GetUserFridges();

            Assert.NotNull(response);
            Assert.IsType<OkObjectResult>(response);

            var okResult = response as OkObjectResult;

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult?.StatusCode);

            var fridges = okResult?.Value as List<OwnerFridgeModel>;

            // Assert

            Assert.IsType<List<OwnerFridgeModel>>(fridges);
            Assert.NotEmpty(fridges);
            Assert.Equal(new Guid("203e97d9-37e4-47a1-83eb-1ef70d072c6f"), fridges[0].Id);
            Assert.Equal("Toshiba GR-RF610WE-PMS", fridges[0].Model);
            Assert.Equal("veronika", fridges[0].Owner);
            Assert.Equal("Toshiba", fridges[0].Producer);
            Assert.Equal(20, fridges[0].Capacity);
        }

        [Fact]
        public async Task GetRentersFridgesAsync_HaveFridges_ShouldReturnOk()
        {
            // Arrange

            var fakeFridgeService = new FridgeFakeService();

            var controller = new FridgeController(fakeFridgeService.Service);

            IEnumerable<IUserFridgeModel> actualFridges = new List<FridgeRenterModel>()
            {
                new FridgeRenterModel()
                {
                    Id = new Guid("203e97d9-37e4-47a1-83eb-1ef70d072c6f"),
                    Model = "Toshiba GR-RF610WE-PMS",
                    Owner = "veronika",
                    Producer = "Toshiba",
                    Capacity =  20,
                    CurrentCount =  0,
                },
                new FridgeRenterModel()
                {
                    Id = new Guid("332ddb8c-57d6-4bbb-a3ea-4d33f5f30fc7"),
                    Model = "Toshiba GR-RF610WE-PMS",
                    Owner = "veronika",
                    Producer = "Toshiba",
                    Capacity = 20,
                    CurrentCount = 0,
                }
            };

            // Act

            fakeFridgeService.Mock.Setup(s => s.GetUserFridges())
                .Returns(Task.FromResult(actualFridges));

            var response = await controller.GetUserFridges();

            Assert.NotNull(response);
            Assert.IsType<OkObjectResult>(response);

            var okResult = response as OkObjectResult;

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult?.StatusCode);

            var fridges = okResult?.Value as List<FridgeRenterModel>;

            // Assert

            Assert.IsType<List<FridgeRenterModel>>(fridges);
            Assert.NotEmpty(fridges);
            Assert.Equal(new Guid("203e97d9-37e4-47a1-83eb-1ef70d072c6f"), fridges[0].Id);
            Assert.Equal("Toshiba GR-RF610WE-PMS", fridges[0].Model);
            Assert.Equal("veronika", fridges[0].Owner);
            Assert.Equal("Toshiba", fridges[0].Producer);
            Assert.Equal(20, fridges[0].Capacity);
        }
    }
}
