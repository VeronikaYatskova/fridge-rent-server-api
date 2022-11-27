using Fridge.Controllers;
using Fridge.Models.DTOs.FridgeProductDto;
using Fridge.Models.DTOs.FridgeProductDto.FridgeProductDto;
using Fridge.Models.DTOs.FridgeProductDtos;
using Fridge.Models.DTOs.ProductDtos;
using Fridge.Tests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace Fridge.Tests.Tests
{
    public class FridgeProductsControllerTests
    {
        [Fact]
        public async Task GetProductsInFridgeByFridgeId_ValidId_ShouldReturnOk()
        {
            // Arrange

            var fakeService = new FridgeProductFakeService();

            var controller = new FridgeProductController(fakeService.Service);

            IEnumerable<ProductWithCurrentCountAndNameDto> products = new List<ProductWithCurrentCountAndNameDto>()
            {
                new ProductWithCurrentCountAndNameDto()
                {
                    Id = Guid.NewGuid(),
                    Name = "Apple",
                    Count = 3,
                },
                new ProductWithCurrentCountAndNameDto()
                {
                    Id= Guid.NewGuid(),
                    Name = "Cake",
                    Count = 5,
                },
            };

            var fridgeId = new Guid("203e97d9-37e4-47a1-83eb-1ef70d072c6f");

            // Act

            fakeService.Mock.Setup(s => s.GetProductsByFridgeIdAsync(fridgeId))
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

            var fakeService = new FridgeProductFakeService();

            var controller = new FridgeProductController(fakeService.Service);

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

            var fakeService = new FridgeProductFakeService();

            var controller = new FridgeProductController(fakeService.Service);

            var fridgeProductDto = new FridgeProductDto()
            {
                FridgeId = new Guid("385e96d7-37e4-47a1-83eb-1ef70d072c8f"),
                ProductId = new Guid("203e97d9-37e4-47a1-83eb-1ef70d072c6f"),
                Count = 1,
            };

            var addProductDto = new AddProductDto()
            {
                Id = Guid.NewGuid(),
                Name = "Apple",
                Count = 1,
            };

            // Act

            fakeService.Mock.Setup(s => s.AddProductAsync(fridgeProductDto))
                .Returns(Task.FromResult(addProductDto));

            var response = await controller.AddProduct(fridgeProductDto);

            var okRequestResult = response as CreatedResult;

            // Assert

            Assert.Equal(201, okRequestResult.StatusCode);
        }

        [Fact]
        public async Task UpdateProductAsync_ValidData_ShouldReturnNoContent()
        {
            // Arrange

            var fakeService = new FridgeProductFakeService();

            var controller = new FridgeProductController(fakeService.Service);

            var productUpdateDto = new ProductUpdateDto()
            {
                FridgeId = new Guid("385e96d7-37e4-47a1-83eb-1ef70d072c8f"),
                ProductId = new Guid("203e97d9-37e4-47a1-83eb-1ef70d072c6f"),
                Count = 1,
            };

            var addProductDto = new AddProductDto()
            {
                Id = Guid.NewGuid(),
                Name = "Apple",
                Count = 1,
            };

            // Act

            fakeService.Mock.Setup(s => s.UpdateProductAsync(productUpdateDto))
                .Returns(Task.FromResult(addProductDto));

            var response = await controller.UpdateProductAsync(productUpdateDto);

            var noContentRequestResult = response as NoContentResult;

            // Assert

            Assert.Equal(204, noContentRequestResult.StatusCode);
        }

        [Fact]
        public async Task DeleteProductAsync_ValidData_ShouldReturnOk()
        {
            // Arrange

            var fakeService = new FridgeProductFakeService();

            var controller = new FridgeProductController(fakeService.Service);

            var fridgeId = "385e96d7-37e4-47a1-83eb-1ef70d072c8f";
            var productId = "203e97d9-37e4-47a1-83eb-1ef70d072c6f";

            // Act

            var response = await controller.DeleteProductFromFridge(fridgeId, productId);

            var okResult = response as OkResult;

            // Assert

            Assert.Equal(200, okResult.StatusCode);
        }
    }
}
