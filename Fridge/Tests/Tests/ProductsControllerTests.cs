using Fridge.Controllers;
using Fridge.Models.Responses;
using Fridge.Tests.Mocks;
using Microsoft.AspNetCore.Mvc;


namespace Fridge.Tests.Tests
{
    public class ProductsControllerTests
    {
        private readonly ProductsFakeService fakeProductsService = new();

        [Fact]
        public async Task GetProductsAsync_Products_ShouldReturnOk()
        {
            // Arrange

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
    }
}
