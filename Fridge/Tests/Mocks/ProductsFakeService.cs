using Fridge.Services.Abstracts;
using Moq;

namespace Fridge.Tests.Mocks
{
    public class ProductsFakeService
    {
        public Mock<IProductsService> Mock;
        public IProductsService Service;

        public ProductsFakeService()
        {
            Mock = new Mock<IProductsService>();
            Service = Mock.Object;
        }
    }
}
