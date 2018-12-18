using System;
using System.Collections.Generic;
using System.Linq;
using LinqStatistics;
using static WebApiCoreCode.DBContext;

namespace WebApiCoreCode
{
      public class DataPoint
      {
        public double X, Y;
        public int RankByX, RankByY;
      }
      public static class RankCorrelation {
        public static long lsqr(long d)
        {
          return d * d;
        }
        public static double ComputeRankCorrelation(double[] X, double[] Y)
        {
          var n = Math.Min(X.Length, Y.Length);
          var list = new List<DataPoint>(n);
          for (var i = 0; i < n; i++)
          {
            list.Add(new DataPoint() { X = X[i], Y = Y[i] });
          }
          var byXList = list.OrderBy(r => r.X).ToArray();
          var byYList = list.OrderBy(r => r.Y).ToArray();
          for (var i = 0; i < n; i++)
          {
            byXList[i].RankByX = i + 1;
            byYList[i].RankByY = i + 1;
          }
          var sumRankDiff 
            = list.Aggregate((long)0, (total, r) => 
            total += lsqr(r.RankByX - r.RankByY));
          var rankCorrelation 
            = 1 - (double)(6 * sumRankDiff) 
            / (n * ((long)n * n - 1));
          return rankCorrelation;
        }
      }
    
}