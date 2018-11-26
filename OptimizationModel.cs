using System;
using System.Data;
using Newtonsoft.Json;


namespace WebApiCoreCode
{
    public class OptimizationModel
    {
        public OptimizationModel() { 

        }

        public GeneralizedAssignmentProblem readJson(string path) {
            return JsonConvert.DeserializeObject<GeneralizedAssignmentProblem>(System.IO.File.ReadAllText(path));
        }
    }
}