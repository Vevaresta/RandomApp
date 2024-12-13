using AutoMapper;
using NLog;
using Random.App.ProductManagement.Domain.Entities;
using Random.App.ProductManagement.Infrastructure.Data_Transfer_Objects;
using System.Text.Json;

namespace RandomApp.Web.Client.Products
{
    public class ProductService : ApiClientBase, IProductService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public ProductService(IHttpClientCreator httpClientCreator, IMapper mapper) : base(httpClientCreator) 
        {
            _mapper = mapper;
            this._logger = LogManager.GetCurrentClassLogger();
        }

        public async Task<IEnumerable<Product>> GetProductsFromApiAsync()
        {

            _logger.Info("Fetching products from external API");

            var response = await HttpClient.GetAsync("products");

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error fetching products: {response.ReasonPhrase}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var productDtos = JsonSerializer.Deserialize<IEnumerable<ProductDto>>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (productDtos == null)
            {
                _logger.Warn("No products returned from API");
                return new List<Product>();
            }

            _logger.Info("Successfully retrieved {Count} products from API", productDtos.Count());

            var products = productDtos
                .Where(dto => !string.IsNullOrEmpty(dto.Title))
                .Select(dto => _mapper.Map<Product>(dto))
                .ToList();

            return products;
        }

    }
}
