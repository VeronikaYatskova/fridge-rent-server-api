using Fridge.Controllers;
using Fridge.Models.DTOs;
using Fridge.Models.DTOs.OwnerDtos;
using Fridge.Models.DTOs.RenterDtos;
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

            var userDto = new RenterDto()
            {
                Email = "sasha@renter.com",
                Password = "1",
            };

            var token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ2ZXJvbmlrYUByZW50ZXIuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiUmVudGVyIiwiZXhwIjoxNjY4MTAxNDcxfQ.2CEqWrcJYYWBUixjxBEKx42L8jkceKU6230sFYWkutUjnoM_0X_8uSLniSXb-fxZYYutEn_x0x_XUdZixTVuuQ";

            // Act

            fakeService.Mock.Setup(s => s.RegisterRenter(userDto))
                .Returns(Task.FromResult(token));

            var response = await controller.RegisterRenter(userDto);

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

            var userDto = new RenterDto()
            {
                Email = "sasharenter.com",
                Password = "",
            };

            // Act

            fakeService.Mock.Setup(s => s.RegisterRenter(userDto))
                .Throws(new ArgumentException("Invalid data"));

            var response = await controller.RegisterRenter(userDto);

            var badRequestResult = response as BadRequestResult;

            // Assert

            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task RegisterRenterAsync_RenterExists_ShouldReturnBadRequestResult()
        {
            // Arrange

            var fakeService = new AuthorizationFakeService();

            var controller = new AuthController(fakeService.Service);

            var userDto = new RenterDto()
            {
                Email = "veronika@renter.com",
                Password = "1",
            };

            // Act

            fakeService.Mock.Setup(s => s.RegisterRenter(userDto))
                .Throws(new ArgumentException("Renter with the same email has been registered."));

            var response = await controller.RegisterRenter(userDto);

            var badRequestResult = response as BadRequestResult;

            // Assert

            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task RegisterOwnerAsync_ValidData_ShouldReturnCreated()
        {
            // Arrange

            var fakeService = new AuthorizationFakeService();

            var controller = new AuthController(fakeService.Service);

            var ownerDto = new OwnerDto()
            {
                Name = "Sasha",
                Email = "sasha@owner.com",
                Password = "1",
                Phone = "+37525478963",
            };

            var token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ2ZXJvbmlrYUByZW50ZXIuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiUmVudGVyIiwiZXhwIjoxNjY4MTAxNDcxfQ.2CEqWrcJYYWBUixjxBEKx42L8jkceKU6230sFYWkutUjnoM_0X_8uSLniSXb-fxZYYutEn_x0x_XUdZixTVuuQ";

            // Act

            fakeService.Mock.Setup(s => s.RegisterOwner(ownerDto))
                .Returns(Task.FromResult(token));

            var response = await controller.RegisterOwner(ownerDto);

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

            var ownerDto = new OwnerDto()
            {
                Name = "Sasha",
                Email = "sashaowner.com",
                Password = "",
                Phone = "+37544478963",
            };

            // Act

            fakeService.Mock.Setup(s => s.RegisterOwner(ownerDto))
                .Throws(new ArgumentException("Invalid data"));

            var response = await controller.RegisterOwner(ownerDto);

            var badRequestResult = response as BadRequestResult;

            // Assert
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task RegisterOwnerAsync_OwnerExists_ShouldReturnBadRequestResult()
        {
            // Arrange

            var fakeService = new AuthorizationFakeService();

            var controller = new AuthController(fakeService.Service);

            var ownerDto = new OwnerDto()
            {
                Name = "Sasha",
                Email = "sasha@owner.com",
                Password = "1",
                Phone = "+37544478963",
            };

            // Act

            fakeService.Mock.Setup(s => s.RegisterOwner(ownerDto))
                .Throws(new ArgumentException("Owner with the same email has been registered."));

            var response = await controller.RegisterOwner(ownerDto);

            var badRequestResult = response as BadRequestResult;

            // Assert
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public void LoginRenter_ValidData_ShouldReturnCreated()
        {
            // Arrange

            var fakeService = new AuthorizationFakeService();

            var controller = new AuthController(fakeService.Service);

            var loginDto = new LoginDto()
            {
                Email = "veronika@renter",
                Password = "1",
            };

            var token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ2ZXJvbmlrYUByZW50ZXIuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiUmVudGVyIiwiZXhwIjoxNjY4MTAxNDcxfQ.2CEqWrcJYYWBUixjxBEKx42L8jkceKU6230sFYWkutUjnoM_0X_8uSLniSXb-fxZYYutEn_x0x_XUdZixTVuuQ";

            // Act

            fakeService.Mock.Setup(s => s.LoginRenter(loginDto))
                .Returns(token);

            var response = controller.LoginRenter(loginDto);

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

            var loginDto = new LoginDto()
            {
                Email = "veronika@owner",
                Password = "1",
            };

            var token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ2ZXJvbmlrYUByZW50ZXIuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiUmVudGVyIiwiZXhwIjoxNjY4MTAxNDcxfQ.2CEqWrcJYYWBUixjxBEKx42L8jkceKU6230sFYWkutUjnoM_0X_8uSLniSXb-fxZYYutEn_x0x_XUdZixTVuuQ";

            // Act

            fakeService.Mock.Setup(s => s.LoginOwner(loginDto))
                .Returns(token);

            var response = controller.LoginRenter(loginDto);

            var createdResult = response as CreatedResult;

            // Assert

            Assert.Equal(201, createdResult.StatusCode);
        }
    }
}
