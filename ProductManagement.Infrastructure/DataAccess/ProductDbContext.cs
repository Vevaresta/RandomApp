﻿using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Entities;


namespace ProductManagement.Infrastructure.ORM
{
    public class ProductDbContext : DbContext
    {

        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Coca Cola Zero", Description = "Sugar Free Drink" });
        }
    }
}
