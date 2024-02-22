using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace User.Management.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize (Roles ="Admin")]
    public class AdminController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> GetUser()
        {
            return new List<string> { "Sahil","Aman","Raghav"};
        }
    }
}
