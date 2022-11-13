using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace FridgeTestProject
{
    [TestFixture]
    public class FridgeControllerTests
    {
        [Test]
        public async Task GetAllFridgesTest()
        {
            using var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();

            var fridges = await client.GetFromJsonAsync<IEnumerable<Fridge.Models.Fridge>>("/api/Fridge");

            Assert.That(fridges, Is.Not.Null);
        }

        [TestCase("24E15823-C87E-4509-B71A-0EE250E0A865")]
        [TestCase("179E4D4C-E305-4AD9-B48E-6D21A5D31003")]
        [TestCase("4459D7F6-F3A7-41E6-882E-94364CA3CBF6")]
        public async Task GetFridgeInfoById(string fridgeId)
        {
            using var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();

            var fridgeGuid = Guid.Parse(fridgeId);

            var fridgeInfo = await client.GetFromJsonAsync<Fridge.Models.Fridge>($"/api/Fridge/Fridge/{fridgeGuid}");

            Assert.That(fridgeInfo, Is.Not.Null);
        }

        [TestCase("E6F26490-7180-4504-9CE4-A7CC76E49A83", "ACA0F6A1-32C0-487E-8A0D-E836BE5A64A9", "2B15503E-BCC9-43D9-ACD3-5FB94A18B395", 50)]
        public async Task AddFridge(string modelId, string ownerId, string producerId, int capacity)
        {
            using var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();

            string json = "{\r\n  \"modelId\": \"" + modelId + "\",\r\n  \"ownerId\": \"" + ownerId + "\",\r\n  \"producerId\": \"" + producerId + "\",\r\n  \"capacity\": " + capacity + "\r\n}";
            
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await client.PostAsync("/api/Fridge", content);

            string actual = result.StatusCode.ToString();
            string expected = "Created";

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase("3459D7F6-F3A7-41E6-882E-94364CA3CBF6")]
        public void GetFridgeInfoByIdReturnNotFound(string fridgeId)
        {
            using var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();

            var fridgeGuid = Guid.Parse(fridgeId);

            Assert.ThrowsAsync<HttpRequestException>(async () =>
                await client.GetFromJsonAsync<HttpStatusCode>($"/api/Fridge/Fridge/{fridgeGuid}"));
        }
    }
}
