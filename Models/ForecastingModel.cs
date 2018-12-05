using System;
using System.Collections.Generic;
using System.Linq;
using static WebApiCoreCode.DBContext;


namespace WebApiCoreCode
{
    public class ForecastingModel
    {
        private List<int> Serie {get; set;}
        public List<double> Baseline {get; set;}

        public ForecastingModel(List<int> serie) 
        {
            this.Serie = serie;
        }
        
        public ForecastingModel applyMA(int seasonality) 
        {
            int startIndex = 0;
            int currentIndex = seasonality / 2;
            List<double> ma = new List<double>();
            for (int i = 0; i < currentIndex; i++) 
            {
                ma.Add(0);
            }
            while((startIndex + seasonality) <= (Serie.Count - seasonality))
            {
                ma.Add(Serie.GetRange(startIndex, seasonality).Average());
                startIndex++;
            }

            if ((seasonality % 2) == 0) 
            {
                List<double> temp = new List<double>(ma);
                currentIndex = seasonality / 2;
                Console.WriteLine("currentIndex: " + currentIndex + " Count: " + ma.Count);
                while((currentIndex + 1) < ma.Count) 
                {
                    temp[currentIndex] = ma.GetRange(currentIndex, 2).Average();
                    currentIndex++;
                }
                temp.RemoveAt(currentIndex);
                ma = temp;
            }

            this.Baseline = ma;
            return this;
        }
    }
}