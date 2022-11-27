using Fridge.Services.Abstracts;
using Moq;

namespace Fridge.Tests.Mocks
{
    public class OwnerFakeService
    {
        public Mock<IOwnerService> Mock;
        public IOwnerService Service;

        public OwnerFakeService()
        {
            Mock = new Mock<IOwnerService>();
            Service = Mock.Object;
        }
    }
}
