using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Multi_Tenancy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok();
        }
    }
}
