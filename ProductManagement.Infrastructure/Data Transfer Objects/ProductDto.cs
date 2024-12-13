using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Random.App.ProductManagement.Infrastructure.Data_Transfer_Objects
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

    }
}
