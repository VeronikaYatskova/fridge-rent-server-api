using Fridge.Models;
using Fridge.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http.Json;
using System.Text;

namespace FridgeTestProject
{
    [TestFixture]
    public class ProductControllerTests
    {
        [Test]
        public async Task GetAllProducts()
        {
            using var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();

            var result = await client.GetFromJsonAsync<IEnumerable<ProductDto>>("/api/Products");

            Assert.That(result, Is.Not.Null);
        }

        [TestCase("e375d4e4-2861-4eca-b18e-c33ca72194bf", "Apples", 3)]
        public async Task UpdateProduct(string productId, string name, int defaultQuantity)
        {
            using var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();

            string json = "{\r\n  \"name\": \"" + name + "\",\r\n  \"defaultQuantity\": \"" + defaultQuantity + "\r\n}";

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await client.PutAsync($"/api/Products/{productId}", content);

            string actual = result.StatusCode.ToString();
            string expected = "NoContent";

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
