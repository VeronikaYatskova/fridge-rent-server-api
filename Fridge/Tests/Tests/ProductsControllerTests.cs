using Fridge.Controllers;
using Fridge.Models.Requests;
using Fridge.Models.Responses;
using Fridge.Tests.Mocks;
using Microsoft.AspNetCore.Mvc;


namespace Fridge.Tests.Tests
{
    public class ProductsControllerTests
    {
        [Fact]
        public async Task GetProductsAsync_Products_ShouldReturnOk()
        {
            // Arrange

            var fakeProductsService = new ProductsFakeService();
            var controller = new ProductsController(fakeProductsService.Service);

            IEnumerable<ProductModel> products = new List<ProductModel>()
            {
                new ProductModel
                {
                    Id = new Guid("36B74198-A896-429F-B040-0512FCA189A8"),
                    Name = "Apple",
                    DefaultQuantity = 3,
                },
                new ProductModel
                {
                    Id = new Guid("F2DDEA9C-7691-4C7A-99EC-ABAEC36DB9BD"),
                    Name = "Milk",
                    DefaultQuantity = 1,
                },
                new ProductModel
                {
                    Id = new Guid("FDB08EB6-D113-4D8A-8576-3454BB89AD55"),
                    Name = "Eggs",
                    DefaultQuantity = 10,
                },
                new ProductModel
                {
                    Id = new Guid("B89AA809-9FAC-4C67-AF3B-6599ADE45F92"),
                    Name = "Cake",
                    DefaultQuantity = 1,
                },
                new ProductModel
                {
                    Id = new Guid("3AB533B7-2AE5-4121-85DD-9D977E1B53ED"),
                    Name = "Tomato",
                    DefaultQuantity = 5,
                }
            };

            // Act

            fakeProductsService.Mock.Setup(s => s.GetProducts())
                .Returns(Task.FromResult(products));

            var response = await controller.GetProducts();

            var okResult = response as OkObjectResult;

            // Assert

            Assert.Equal(200, okResult.StatusCode);
        }

        //[Fact]
        //public async Task UpdateProductAsync_ValidData_ShouldReturnNoContent()
        //{
        //    // Arrange

        //    var fakeProductsService = new ProductsFakeService();
        //    var fakeFridgeProductService = new FridgeProductFakeService();

        //    var controller = new ProductsController(fakeProductsService.Service);

        //    var updateProductModel = new UpdateProductModel()
        //    {
        //        FridgeId = new Guid("385e96d7-37e4-47a1-83eb-1ef70d072c8f"),
        //        ProductId = new Guid("203e97d9-37e4-47a1-83eb-1ef70d072c6f"),
        //        Count = 1,
        //    };

        //    // Act

        //    fakeFridgeProductService.Mock.Setup(s => s.UpdateProductAsync(updateProductModel))
        //        .Returns(Task.FromResult(
        //        new 
        //        {
        //            Id = Guid.NewGuid(),
        //            Name = "Apple",
        //            Count = 1,
        //        }));

        //    var response = await controller.UpdateProductAsync(updateProductModel);

        //    var noContentRequestResult = response as NoContentResult;

        //    // Assert

        //    Assert.Equal(204, noContentRequestResult.StatusCode);
        //}
    }
}
