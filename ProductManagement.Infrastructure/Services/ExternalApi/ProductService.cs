using AutoMapper;
using Common.Shared.Http;
using RandomApp.ProductManagement.Application.DataTransferObjects;
using RandomApp.ProductManagement.Application.Services.Interfaces;
using System.Text.Json;

namespace RandomApp.ProductManagement.Infrastructure.Services.ExternalApi
{
    public class ProductService : ApiClientBase, IProductService
    {
        private readonly IMapper _mapper;

        public ProductService(IHttpClientCreator httpClientCreator, IMapper mapper) : base(httpClientCreator)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsFromApiAsync()
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
                return new List<ProductDto>();
            }

            return productDtos
                .Where(dto => !string.IsNullOrEmpty(dto.Name))
                .ToList();
        }
    }
}
