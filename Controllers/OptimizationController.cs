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
    public class OptimizationController : ControllerBase
    {
        WebApiCoreCode.Controller controller = new WebApiCoreCode.Controller();

        // GET api/optimization/trivial/gap10
        [HttpGet("{name}/{algorithm}")]
        public ActionResult<string> Get(string name,string algorithm)
        {
            string fullname = name + (name.EndsWith(".json") ? "" : ".json");
            return JsonConvert.SerializeObject(controller.solveGAP(fullname,algorithm).Split(' '));
        }

        // GET api/optimization/problems
        [HttpGet("problems")]
        public ActionResult<string> Get()
        {
            return JsonConvert.SerializeObject(
                System.IO.Directory.GetFiles(@"problems\", "*.json").Select(n => n.Replace(@"problems\",""))
            );
        }
    }
}
