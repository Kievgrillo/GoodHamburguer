using GoodHamburger.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburguer.Api.Controllers
{
    [ApiController]
    [Route("api/menu")]
    public class MenuController(ServiceMenu menuService) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetMenu() => Ok(menuService.GetMenu());
    }
}

