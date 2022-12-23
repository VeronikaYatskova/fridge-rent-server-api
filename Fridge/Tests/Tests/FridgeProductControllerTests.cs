using Fridge.Controllers;
using Fridge.Models.Requests;
using Fridge.Models.Responses;
using Fridge.Tests.Mocks;
using Microsoft.AspNetCore.Mvc;


namespace Fridge.Tests.Tests
{
    public class FridgeProductControllerTests
    {
        [Fact]
        public async Task GetProductsInFridgeByFridgeId_ValidId_ShouldReturnOk()
        {
            var fakeFridgeProductService = new FridgeProductFakeService();
            var controller = new FridgeProductController(fakeFridgeProductService.Service);

            IEnumerable<ProductWithCurrentCountAndNameModel> expectedProducts = new List<ProductWithCurrentCountAndNameModel>()
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

            fakeFridgeProductService.Mock.Setup(s => s.GetProductsByFridgeIdAsync(fridgeId))
                .Returns(Task.FromResult(expectedProducts));

            var response = await controller.GetProductsInFridgeByFridgeId(fridgeId);

            Assert.NotNull(response);
            Assert.IsType<OkObjectResult>(response);

            var okRequestResult = response as OkObjectResult;

            Assert.Equal(200, okRequestResult?.StatusCode);
            Assert.NotNull(okRequestResult);

            var products = okRequestResult.Value;

            Assert.Equal(expectedProducts, products);
        }

        [Fact]
        public async Task FillTheFridgeWithProduct_ValidProductId_ShouldReturnOk()
        {
            // Arrange

            var fakeFridgeProductService = new FridgeProductFakeService();

            var controller = new FridgeProductController(fakeFridgeProductService.Service);

            var productId = new Guid("203e97d9-37e4-47a1-83eb-1ef70d072c6f");

            // Act

            var response = await controller.GetProductsInFridgeByFridgeId(productId);

            Assert.NotNull(response);
            Assert.IsType<OkObjectResult>(response);

            var okRequestResult = response as OkObjectResult;

            // Assert

            Assert.NotNull(okRequestResult.Value);
            Assert.Equal(200, okRequestResult?.StatusCode);
        }

        [Fact]
        public async Task AddProduct_ValidData_ShouldReturnOkResult()
        {
            // Arrange

            var fakeFridgeProductService = new FridgeProductFakeService();

            var controller = new FridgeProductController(fakeFridgeProductService.Service);

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

            var response = await controller.AddProduct(addProductModel.FridgeId, addProductModel.ProductId, addProductModel.Count);

            Assert.NotNull(response);
            Assert.IsType<OkResult>(response);

            var okRequestResult = response as OkResult;

            Assert.NotNull(okRequestResult);
            Assert.Equal(200, okRequestResult?.StatusCode);
        }
        
        [Fact]
        public async Task DeleteProductAsync_ValidData_ShouldReturnOk()
        {
            // Arrange

            var fakeFridgeProductService = new FridgeProductFakeService();

            var controller = new FridgeProductController(fakeFridgeProductService.Service);

            var fridgeId = new Guid("385e96d7-37e4-47a1-83eb-1ef70d072c8f");
            var productId = new Guid("203e97d9-37e4-47a1-83eb-1ef70d072c6f");

            // Act

            var response = await controller.DeleteProductFromFridge(fridgeId, productId);

            Assert.NotNull(response);
            Assert.IsType<OkResult>(response);

            var okResult = response as OkResult;

            Assert.NotNull(okResult);

            // Assert

            Assert.Equal(200, okResult?.StatusCode);
        }

        [Fact]
        public async Task UpdateProductAsync_ValidData_ShouldReturnNoContent()
        {
            // Arrange

            var fakeFridgeProductService = new FridgeProductFakeService();

            var controller = new FridgeProductController(fakeFridgeProductService.Service);

            var updateProductModel = new UpdateProductModel()
            {
                FridgeId = new Guid("385e96d7-37e4-47a1-83eb-1ef70d072c8f"),
                ProductId = new Guid("203e97d9-37e4-47a1-83eb-1ef70d072c6f"),
                Count = 1,
            };

            // Act

            fakeFridgeProductService.Mock.Setup(s => s.UpdateProductAsync(updateProductModel))
                .Returns(Task.FromResult(
                new
                {
                    Id = Guid.NewGuid(),
                    Name = "Apple",
                    Count = 1,
                }));

            var response = await controller.UpdateProductAsync(updateProductModel.FridgeId, updateProductModel.ProductId, updateProductModel.Count);

            Assert.NotNull(response);
            Assert.IsType<NoContentResult>(response);

            var noContentRequestResult = response as NoContentResult;

            // Assert

            Assert.NotNull(noContentRequestResult);
            Assert.Equal(204, noContentRequestResult.StatusCode);
        }
    }
}
