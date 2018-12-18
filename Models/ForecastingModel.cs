using System;
using System.Collections.Generic;
using System.Linq;
using LinqStatistics;
using static WebApiCoreCode.DBContext;

namespace WebApiCoreCode
{
    public class ForecastingModel
    {
        public int seasonalityRate;
        private List<double> Serie {get; set;}
        public List<double> Baseline {get; set;}
        public List<double> Seasonality {get; set;}
        private List<double> Seasons = new List<double>();
        public List<double> SeasonalityWithoutNoise = new List<double>();
        public List<double> SeasonallyAdjustedData {get; set;}
        public LeastSquares LeastSquaresParameters;
        public List<double> Trend;
        public List<double> ForecastedData;

        public ForecastingModel(IEnumerable<double> serie) 
        {
            this.Serie = serie.ToList();
        }
        public ForecastingModel findSeasonality() 
        {
            double[] arrSource = this.Serie.ToArray();
            double pearson;  
            double max = -1; 
            for(int shifts = 1; shifts <= 20; shifts++) {
                double[] arr = new double[arrSource.Length];
                Array.Copy(arrSource, 0, arr, shifts, arr.Length - shifts);
                pearson = StatisticsModel.Pearson(arrSource,arr);
                
                if (pearson > max) 
                {
                    seasonalityRate = shifts;
                    max = pearson;
                }
            }
            Console.WriteLine("pearson: " + max + " seasonalityRate: " + seasonalityRate);
            return this;
        }
        //media mobile
        public ForecastingModel applyMA() 
        {
            int startIndex = 0;
            int currentIndex = seasonalityRate / 2;
            List<double> ma = new List<double>();
            for (int i = 0; i < currentIndex; i++) 
            {
                ma.Add(1);
            }
            while((startIndex + seasonalityRate) <= (Serie.Count() - seasonalityRate))
            {
                ma.Add(Serie.GetRange(startIndex, seasonalityRate).Average());
                startIndex++;
            }

            if ((seasonalityRate % 2) == 0) 
            {
                List<double> temp = new List<double>(ma);
                currentIndex = seasonalityRate / 2;
                //Console.WriteLine("currentIndex: " + currentIndex + " Count: " + ma.Count);
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

        public ForecastingModel calculateSeasonality() 
        {
            this.Seasonality = new List<double>();
            for(int i = 0; i < this.Baseline.Count; i++) 
            {
                //Console.WriteLine("Serie: " + this.Serie.ElementAt(i) + " Baseline: " + this.Baseline.ElementAt(i));
                this.Seasonality.Add(this.Serie.ElementAt(i) / this.Baseline.ElementAt(i));
            }
            for (int i = 0; i < seasonalityRate / 2 * 3; i++) {
                this.Seasonality.Add(1);
            }
            if ((seasonalityRate % 2) == 1)
            {
                this.Seasonality.Add(1);
            } 
            return this;
        }

        public ForecastingModel deleteNoise() 
        {
            for (int i = 0; i < this.Seasonality.Count; i++) {
                Seasons.Add(i % this.seasonalityRate);
            }
            for (int i = 0; i < seasonalityRate; i++) {
                List<int> indeces = new List<int>();
                for (int j = 0; j < Seasons.Count; j++) {
                    if (Seasons.ElementAt(j) == i) 
                    {
                        indeces.Add(j);
                    }
                }
                List<double> temp = new List<double>();
                indeces.Where(index => index > seasonalityRate / 2)
                       .Where(index => index < (this.Baseline.Count - (seasonalityRate / 2))).ToList()
                       .ForEach(index => temp.Add(this.Seasonality.ElementAt(index)));
                //temp.ForEach(x => Console.WriteLine(x));
                this.SeasonalityWithoutNoise.Add(temp.Average());
            }
            return this;
        }

        public ForecastingModel seasonAdjustement() 
        {
            this.SeasonallyAdjustedData = new List<double>();
            for(int i = 0; i < this.Seasons.Count; i++) 
            {   
                this.Seasons[i] = this.SeasonalityWithoutNoise.ElementAt((int)(this.Seasons.ElementAt(i)));
            }
            for(int i = 0; i < this.Serie.Count - seasonalityRate; i++) 
            {
                this.SeasonallyAdjustedData.Add(this.Serie.ElementAt(i) / this.Seasons.ElementAt(i));
            }
            return this;
        }
        public ForecastingModel calculateTrend() 
        {
            this.Trend = new List<double>();
            List<Tuple<double, double>> LeastSquaresSerie = new List<Tuple<double, double>>();
            for (int i = 0; i < this.SeasonallyAdjustedData.Count; i++) 
            {
                LeastSquaresSerie.Add(new Tuple<double, double>(i+1, this.SeasonallyAdjustedData.ElementAt(i)));
            }
            //y = mx + b
            LeastSquaresParameters = StatisticsModel.LeastSquares(LeastSquaresSerie);
            for (int i = 0; i < this.Serie.Count; i++) {
                this.Trend.Add((i + 1) * LeastSquaresParameters.M + LeastSquaresParameters.B);
            }
            return this;
        }

        public ForecastingModel forecast()
        {
            this.ForecastedData = new List<double>();
            Console.WriteLine("Trend: " + this.Trend.Count + " Seasons: " + this.Seasons.Count);
            for (int i = 0; i < Serie.Count; i++) {
                this.ForecastedData.Add(this.Trend.ElementAt(i) * this.Seasons.ElementAt(i));
            }
            return this;
        }

    }
}