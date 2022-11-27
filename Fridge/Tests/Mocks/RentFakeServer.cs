using Fridge.Services.Abstracts;
using Moq;

namespace Fridge.Tests.Mocks
{
    public class RentFakeServer
    {
        public Mock<IRentService> Mock;
        public IRentService Service;

        public RentFakeServer()
        {
            Mock = new Mock<IRentService>();
            Service = Mock.Object;
        }
    }
}
