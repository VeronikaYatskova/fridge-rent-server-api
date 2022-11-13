using Fridge.Models.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Text;

namespace FridgeTestProject
{
    [TestFixture]
    public class FridgeProductControllerTests
    {
        [Test]
        public async Task GetFridgesWithProducts()
        {
            using var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();

            var result = await client.GetFromJsonAsync<IEnumerable<FridgeProductDto>>("/api/FridgeProduct");

            Assert.That(result, Is.Not.Null);
        }

        [TestCase("179e4d4c-e305-4ad9-b48e-6d21a5d31003", "e375d4e4-2861-4eca-b18e-c33ca72194bf")]
        [TestCase("179e4d4c-e305-4ad9-b48e-6d21a5d31003", "5c9de897-b4ad-4769-8681-729631fe2bcb")]
        [TestCase("24e15823-c87e-4509-b71a-0ee250e0a865", "7f547fea-8c97-4bc9-a255-56cbde0fc9cf")]
        [TestCase("4459d7f6-f3a7-41e6-882e-94364ca3cbf6", "787f9ca0-927c-4d4d-bc04-44da0297554f")]
        public async Task GetProductById(Guid fridgeId, Guid productId)
        {
            using var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();

            var result = await client.GetFromJsonAsync<ProductDto>($"/api/FridgeProduct/Product?fridgeId={fridgeId}&productId={productId}");

            Assert.That(result, Is.Not.Null);
        }

        [TestCase("4459d7f6-f3a7-41e6-882e-94364ca3cbf6", "786f9ca0-927c-4d4d-bc04-44da0297554f")]
        public void GetProductByIdReturnNotFound(Guid fridgeId, Guid productId)
        {
            using var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();

            Assert.ThrowsAsync<HttpRequestException>(async () =>
                await client.GetFromJsonAsync<ProductDto>($"/api/FridgeProduct/Product?fridgeId={fridgeId}&productId={productId}"));
        }

        [TestCase("179e4d4c-e305-4ad9-b48e-6d21a5d31003")]
        [TestCase("24e15823-c87e-4509-b71a-0ee250e0a865")]
        [TestCase("4459d7f6-f3a7-41e6-882e-94364ca3cbf6")]
        public async Task GetProductsByFridgeId(string fridgeId)
        {
            using var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();

            var result = await client.GetFromJsonAsync<IEnumerable<ProductDto>>($"/api/FridgeProduct/Fridge/{fridgeId}");

            Assert.That(result, Is.Not.Null);
        }

        [TestCase("4659d7f6-f3a7-41e6-882e-94364ca3cbf6")]
        public void GetProductsByFridgeIdReturnNotFound(string fridgeId)
        {
            using var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();

            Assert.ThrowsAsync<HttpRequestException>(async () =>
                await client.GetFromJsonAsync<IEnumerable<ProductDto>>($"/api/FridgeProduct/Fridge?fridgeId={fridgeId}"));
        }

        [TestCase("DD930B8C-6CE1-4915-8A0A-922CE430718C", "7f547fea-8c97-4bc9-a255-56cbde0fc9cf", 1)]
        [TestCase("6D72B213-A206-4912-8C5B-A3B1996EA6C6", "787f9ca0-927c-4d4d-bc04-44da0297554f", 1)]
        [TestCase("531C3022-4591-45B0-844E-C02BD57CAD2A", "5c9de897-b4ad-4769-8681-729631fe2bcb", 1)]
        public async Task AddProduct(string fridgeId, string productId, int count)
        {
            using var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();

            string json = "{\r\n  \"fridgeId\": \"" + fridgeId + "\",\r\n  \"productId\": \"" + productId + "\",\r\n  \"count\": " + count + "\r\n}";

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await client.PostAsync("/api/FridgeProduct", content);

            string actual = result.StatusCode.ToString();
            string expected = "OK";

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase("531C3022-4591-45B0-844E-C02BD57CAD2A", "5c9de897-b4ad-4769-8681-729631fe2bcb", 53)]
        public async Task AddProductWhenFridgeIsFull(string fridgeId, string productId, int count)
        {
            using var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();

            string json = "{\r\n  \"fridgeId\": \"" + fridgeId + "\",\r\n  \"productId\": \"" + productId + "\",\r\n  \"count\": " + count + "\r\n}";

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await client.PostAsync("/api/FridgeProduct", content);

            var actual = result.StatusCode.ToString();
            var expected = "InternalServerError";

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase("531C3023-4591-45B0-844E-C02BD57CAD2A", "5c9de897-b4ad-4769-8681-729631fe2bcb", 1)]
        public async Task AddProductFridgeIdNotFound(string fridgeId, string productId, int count)
        {
            using var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();

            string json = "{\r\n  \"fridgeId\": \"" + fridgeId + "\",\r\n  \"productId\": \"" + productId + "\",\r\n  \"count\": " + count + "\r\n}";

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await client.PostAsync("/api/FridgeProduct", content);

            var actual = result.StatusCode.ToString();
            var expected = "NotFound";

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase("531C3022-4591-45B0-844E-C02BD57CAD2A", "5c9de894-b4ad-4769-8681-729631fe2bcb", 1)]
        public async Task AddProductProductIdNotFound(string fridgeId, string productId, int count)
        {
            using var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();

            string json = "{\r\n  \"fridgeId\": \"" + fridgeId + "\",\r\n  \"productId\": \"" + productId + "\",\r\n  \"count\": " + count + "\r\n}";

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await client.PostAsync("/api/FridgeProduct", content);

            var actual = result.StatusCode.ToString();
            var expected = "NotFound";

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase("531C3022-4591-45B0-844E-C02BD57CAD2A", "5c9de897-b4ad-4769-8681-729631fe2bcb")]
        [TestCase("DD930B8C-6CE1-4915-8A0A-922CE430718C", "7f547fea-8c97-4bc9-a255-56cbde0fc9cf")]
        [TestCase("6D72B213-A206-4912-8C5B-A3B1996EA6C6", "787f9ca0-927c-4d4d-bc04-44da0297554f")]
        public async Task DeleteProductFromFridge(string fridgeId, string productId)
        {
            using var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();

            var result = await client.DeleteAsync($"/api/FridgeProduct?fridgeId={fridgeId}&productId={productId}");

            string actual = result.StatusCode.ToString();
            string expected = "OK";

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase("6E72B213-A206-4912-8C5B-A3B1996EA6C6", "787f9ca0-927c-4d4d-bc04-44da0297554f")]
        [TestCase("6E72B213-A206-4912-8C5B-A3B1996EA6C6", "797f9ca0-927c-4d4d-bc04-44da0297554f")]
        [TestCase("6E72B213-A206-4912-8C5B-A3O1996EA6C6", "787f9ua0-927c-4d4d-bc04-44da0297554f")]
        public async Task DeleteProductFromFridgeReturnNotFound(string fridgeId, string productId)
        {
            using var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();

            var result = await client.DeleteAsync($"/api/FridgeProduct?fridgeId={fridgeId}&productId={productId}");

            string actual = result.StatusCode.ToString();
            string expected = "NotFound";

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
