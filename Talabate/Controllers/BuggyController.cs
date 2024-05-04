using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Repositery.Data;
using Talabate.Errors;

namespace Talabate.Controllers
{
  
    public class BuggyController : BaseApiController
    {
        private readonly StoreContext _dbcontext;

        public BuggyController(StoreContext dbcontext)
        {
           _dbcontext = dbcontext;
        }
        [HttpGet ("NotFound")]
        public ActionResult GetNotFoundRequest()
        {
            var product = _dbcontext.Products.Find(100);
            if (product is null)
                return NotFound(new ApiResponse(404));
            return Ok(product);
        }
        [HttpGet("Server Error")]
        public ActionResult GetServererror()
        {
            var product = _dbcontext.Products.Find(100);

            var productToReturn = product.ToString();
            return Ok(productToReturn);
        }
        [HttpGet("badrequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }
        [HttpGet("badrequest/{id}")]
        public ActionResult GetBadRequest(int id)
        {

            return Ok();
        }



    }
}
