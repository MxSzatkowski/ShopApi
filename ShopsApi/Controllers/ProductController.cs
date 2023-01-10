using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopsApi.Entities;
using ShopsApi.Models;
using ShopsApi.Services;

namespace ShopsApi.Controllers
{
    [Route("/api/shop/{shopId}/product")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Get([FromRoute] int shopId)
        {
            var product = _productService.Get(shopId);
            return Ok(product);

        }

        [HttpPost]
        [Authorize(Roles = "Admin,Owner")]
        public ActionResult Add([FromRoute]int shopId, [FromForm]CreateProductDto dto)
        {
            _productService.Create(shopId, dto);
            return Ok("Created");
        }

        [HttpDelete("{productId}")]
        [Authorize(Roles = "Admin,Owner")]
        public ActionResult Delete([FromRoute]int shopId, [FromRoute] int productId)
        {
            _productService.DeleteById(shopId, productId);
            return Ok("Deleted");
        }

    }

}
