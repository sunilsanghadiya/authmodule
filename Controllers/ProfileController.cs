using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace authmodule.Controllers
{
    [ApiController]
    [Route("Profile")]
    [Authorize]
    public class ProfileController : Controller
    {
        public ProfileController() 
        {

        }

        // [HttpGet("GetUserProfile")]
        // public async Task<IActionResult> GetUserProfile()
        // {
            
        // }
    }
}