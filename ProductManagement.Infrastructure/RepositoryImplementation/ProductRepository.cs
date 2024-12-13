﻿using Common.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using Random.App.ProductManagement.Domain.Entities;
using Random.App.ProductManagement.Domain.RepositoryInterfaces;
using Random.App.ProductManagement.Infrastructure.DataAccess;

namespace Random.App.ProductManagement.Infrastructure.RepositoryImplementation
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ProductDbContext _productDbContext;

        public ProductRepository(ProductDbContext context) : base(context)
        {
            _productDbContext = context;
        }

        public async Task<IEnumerable<Product>> GetPopularProducts(string keyword)
        {
            return await _productDbContext.Products
                .Where(p => p.Description.Contains(keyword))
                .ToListAsync();

        }

        public async Task<Product> GetProductByApiIdAsync(int originalApiId)
        {
            return await _productDbContext.Products.FirstOrDefaultAsync(p => p.OriginalApiId == originalApiId);
        }
    }
}
