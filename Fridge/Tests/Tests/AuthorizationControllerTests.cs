﻿using Fridge.Controllers;
using Fridge.Models;
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
            var fakeService = new AuthorizationFakeService();

            var controller = new AuthController(fakeService.Service);

            var addUserModel = new AddUserModel()
            {
                Email = "sasha@renter.com",
                Password = "1",
            };

            var token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ2ZXJvbmlrYUByZW50ZXIuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiUmVudGVyIiwiZXhwIjoxNjY4MTAxNDcxfQ.2CEqWrcJYYWBUixjxBEKx42L8jkceKU6230sFYWkutUjnoM_0X_8uSLniSXb-fxZYYutEn_x0x_XUdZixTVuuQ";

            fakeService.Mock.Setup(s => s.RegisterUser(addUserModel, UserRoles.Renter))
                .Returns(Task.FromResult(token));

            var response = await controller.RegisterUser(addUserModel);

            var createdResult = response as CreatedResult;

            Assert.NotNull(createdResult);
            Assert.IsType<CreatedResult>(createdResult);
            Assert.Equal(201, createdResult.StatusCode);

            var actualToken = createdResult.Value;

            Assert.NotNull(actualToken);
            Assert.Equal(token, actualToken);
        }

        [Fact]
        public async Task RegisterRenterAsync_InvalidData_ShouldReturnUnprocessableEntityResult()
        {
            var fakeService = new AuthorizationFakeService();

            var controller = new AuthController(fakeService.Service);

            var addUserModel = new AddUserModel()
            {
                Email = "sasharenter.com",
                Password = "",
                IsOwner = false,
            };

            var response = await controller.RegisterUser(addUserModel);

            Assert.NotNull(response);

            var unprocessableEntityObjectResult = response as UnprocessableEntityObjectResult;

            Assert.NotNull(unprocessableEntityObjectResult);
            Assert.Equal(422, unprocessableEntityObjectResult?.StatusCode);
        }

        [Fact]
        public async Task RegisterRenterAsync_RenterExists_ShouldReturnBadRequestResult()
        {
            var fakeService = new AuthorizationFakeService();

            var controller = new AuthController(fakeService.Service);

            var addUserModel = new AddUserModel()
            {
                Email = "veronika@renter.com",
                Password = "1",
            };

            fakeService.Mock.Setup(s => s.RegisterUser(addUserModel, UserRoles.Renter))
                .Throws(new ArgumentException("Renter with the same email has been registered."));

            var response = await controller.RegisterUser(addUserModel);

            Assert.NotNull(response);

            var badRequestResult = response as BadRequestObjectResult;

            Assert.NotNull(badRequestResult);
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
            Assert.Equal(400, badRequestResult?.StatusCode);
        }

        [Fact]
        public async Task RegisterOwnerAsync_ValidData_ShouldReturnCreated()
        {
            var fakeService = new AuthorizationFakeService();

            var controller = new AuthController(fakeService.Service);

            var addUserModel = new AddUserModel()
            {
                Email = "sasha@owner.com",
                Password = "1",
            };

            var token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ2ZXJvbmlrYUByZW50ZXIuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiUmVudGVyIiwiZXhwIjoxNjY4MTAxNDcxfQ.2CEqWrcJYYWBUixjxBEKx42L8jkceKU6230sFYWkutUjnoM_0X_8uSLniSXb-fxZYYutEn_x0x_XUdZixTVuuQ";

            fakeService.Mock.Setup(s => s.RegisterUser(addUserModel, UserRoles.Owner))
                .Returns(Task.FromResult(token));

            var response = await controller.RegisterUser(addUserModel);

            Assert.NotNull(response);

            var createdResult = response as CreatedResult;

            Assert.NotNull(createdResult);
            Assert.IsType<CreatedResult>(createdResult);
            Assert.Equal(201, createdResult?.StatusCode);
        }

        [Fact]
        public async Task RegisterOwnerAsync_OwnerExists_ShouldReturnBadRequestResult()
        {
            var fakeService = new AuthorizationFakeService();
            var controller = new AuthController(fakeService.Service);

            var addOwnerModel = new AddUserModel()
            {
                Email = "sasha@owner.com",
                Password = "1",
                IsOwner= true,
            };

            fakeService.Mock.Setup(s => s.RegisterUser(addOwnerModel, UserRoles.Owner))
                .Throws(new ArgumentException("Owner with the same email has been registered."));

            var response = await controller.RegisterUser(addOwnerModel);

            Assert.NotNull(response);

            var badRequestResult = response as BadRequestObjectResult;

            Assert.NotNull(badRequestResult);
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task LoginRenter_ValidData_ShouldReturnCreated()
        {
            var fakeService = new AuthorizationFakeService();

            var controller = new AuthController(fakeService.Service);

            var loginModel = new LoginModel()
            {
                Email = "veronika@renter",
                Password = "1",
            };

            var token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ2ZXJvbmlrYUByZW50ZXIuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiUmVudGVyIiwiZXhwIjoxNjY4MTAxNDcxfQ.2CEqWrcJYYWBUixjxBEKx42L8jkceKU6230sFYWkutUjnoM_0X_8uSLniSXb-fxZYYutEn_x0x_XUdZixTVuuQ";

            fakeService.Mock.Setup(s => s.LoginUser(loginModel))
                .Returns(Task.FromResult(token));

            var response = controller.LoginUser(loginModel);

            Assert.NotNull(response);

            var createdResult = await response as CreatedResult;

            Assert.NotNull(createdResult);
            Assert.IsType<CreatedResult>(createdResult);
            Assert.Equal(201, createdResult.StatusCode);

            var actualToken = createdResult.Value;

            Assert.Equal(actualToken, token);
        }

        [Fact]
        public async Task LoginOwner_ValidData_ShouldReturnCreated()
        {
            var fakeService = new AuthorizationFakeService();

            var controller = new AuthController(fakeService.Service);

            var loginModel = new LoginModel()
            {
                Email = "veronika@owner",
                Password = "1",
            };

            var token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ2ZXJvbmlrYUByZW50ZXIuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiUmVudGVyIiwiZXhwIjoxNjY4MTAxNDcxfQ.2CEqWrcJYYWBUixjxBEKx42L8jkceKU6230sFYWkutUjnoM_0X_8uSLniSXb-fxZYYutEn_x0x_XUdZixTVuuQ";

            fakeService.Mock.Setup(s => s.LoginUser(loginModel))
                .Returns(Task.FromResult(token));

            var response = controller.LoginUser(loginModel);

            Assert.NotNull(response);

            var createdResult = await response as CreatedResult;

            Assert.NotNull(createdResult);
            Assert.IsType<CreatedResult>(createdResult);
            Assert.Equal(201, createdResult.StatusCode);

            var actualToken = createdResult.Value;

            Assert.NotNull(actualToken);
            Assert.Equal(token, actualToken);
        }
    }
}
