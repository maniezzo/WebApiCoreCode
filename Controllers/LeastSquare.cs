using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data.Common;
using System.Linq;
using static WebApiCoreCode.DBContext;
using LinqStatistics;

namespace WebApiCoreCode 
{
    public static class myLeastSquares 
    {
        public static LeastSquares LeastSquares(this IEnumerable<Tuple<double, double>> source)
        {
            int numPoints = 0;
            double sumX = 0;
            double sumY = 0;
            double sumXX = 0;
            double sumXY = 0;

            foreach (var tuple in source)
            {
                numPoints++;
                sumX += tuple.Item1;
                sumY += tuple.Item2;
                sumXX += tuple.Item1 * tuple.Item1;
                sumXY += tuple.Item1 * tuple.Item2;
            }

            if (numPoints < 2)
                throw new InvalidOperationException("Source must have at least 2 elements");

            double b = (-sumX * sumXY + sumXX * sumY) / (numPoints * sumXX - sumX * sumX);
            double m = (-sumX * sumY + numPoints * sumXY) / (numPoints * sumXX - sumX * sumX);

            return new LeastSquares(m, b);
        }
    }
}