using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data.Common;
using System.Linq;
using static WebApiCoreCode.DBContext;

namespace WebApiCoreCode
{
    public class Controller
    {
        AbstractModel model;
        OptimizationModel optimizationModel;
        ForecastingModel forecastingModel;

        public delegate void viewEventHandler(object sender, string textToWrite); 
        public event viewEventHandler FlushText;
        string connString;
        string provider;
        string factory;


        public Controller() { 

            optimizationModel = new OptimizationModel();

            dynamic config = JsonConvert.DeserializeObject(System.IO.File.ReadAllText("settings.json"));

            switch(config.settings.strategy.ToString())
            {
                case "Factory":
                    model = new Model();
                    break;
                case "ORM":
                    model = new ModelEF();
                    break;
            }

            model.FlushText += controllerViewEventHandler; 

            switch (config.settings.dbServer.ToString())
            { 
                case "SQLiteConn":  
                    connString = config.connectionStrings[0].connectionString;
                    provider = config.connectionStrings[0].providerName;
                    factory = config.connectionStrings[0].factory;
                    break;
                case "LocalSqlServConn":
                    connString = config.connectionStrings[1].connectionString;
                    provider = config.connectionStrings[1].providerName;
                    factory = config.connectionStrings[1].factory;
                    break;
                case "RemoteSqlServConn":
                    connString = config.connectionStrings[2].connectionString;
                    provider = config.connectionStrings[2].providerName;
                    factory = config.connectionStrings[2].factory;
                    break;
            }
            
            DbProviderFactories.RegisterFactory(provider, factory);
        }

        public bool addCustomer(Cliente value)
        {
            return model.addCustomer(connString, provider,value);
        }
        public bool updateCustomer(int id, Cliente value)
        {
            return model.updateCustomer(connString, provider,id, value);
        }
        internal bool deleteCustomer(int id)
        {
            return model.deleteCustomer(connString, provider,id);
        }

        public IEnumerable<double> GetSeries(string name)
        {
            var series = model.GetSeries(name, provider, connString);
            this.forecastingModel = new ForecastingModel(series);
            return series;
        }

        public IEnumerable<IEnumerable<double>> doForecasting(string name, Boolean isPearson) 
        {
            List<double> series = this.GetSeries(name).ToList();

            this.forecastingModel
                .findSeasonality(isPearson)
                .applyMA()//MA and Baseline
                .calculateSeasonality()
                .deleteNoise()
                .seasonAdjustement()
                .calculateTrend()
                .forecast();

            List<double> forecast = this.forecastingModel.ForecastedData.TakeLast(this.forecastingModel.seasonalityRate).ToList();
            List<List<double>> result = new List<List<double>>();
            result.Add(series.Take(series.Count - this.forecastingModel.seasonalityRate).Concat(forecast).ToList());
            result.Add(series.Take(series.Count - this.forecastingModel.seasonalityRate).ToList());
            
            return result;
        }

        public float[] GetAvgAndVariance()
        {
            return model.GetAvgAndVariance(provider, connString);
        }

        private void controllerViewEventHandler(object sender, string textToWrite)
        { 
            FlushText(this, textToWrite); 
        }

        public void doSomething()
        { 
            model.doSomething();
        }

        public string getCustomerName(int id)
        {
            return model.GetCustomerName(connString, provider, id);
        }

        public string solveGAP(string name, string algoritm)
        {
            optimizationModel.readJson("problems/" + name);
            int[] sol = optimizationModel.findSol();

            if (optimizationModel.isSolValid(sol)) 
            {
                switch (algoritm)
                {
                    case "gap10":
                        return  algoritm + " " + optimizationModel.writeSol(optimizationModel.Gap10(sol));
                    case "SimulatedAnnealing":
                            int[] sol3 = null;
                            for (int i = 0; i < 5; i++)
                            {
                                int[] sol3tmp = optimizationModel.SimulatedAnnealing(sol);
                                if(i==0) sol3= sol3tmp;
                                else{
                                    sol3= optimizationModel.getCost(sol3)> optimizationModel.getCost(sol3tmp)?sol3tmp: sol3;
                                }
                            }
                            return  algoritm + " " + optimizationModel.writeSol(sol3);
                    case "TabuSearch":
                        return  algoritm + " " + optimizationModel.writeSol(optimizationModel.TabuSearch(sol));
                    default: return "";
                 }
            } 
            else
            {
                return "No intial solution found." ;
            }
        }
    }
}