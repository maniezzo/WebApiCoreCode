using System;
using System.Data;

namespace WebApiCoreCode
{
    public class GeneralizedAssignmentProblem
    {
       public string name {get; set;}
       public int numcli {get; set;}
       public int numserv {get; set;}
       public int[,] cost {get; set;}
       public int[,] req {get; set;}
       public int[] cap {get; set;}
    }
}