using Fridge.Services.Abstracts;
using Moq;

namespace Fridge.Tests.Mocks
{
    public class FridgeProductFakeService
    {
        public Mock<IFridgeProductService> Mock;
        public IFridgeProductService Service;

        public FridgeProductFakeService()
        {
            Mock = new Mock<IFridgeProductService>();
            Service = Mock.Object;
        }
    }
}
