using Random.App.ProductManagement.Domain.Entities;
using Random.App.ProductManagement.Infrastructure.DataTransferObjects;

namespace Random.App.ProductManagement.Infrastructure.Mapping
{
    //  A static mapper class is like a utility that takes input, transforms it, and produces output, without needing any internal state or instance-specific data.
    public static class ProductMapper
    {
        public static Product ToEntity(this ProductDto dto)
        {
            return new Product
            {
                Id = dto.Id,
                Name = dto.Title,
                Price = dto.Price,
                Category = dto.Category,
                Description = dto.Description,
                Image = dto.Image
            };
        }
    }
}
