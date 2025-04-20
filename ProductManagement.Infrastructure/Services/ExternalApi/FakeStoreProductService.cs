using AutoMapper;
using Common.Shared.Http;
using RandomApp.ProductManagement.Application.DataTransferObjects;
using RandomApp.ProductManagement.Application.Services.Interfaces;
using RandomApp.ProductManagement.Domain.Enums;
using System.Text.Json;

namespace RandomApp.ProductManagement.Infrastructure.Services.ExternalApi
{
    public class FakeStoreProductService : ApiClientBase, IProductService
    {
        private readonly IMapper _mapper;
        private const string DefaultCurrency = "EUR";
        private const string SkuPrefix = "FS-";

        public FakeStoreProductService(IHttpClientCreator httpClientCreator, IMapper mapper) : base(httpClientCreator)
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

            var fakeStoreProducts = JsonSerializer.Deserialize<IEnumerable<FakeStoreProductDto>>(jsonResponse,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (fakeStoreProducts == null)
            {
                return new List<ProductDto>();
            }


            var productDtos = fakeStoreProducts
                .Where(fakeProduct => !string.IsNullOrEmpty(fakeProduct.Title))
                .Select(fakeProduct => new ProductDto
            {
                Id = 0,
                OriginalApiId = fakeProduct.Id,
                Name = fakeProduct.Title,
                Amount = fakeProduct.Price,
                Currency = DefaultCurrency,
                SKU = GenerateSku(fakeProduct.Id > 0 ? fakeProduct.Id : 1),
                Category = NormalizeCategory(fakeProduct.Category),
                ProductDescription = fakeProduct.Description,
                Image = fakeProduct.Image
            })
            .ToList();

            return productDtos;
        }

        private string GenerateSku(int externalId)
        {
            return $"{SkuPrefix}{externalId:D5}";
        }

        private string NormalizeCategory(string apiCategory)
        {
            if (string.IsNullOrEmpty(apiCategory))
                return Category.Other.ToString();

            var normalized = apiCategory.Trim()
                .Replace(" ", "")
                .Replace("-", "")
                .Replace("_", "");

            if (normalized.Length > 0)
            {
                normalized = char.ToUpper(normalized[0]) +
                    (normalized.Length > 1 ? normalized.Substring(1).ToLower() : "");
            }

            if (Enum.TryParse<Category>(normalized, true, out var result))
            {
                return result.ToString();
            }

            return apiCategory.ToLower() switch
            {
                "men's clothing" => Category.MenClothing.ToString(),
                "women's clothing" => Category.WomenClothing.ToString(),
                "jewelery" => Category.Jewelry.ToString(),
                "electronics" => Category.Electronics.ToString(),

                _ => Category.Other.ToString()
            };




        }
    }
}
