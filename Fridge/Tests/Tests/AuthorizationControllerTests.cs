using Fridge.Controllers;
using Fridge.Models.Requests;
using Fridge.Tests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace Fridge.Tests.Tests
{
    public class AuthorizationControllerTests
    {
        [Fact]
        public async Task RegisterRenterAsync_ValidData_ShouldReturnCreated()
        {
            // Arrange

            var fakeService = new AuthorizationFakeService();

            var controller = new AuthController(fakeService.Service);

            var addRenterModel = new AddRenterModel()
            {
                Email = "sasha@renter.com",
                Password = "1",
            };

            var token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ2ZXJvbmlrYUByZW50ZXIuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiUmVudGVyIiwiZXhwIjoxNjY4MTAxNDcxfQ.2CEqWrcJYYWBUixjxBEKx42L8jkceKU6230sFYWkutUjnoM_0X_8uSLniSXb-fxZYYutEn_x0x_XUdZixTVuuQ";

            // Act

            fakeService.Mock.Setup(s => s.RegisterRenter(addRenterModel))
                .Returns(Task.FromResult(token));

            var response = await controller.RegisterRenter(addRenterModel);

            var createdResult = response as CreatedResult;

            // Assert
            Assert.Equal(201, createdResult.StatusCode);
        }

        [Fact]
        public async Task RegisterRenterAsync_InvalidData_ShouldReturnBadRequestResult()
        {
            // Arrange

            var fakeService = new AuthorizationFakeService();

            var controller = new AuthController(fakeService.Service);

            var addRenterModel = new AddRenterModel()
            {
                Email = "sasharenter.com",
                Password = "",
            };

            // Act

            fakeService.Mock.Setup(s => s.RegisterRenter(addRenterModel))
                .Throws(new ArgumentException("Invalid data"));

            var response = await controller.RegisterRenter(addRenterModel);

            var badRequestResult = response as BadRequestObjectResult;

            // Assert

            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task RegisterRenterAsync_RenterExists_ShouldReturnBadRequestResult()
        {
            // Arrange

            var fakeService = new AuthorizationFakeService();

            var controller = new AuthController(fakeService.Service);

            var addRenterModel = new AddRenterModel()
            {
                Email = "veronika@renter.com",
                Password = "1",
            };

            // Act

            fakeService.Mock.Setup(s => s.RegisterRenter(addRenterModel))
                .Throws(new ArgumentException("Renter with the same email has been registered."));

            var response = await controller.RegisterRenter(addRenterModel);

            var badRequestResult = response as BadRequestObjectResult;

            // Assert

            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task RegisterOwnerAsync_ValidData_ShouldReturnCreated()
        {
            // Arrange

            var fakeService = new AuthorizationFakeService();

            var controller = new AuthController(fakeService.Service);

            var addOwnerModel = new AddOwnerModel()
            {
                Name = "Sasha",
                Email = "sasha@owner.com",
                Password = "1",
                Phone = "+37525478963",
            };

            var token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ2ZXJvbmlrYUByZW50ZXIuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiUmVudGVyIiwiZXhwIjoxNjY4MTAxNDcxfQ.2CEqWrcJYYWBUixjxBEKx42L8jkceKU6230sFYWkutUjnoM_0X_8uSLniSXb-fxZYYutEn_x0x_XUdZixTVuuQ";

            // Act

            fakeService.Mock.Setup(s => s.RegisterOwner(addOwnerModel))
                .Returns(Task.FromResult(token));

            var response = await controller.RegisterOwner(addOwnerModel);

            var createdResult = response as CreatedResult;

            // Assert
            Assert.Equal(201, createdResult.StatusCode);
        }

        [Fact]
        public async Task RegisterOwnerAsync_InvalidData_ShouldReturnBadRequestResult()
        {
            // Arrange

            var fakeService = new AuthorizationFakeService();

            var controller = new AuthController(fakeService.Service);

            var addOwnerModel = new AddOwnerModel()
            {
                Name = "Sasha",
                Email = "sashaowner.com",
                Password = "",
                Phone = "+37544478963",
            };

            // Act

            fakeService.Mock.Setup(s => s.RegisterOwner(addOwnerModel))
                .Throws(new ArgumentException("Invalid data"));

            var response = await controller.RegisterOwner(addOwnerModel);

            var badRequestResult = response as BadRequestObjectResult;

            // Assert
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task RegisterOwnerAsync_OwnerExists_ShouldReturnBadRequestResult()
        {
            // Arrange

            var fakeService = new AuthorizationFakeService();

            var controller = new AuthController(fakeService.Service);

            var addOwnerModel = new AddOwnerModel()
            {
                Name = "Sasha",
                Email = "sasha@owner.com",
                Password = "1",
                Phone = "+37544478963",
            };

            // Act

            fakeService.Mock.Setup(s => s.RegisterOwner(addOwnerModel))
                .Throws(new ArgumentException("Owner with the same email has been registered."));

            var response = await controller.RegisterOwner(addOwnerModel);

            var badRequestResult = response as BadRequestObjectResult;

            // Assert
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public void LoginRenter_ValidData_ShouldReturnCreated()
        {
            // Arrange

            var fakeService = new AuthorizationFakeService();

            var controller = new AuthController(fakeService.Service);

            var loginModel = new LoginModel()
            {
                Email = "veronika@renter",
                Password = "1",
            };

            var token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ2ZXJvbmlrYUByZW50ZXIuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiUmVudGVyIiwiZXhwIjoxNjY4MTAxNDcxfQ.2CEqWrcJYYWBUixjxBEKx42L8jkceKU6230sFYWkutUjnoM_0X_8uSLniSXb-fxZYYutEn_x0x_XUdZixTVuuQ";

            // Act

            fakeService.Mock.Setup(s => s.LoginRenter(loginModel))
                .Returns(token);

            var response = controller.LoginRenter(loginModel);

            var createdResult = response as CreatedResult;

            // Assert

            Assert.Equal(201, createdResult.StatusCode);
        }

        [Fact]
        public void LoginOwner_ValidData_ShouldReturnCreated()
        {
            // Arrange

            var fakeService = new AuthorizationFakeService();

            var controller = new AuthController(fakeService.Service);

            var loginModel = new LoginModel()
            {
                Email = "veronika@owner",
                Password = "1",
            };

            var token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ2ZXJvbmlrYUByZW50ZXIuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiUmVudGVyIiwiZXhwIjoxNjY4MTAxNDcxfQ.2CEqWrcJYYWBUixjxBEKx42L8jkceKU6230sFYWkutUjnoM_0X_8uSLniSXb-fxZYYutEn_x0x_XUdZixTVuuQ";

            // Act

            fakeService.Mock.Setup(s => s.LoginOwner(loginModel))
                .Returns(token);

            var response = controller.LoginRenter(loginModel);

            var createdResult = response as CreatedResult;

            // Assert

            Assert.Equal(201, createdResult.StatusCode);
        }
    }
}
