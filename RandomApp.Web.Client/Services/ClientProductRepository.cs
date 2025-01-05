using AutoMapper;
using NLog;
using RandomApp.ProductManagement.Domain.Entities;
using RandomApp.ProductManagement.Domain.RepositoryInterfaces;
using System.Linq.Expressions;
using System.Net.Http.Json;

namespace RandomApp.Web.Client.Services
{
    public class ClientProductRepository : ApiClientBase, IProductRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public const string products_endpoint = "api/product/all";
        public ClientProductRepository(IHttpClientCreator httpClientCreator, IMapper mapper, ILogger logger) : base(httpClientCreator)
        {   
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                _logger.Info($"Calling {products_endpoint}");
                var response = await HttpClient.GetAsync(products_endpoint);
                _logger.Info($"Response status: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.Error($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    return Enumerable.Empty<Product>();
                }

                var content = await response.Content.ReadAsStringAsync();
                _logger.Info($"Response content: {content}");

                var products = await response.Content.ReadFromJsonAsync<IEnumerable<Product>>();
                _logger.Info($"Parsed {products?.Count() ?? 0} products");
                return products ?? Enumerable.Empty<Product>();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error fetching products");
                throw;
            }
        }


        public Task<IEnumerable<Product>> GetPopularProducts(string keyword)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProductByApiIdAsync(int originalApiId)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }



        public Task<IEnumerable<Product>> Find(Expression<Func<Product, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(Product entity)
        {
            throw new NotImplementedException();
        }

        public Task AddRangeAsync(IEnumerable<Product> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(Product entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(Product entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<Product> entities)
        {
            throw new NotImplementedException();
        }
    }
}
