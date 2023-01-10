using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopsApi.Models;
using ShopsApi.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShopsApi.Controllers
{
    [Route("/api/shop")]
    [ApiController]
    [Authorize]
    public class ShopController : ControllerBase
    {
        private readonly IShopService _shopService;

        public ShopController(IShopService shopService)
        {
            _shopService = shopService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Get([FromQuery] ShopQuery query)
        {
            var shop = _shopService.Get(query);
            return Ok(shop);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Owner")]
        public ActionResult Add([FromForm] CreateShopDto dto)
        {
            var userId = int.Parse(User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value);
            _shopService.Create(dto);
            return Ok("Created");
        }

        [HttpDelete("{shopId}")]
        [Authorize(Roles = "Admin,Owner")]
        public ActionResult Delete([FromRoute]int shopId)
        {
            _shopService.Delete(shopId);
            return Ok("Deleted");
        }

        [HttpPut("{shopId}")]
        [Authorize(Roles = "Admin,Owner")]
        public ActionResult Update([FromRoute]int shopId, [FromForm]UpdateShopDto dto)
        {
            _shopService.Update(shopId, dto);
         
            return Ok("Shop information updated");
        }

    }
    
}
