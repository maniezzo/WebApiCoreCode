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
            this.forecastingModel = new ForecastingModel(model.GetSeries(provider, connString).Select(x => int.Parse(x)).ToList());
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

        public IEnumerable<string> GetSeries()
        {
            return model.GetSeries(provider, connString);
        }

        public IEnumerable<String> doForecasting() 
        {
            this.forecastingModel.applyMA(12).calculateSeasonality().deleteNoise().seasonAdjustement().calculateTrend();
            return this.forecastingModel.Trend.Select(x => x.ToString());
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

        public string solveGAP(string name)
        {
            GeneralizedAssignmentProblem problem = optimizationModel.readJson("problems/"+name);

            int[] sol = optimizationModel.findSol();

            try 
            {
                return optimizationModel.checkSol(sol).ToString();
            } 
            catch (Exception e)
            {
                return e.Message +'\n' + String.Join(',', sol);
            }
        }
    }
}