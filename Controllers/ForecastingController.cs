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
        [HttpGet("{name}/{index}")]
        public ActionResult<string> Get(string name, string index)
        {
            return JsonConvert.SerializeObject(controller.doForecasting(name, index == "Pearson").Select(x => x.Select(y => y.ToString("0")).ToArray()));
        }
    }
}
