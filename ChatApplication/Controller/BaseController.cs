using Microsoft.AspNetCore.Mvc;

namespace ChatApplication.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public IConfiguration _configuration { get; }
        public BaseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
