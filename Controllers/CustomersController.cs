using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static WebApiCoreCode.DBContext;

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
            return JsonConvert.SerializeObject(controller.getCustomerName(id));
        }

        // POST api/customers
        [HttpPost]
        public ActionResult<string> Post([FromBody] Cliente value)
        {
            if (controller.addCustomer(value))
                return JsonConvert.SerializeObject("Customer created");
            else
                return JsonConvert.SerializeObject("Error during creation");
        }

        // PUT api/customers/5
        [HttpPut("{id}")]
        public ActionResult<string> Put(int id, [FromBody] Cliente value)
        {
            if (controller.updateCustomer(id, value))
                return JsonConvert.SerializeObject("Customer updated");
            else
                return JsonConvert.SerializeObject("Error during update");
        }

        // DELETE api/customers/pippo
        [HttpDelete("{id}")]
        [ActionName("deleteUser")]
        public ActionResult<string> Delete(int id)
        {
            if (controller.deleteCustomer(id))
                return JsonConvert.SerializeObject("Customer deleted");
            else
                return JsonConvert.SerializeObject("Error during delete");
        }
    }
}
