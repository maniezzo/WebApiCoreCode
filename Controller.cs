using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Configuration;
using Newtonsoft.Json;
using System.Data.Common;

namespace WebApiCoreCode
{
    public class Controller
    {
        Model model = new Model();
        public delegate void viewEventHandler(object sender, string textToWrite); 
        public event viewEventHandler FlushText;
        string connString;
        string provider;
        string factory;


        public Controller() { 

            model.FlushText += controllerViewEventHandler; 

            dynamic config = JsonConvert.DeserializeObject(System.IO.File.ReadAllText("settings.json"));

            switch (config.settings.value.ToString())
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
            }
            
            DbProviderFactories.RegisterFactory(provider, factory);
        }

        private void controllerViewEventHandler(object sender, string textToWrite)
        { 
            FlushText(this, textToWrite); 
        }

        public void doSomething()
        { 
            model.doSomething();
        }

        public void goQuery(string query)
        {
            model.goQuery(connString, query);
        }

        public void getClientName(string id)
        {
            model.GetCustomerName(connString, provider, id);
        }
    }
}
