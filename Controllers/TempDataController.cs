using authmodule.Db;
using authmodule.Services;
using Microsoft.AspNetCore.Mvc;

namespace authmodule.Controllers
{
    [Route("/tempdata")]
    [ApiController]
    public class TempDataController : ControllerBase
    { 

        private readonly ITempDataService _tempDataService;
        public TempDataController(ITempDataService tempDataService) {
            _tempDataService = tempDataService;
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetData() 
        {
            Object? result = await _tempDataService.GetData();
            return Ok(result);
        }
    }
}