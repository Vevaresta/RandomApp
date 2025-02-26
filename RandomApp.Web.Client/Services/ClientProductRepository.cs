//using AutoMapper;
//using Common.Shared.Http;
//using RandomApp.ProductManagement.Domain.Entities;
//using RandomApp.ProductManagement.Domain.RepositoryInterfaces;
//using System.Linq.Expressions;
//using System.Net.Http.Json;

//namespace RandomApp.Web.Client.Services
//{
//    public class ClientProductRepository : ApiClientBase, IProductRepository
//    {
//        private readonly IMapper _mapper;
//        public const string products_endpoint = "api/product/all";
//        public ClientProductRepository(IHttpClientCreator httpClientCreator, IMapper mapper) : base(httpClientCreator)
//        {   
//            _mapper = mapper;
//        }

//        public async Task<IEnumerable<Product>> GetAllAsync()
//        {
//            try
//            {
//                var response = await HttpClient.GetAsync(products_endpoint);

//                if (!response.IsSuccessStatusCode)
//                {
//                    return Enumerable.Empty<Product>();
//                }

//                var content = await response.Content.ReadAsStringAsync();

//                var products = await response.Content.ReadFromJsonAsync<IEnumerable<Product>>();
//                if (products == null)
//                {

//                }
//                else
//                {

//                }

//                if (products == null)
//                {
//                    return Enumerable.Empty<Product>();
//                }
//                else
//                {
//                    return products;
//                }
//            }
//            catch (Exception ex)
//            {
//                throw;
//            }
//        }


//        public Task<IEnumerable<Product>> GetPopularProducts(string keyword)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<Product> GetProductByApiIdAsync(int originalApiId)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<Product> GetByIdAsync(int id)
//        {
//            throw new NotImplementedException();
//        }



//        public Task<IEnumerable<Product>> Find(Expression<Func<Product, bool>> expression)
//        {
//            throw new NotImplementedException();
//        }

//        public Task AddAsync(Product entity)
//        {
//            throw new NotImplementedException();
//        }

//        public Task AddRangeAsync(IEnumerable<Product> entities)
//        {
//            throw new NotImplementedException();
//        }

//        public void Update(Product entity)
//        {
//            throw new NotImplementedException();
//        }

//        public void Remove(Product entity)
//        {
//            throw new NotImplementedException();
//        }

//        public void RemoveRange(IEnumerable<Product> entities)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
