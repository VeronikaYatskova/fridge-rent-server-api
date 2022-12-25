using Fridge.Services.Abstracts;
using Moq;

namespace Fridge.Tests.Mocks
{
    public class AuthorizationFakeService
    {
        public Mock<IAuthService> Mock;
        public IAuthService Service;

        public AuthorizationFakeService()
        {
            Mock = new Mock<IAuthService>();
            Service = Mock.Object;
        }
    }
}
