using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoCrafts.WebSite.Controllers
{
    [Route("[controller]")]    // Or use [Route("/Products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public JsonFileProductService ProductService { get; }
        public ProductsController(JsonFileProductService productService)
        {
            this.ProductService = productService;
        }

        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return ProductService.GetProducts();
        }

        //Except [HttpGet] for that type of methods better pick more suitable attributes, like down:
        //  [HttpPost] - Make a new thing in a database
        //  [HttpPut] - Update a database
        //  [HttpPatch] - Changing a little. [HttpPatch] "[FromBody]" - of the patch
        //  [HttpDelete] - For deleting
        [Route("Rate")] //The URL of this method would be Products/Rate
        [HttpGet] //Example of URL "localhost:5001/Products/Rate?ProductId=jenlooper-light&Rating=3"
        public ActionResult Get( // Name method is tested name. Better rename this method somehow.
            [FromQuery] string ProductId, 
            [FromQuery] int Rating)
        {
            ProductService.AddRating(ProductId, Rating);
            return Ok();
        }
    }
}
