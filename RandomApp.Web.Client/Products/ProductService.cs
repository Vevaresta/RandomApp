using Random.App.ProductManagement.Domain.Entities;
using System.Text.Json;

namespace RandomApp.Web.Client.Products
{
    public class ProductService : ApiClientBase, IProductService
    {
        public ProductService(IHttpClientCreator httpClientCreator) : base(httpClientCreator) { }

        public async Task<IEnumerable<Product>> GetProductsFromApiAsync()
        {
            var response = await HttpClient.GetAsync("products");

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error fetching products: {response.ReasonPhrase}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var products = JsonSerializer.Deserialize<IEnumerable<Product>>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return products ?? new List<Product>();
        }
    }
}
