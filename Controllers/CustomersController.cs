using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        WebApiCoreCode.Controller controller = new WebApiCoreCode.Controller();
        // GET api/customers/3
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return controller.getClientName(id);
        }

        // POST api/customers
        [HttpPost]
        public string Post([FromBody] Customer value)
        {
            return JsonConvert.SerializeObject("{\"name\":\" " + value.Name + "\"}");
        }
    }
}
