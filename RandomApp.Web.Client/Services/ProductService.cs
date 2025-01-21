using AutoMapper;
using RandomApp.ProductManagement.Application.DataTransferObjects;
using RandomApp.ProductManagement.Application.Services;
using RandomApp.ProductManagement.Domain.Entities;
using System.Text.Json;

namespace RandomApp.Web.Client.Services
{
    public class ProductService : ApiClientBase, IProductService
    {
        private readonly IMapper _mapper;

        public ProductService(IHttpClientCreator httpClientCreator, IMapper mapper) : base(httpClientCreator)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<Product>> GetProductsFromApiAsync()
        {
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
                return new List<Product>();
            }

            var products = productDtos
                .Where(dto => !string.IsNullOrEmpty(dto.Title))
                .Select(_mapper.Map<Product>)
                .ToList();

            return products;
        }
    }
}
