using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForecastingController : ControllerBase
    {
        WebApiCoreCode.Controller controller = new WebApiCoreCode.Controller();
        // GET api/forecasting
        [HttpGet]
        public ActionResult<string> Get()
        {
            return JsonConvert.SerializeObject(controller.GetMA().ToArray());
        }
    }
}
