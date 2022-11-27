using Fridge.Controllers;
using Fridge.Models.DTOs.FridgeDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestProject1.Mocks;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;


namespace Fridge.Tests.Tests
{
    [TestClass]
    public class FridgeControllerTests
    {
        [TestMethod]
        public async Task GetFridgesAsync_ThereAreFridges_ReturnOk()
        {
            // Arrange

            var fakeService = new FridgeFakeService();

            var controller = new FridgeController(fakeService.Service);

            // Act

            fakeService.Mock.Setup(s => s.GetFridges())
                .Returns(Task.FromResult(new List<FridgeDto>()
                {
                    new FridgeDto()
                    {
                        Id = new Guid("203e97d9-37e4-47a1-83eb-1ef70d072c6f"),
                        Model = "Toshiba GR-RF610WE-PMS",
                        Owner = "veronika",
                        Producer = "Toshiba",
                        Capacity =  20,
                        CurrentCount =  0,
                        isRented = false
                    },
                    new FridgeDto()
                    {
                        Id = new Guid("332ddb8c-57d6-4bbb-a3ea-4d33f5f30fc7"),
                        Model = "Toshiba GR-RF610WE-PMS",
                        Owner = "veronika",
                        Producer = "Toshiba",
                        Capacity = 20,
                        CurrentCount = 0,
                        isRented = false
                    }
                }));

            var response = await controller.GetFridges();

            var okResult = response as OkObjectResult;

            // Arrange

            Assert.AreEqual(200, okResult.StatusCode);
        }
    }
}
