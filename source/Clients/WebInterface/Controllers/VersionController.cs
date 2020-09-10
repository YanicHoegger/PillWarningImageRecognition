using Clients.Shared;
using Microsoft.AspNetCore.Mvc;

namespace WebInterface.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VersionController : Controller
    {
        [HttpGet]
        public string Get()
        {
            return VersionConstant.Current;
        }
    }
}
