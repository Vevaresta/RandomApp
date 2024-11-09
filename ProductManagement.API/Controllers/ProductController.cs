using Common.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Random.App.ProductManagement.API.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; 
        }
    }
}
