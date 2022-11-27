using Fridge.Services.Abstracts;
using Moq;

namespace Fridge.Tests.Mocks
{
    public class AuthorizationFakeService
    {
        public Mock<IAuthorizationService> Mock;
        public IAuthorizationService Service;

        public AuthorizationFakeService()
        {
            Mock = new Mock<IAuthorizationService>();
            Service = Mock.Object;
        }
    }
}
