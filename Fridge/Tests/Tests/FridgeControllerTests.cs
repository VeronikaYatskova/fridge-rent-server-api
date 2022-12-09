using Fridge.Controllers;
using Fridge.Data.Models;
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
            // Arrange

            var fakeFridgeService = new FridgeFakeService();
            var fakeFridgeProductService = new FridgeProductFakeService();
            
            var controller = new FridgeController
            (
                fakeFridgeService.Service,
                fakeFridgeProductService.Service
            );

            IEnumerable<FridgeModel> actualFridges = new List<FridgeModel>()
            {
                new FridgeModel()
                {
                    Id = new Guid("203e97d9-37e4-47a1-83eb-1ef70d072c6f"),
                    Model = "Toshiba GR-RF610WE-PMS",
                    Owner = "veronika",
                    Producer = "Toshiba",
                    Capacity =  20,
                    CurrentCount =  0,
                },
                new FridgeModel()
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

            fakeFridgeService.Mock.Setup(s => s.GetFridges())
                .Returns(Task.FromResult(actualFridges));

            var response = await controller.GetFridges();

            var okResult = response as OkObjectResult;

            var fridges = okResult?.Value as List<FridgeModel>;

            // Assert

            Assert.Equal(200, okResult?.StatusCode);
            Assert.NotEmpty(fridges);
        }

        [Fact]
        public async Task GetFridgesAsync_NoFridges_ShouldReturnOk()
        {
            // Arrange

            var fakeFridgeService = new FridgeFakeService();
            var fakeFridgeProductService = new FridgeProductFakeService();
            
            var controller = new FridgeController
            (
                fakeFridgeService.Service,
                fakeFridgeProductService.Service
            );

            IEnumerable<FridgeModel> actualFridges = new List<FridgeModel>() { };

            // Act

            fakeFridgeService.Mock.Setup(s => s.GetFridges())
                .Returns(Task.FromResult(actualFridges));

            var response = await controller.GetFridges();

            var okResult = response as OkObjectResult;

            var fridges = okResult.Value as List<FridgeModel>;

            // Assert

            Assert.Equal(200, okResult.StatusCode);
            Assert.Empty(fridges);
        }

        [Fact]
        public async Task GetFridgesAsync_FridgesListIsNull_ShouldReturnNotFound()
        {
            // Arrange

            var fakeFridgeService = new FridgeFakeService();
            var fakeFridgeProductService = new FridgeProductFakeService();
            
            var controller = new FridgeController
            (
                fakeFridgeService.Service,
                fakeFridgeProductService.Service
            );

            // Act

            fakeFridgeService.Mock.Setup(s => s.GetFridges())
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

            var fakeFridgeService = new FridgeFakeService();
            var fakeFridgeProductService = new FridgeProductFakeService();
            
            var controller = new FridgeController
            (
                fakeFridgeService.Service,
                fakeFridgeProductService.Service
            );

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

            fakeFridgeService.Mock.Setup(s => s.GetModels())
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

            var fakeFridgeService = new FridgeFakeService();
            var fakeFridgeProductService = new FridgeProductFakeService();
            
            var controller = new FridgeController
            (
                fakeFridgeService.Service,
                fakeFridgeProductService.Service
            );

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

            fakeFridgeService.Mock.Setup(s => s.GetProducers())
                .Returns(Task.FromResult(actualProducers));

            var response = await controller.GetModels();

            var okResult = response as OkObjectResult;

            // Assert

            Assert.Equal(200, okResult.StatusCode);
        }



        [Fact]
        public async Task GetProductsInFridgeByFridgeId_ValidId_ShouldReturnOk()
        {
            // Arrange

            var fakeFridgeService = new FridgeFakeService();
            var fakeFridgeProductService = new FridgeProductFakeService();
            
            var controller = new FridgeController
            (
                fakeFridgeService.Service,
                fakeFridgeProductService.Service
            );

            IEnumerable<ProductWithCurrentCountAndNameModel> products = new List<ProductWithCurrentCountAndNameModel>()
            {
                new ProductWithCurrentCountAndNameModel()
                {
                    Id = Guid.NewGuid(),
                    Name = "Apple",
                    Count = 3,
                },
                new ProductWithCurrentCountAndNameModel()
                {
                    Id= Guid.NewGuid(),
                    Name = "Cake",
                    Count = 5,
                },
            };

            var fridgeId = new Guid("203e97d9-37e4-47a1-83eb-1ef70d072c6f");

            // Act

            fakeFridgeProductService.Mock.Setup(s => s.GetProductsByFridgeIdAsync(fridgeId))
                .Returns(Task.FromResult(products));

            var response = await controller.GetProductsInFridgeByFridgeId(fridgeId);

            var okRequestResult = response as OkObjectResult;

            // Assert

            Assert.Equal(200, okRequestResult.StatusCode);
        }

        [Fact]
        public async Task FillTheFridgeWithProduct_ValidProductId_ShouldReturnOk()
        {
            // Arrange

            var fakeFridgeService = new FridgeFakeService();
            var fakeFridgeProductService = new FridgeProductFakeService();
            
            var controller = new FridgeController
            (
                fakeFridgeService.Service,
                fakeFridgeProductService.Service
            );

            var productId = new Guid("203e97d9-37e4-47a1-83eb-1ef70d072c6f");

            // Act

            var response = await controller.GetProductsInFridgeByFridgeId(productId);

            var okRequestResult = response as OkObjectResult;

            // Assert

            Assert.Equal(200, okRequestResult.StatusCode);
        }

        [Fact]
        public async Task AddProduct_ValidData_ShouldReturnCreated()
        {
            // Arrange

            var fakeFridgeService = new FridgeFakeService();
            var fakeFridgeProductService = new FridgeProductFakeService();
            
            var controller = new FridgeController
            (
                fakeFridgeService.Service,
                fakeFridgeProductService.Service
            );

            var addProductModel = new AddProductModel()
            {
                FridgeId = new Guid("385e96d7-37e4-47a1-83eb-1ef70d072c8f"),
                ProductId = new Guid("203e97d9-37e4-47a1-83eb-1ef70d072c6f"),
                Count = 1,
            };

            var productWithCurrentCountAndNameModel = new ProductWithCurrentCountAndNameModel()
            {
                Id = Guid.NewGuid(),
                Name = "Apple",
                Count = 1,
            };

            // Act

            fakeFridgeProductService.Mock.Setup(s => s.AddProductAsync(addProductModel))
                .Returns(Task.FromResult(productWithCurrentCountAndNameModel));

            var response = await controller.AddProduct(addProductModel);

            var okRequestResult = response as CreatedResult;

            // Assert

            Assert.Equal(201, okRequestResult.StatusCode);
        }

        [Fact]
        public async Task DeleteProductAsync_ValidData_ShouldReturnOk()
        {
            // Arrange

            var fakeFridgeService = new FridgeFakeService();
            var fakeFridgeProductService = new FridgeProductFakeService();
            
            var controller = new FridgeController
            (
                fakeFridgeService.Service,
                

                fakeFridgeProductService.Service
            );

            var fridgeId = "385e96d7-37e4-47a1-83eb-1ef70d072c8f";
            var productId = "203e97d9-37e4-47a1-83eb-1ef70d072c6f";

            // Act

            var response = await controller.DeleteProductFromFridge(fridgeId, productId);

            var okResult = response as OkResult;

            // Assert

            Assert.Equal(200, okResult.StatusCode);
        }


        [Fact]
        public async Task GetOwnersFridgesAsync_HaveFridges_ShouldReturnOk()
        {
            // Arrange

            var fakeFridgeService = new FridgeFakeService();
            var fakeFridgeProductService = new FridgeProductFakeService();
            
            var controller = new FridgeController
            (
                fakeFridgeService.Service,
                fakeFridgeProductService.Service
            );

            IEnumerable<FridgeModel> actualFridges = new List<FridgeModel>()
            {
                new FridgeModel()
                {
                    Id = new Guid("203e97d9-37e4-47a1-83eb-1ef70d072c6f"),
                    Model = "Toshiba GR-RF610WE-PMS",
                    Owner = "veronika",
                    Producer = "Toshiba",
                    Capacity =  20,
                    CurrentCount =  0,
                },
                new FridgeModel()
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

            fakeFridgeService.Mock.Setup(s => s.GetOwnersFridges())
                .Returns(Task.FromResult(actualFridges));

            var response = await controller.GetOwnersFridges();

            var okResult = response as OkObjectResult;

            var fridges = okResult?.Value as List<FridgeModel>;

            // Assert

            Assert.Equal(200, okResult?.StatusCode);
            Assert.NotEmpty(fridges);
        }

        [Fact]
        public async Task GetRentedFridgeInfoAsync_ValidId_ShouldReturnOk()
        {
            // Arrange

            var fakeFridgeService = new FridgeFakeService();
            var fakeFridgeProductService = new FridgeProductFakeService();
            
            var controller = new FridgeController
            (
                fakeFridgeService.Service,
                fakeFridgeProductService.Service
            );

            var fridgeId = new Guid("203e97d9-37e4-47a1-83eb-1ef70d072c6f");

            // Act

            fakeFridgeService.Mock.Setup(s => s.GetRentedFridgeInfo(fridgeId))
                .Returns(Task.FromResult(new RentDocumentModel()
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

            var rentDocument = okResult?.Value as RentDocumentModel;

            // Assert

            Assert.Equal(200, okResult?.StatusCode);
            Assert.NotNull(rentDocument);
        }

        [Fact]
        public async Task AddFridge_ValidData_ShouldReturnOk()
        {
            // Arrange

            var fakeFridgeService = new FridgeFakeService();
            var fakeFridgeProductService = new FridgeProductFakeService();
            
            var controller = new FridgeController
            (
                fakeFridgeService.Service,
                fakeFridgeProductService.Service
            );

            var addFridgeOwnerModel = new AddFridgeOwnerModel()
            {
                ModelId = Guid.NewGuid(),
                ProducerId = Guid.NewGuid(),
                Capacity = 20,
            };

            var addFridgeModel = new AddFridgeModel()
            {
                FridgeId = Guid.NewGuid(),
                ModelId = addFridgeOwnerModel.ModelId,
                ProducerId = addFridgeOwnerModel.ProducerId,
                OwnerId = Guid.NewGuid(),
                Capacity = 20,
            };

            // Act

            fakeFridgeService.Mock.Setup(s => s.AddFridge(addFridgeOwnerModel))
                .Returns(Task.FromResult(addFridgeModel));

            var response = await controller.AddFridge(addFridgeOwnerModel);

            var createdResult = response as CreatedResult;

            var newFridge = createdResult?.Value as AddFridgeModel;

            // Assert

            Assert.Equal(201, createdResult?.StatusCode);
            Assert.NotNull(newFridge);
        }

        [Fact]
        public async Task DeleteFridge_ValidId_ShouldReturnOk()
        {
            // Arrange

            var fakeFridgeService = new FridgeFakeService();
            var fakeFridgeProductService = new FridgeProductFakeService();
            
            var controller = new FridgeController
            (
                fakeFridgeService.Service,
                fakeFridgeProductService.Service
            );

            var fridgeId = Guid.NewGuid();

            // Act

            var response = await controller.DeleteFridge(fridgeId);

            var okResult = response as OkResult;

            // Assert

            Assert.Equal(200, okResult?.StatusCode);
        }


        [Fact]
        public async Task GetRentersFridgesAsync_HaveFridges_ShouldReturnOk()
        {
            // Arrange

            var fakeFridgeService = new FridgeFakeService();
            var fakeFridgeProductService = new FridgeProductFakeService();
            
            var controller = new FridgeController
            (
                fakeFridgeService.Service,
                fakeFridgeProductService.Service
            );

            IEnumerable<FridgeModel> actualFridges = new List<FridgeModel>()
            {
                new FridgeModel()
                {
                    Id = new Guid("203e97d9-37e4-47a1-83eb-1ef70d072c6f"),
                    Model = "Toshiba GR-RF610WE-PMS",
                    Owner = "veronika",
                    Producer = "Toshiba",
                    Capacity =  20,
                    CurrentCount =  0,
                },
                new FridgeModel()
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

            fakeFridgeService.Mock.Setup(s => s.GetRentersFridges())
                .Returns(Task.FromResult(actualFridges));

            var response = await controller.GetRentersFridges();

            var okResult = response as OkObjectResult;

            var fridges = okResult?.Value as List<FridgeModel>;

            // Assert

            Assert.Equal(200, okResult?.StatusCode);
            Assert.NotEmpty(fridges);
        }

        [Fact]
        public async Task RentFridge_ValidData_ShouldReturnOk()
        {
            // Arrange

            var fakeFridgeService = new FridgeFakeService();
            var fakeFridgeProductService = new FridgeProductFakeService();
            
            var controller = new FridgeController
            (
                fakeFridgeService.Service,
                fakeFridgeProductService.Service
            );

            var fridgeId = Guid.NewGuid();

            var fridgeServicePartDto = new AddFridgeModel()
            {
                FridgeId = fridgeId,
                ModelId = Guid.NewGuid(),
                ProducerId = Guid.NewGuid(),
                OwnerId = Guid.NewGuid(),
                Capacity = 20,
            };

            // Act

            fakeFridgeService.Mock.Setup(s => s.RentFridge(fridgeId))
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

            var fakeFridgeService = new FridgeFakeService();
            var fakeFridgeProductService = new FridgeProductFakeService();
            
            var controller = new FridgeController
            (
                fakeFridgeService.Service,
                fakeFridgeProductService.Service
            );

            var fridgeId = Guid.NewGuid();

            // Act

            var response = await controller.Remove(fridgeId);

            var okResult = response as OkResult;

            // Assert

            Assert.Equal(200, okResult?.StatusCode);
        }
    }
}