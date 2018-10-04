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

namespace WebApiCoreCode
{
    public class Controller
    {
        Model model = new Model();
        public delegate void viewEventHandler(object sender, string textToWrite); 
        public event viewEventHandler FlushText;
        /*string connString1 = @"Data Source=testDB.db; Version=3";
        string providerName1 = "System.Data.SQLite";
        string connString2 = "Data Source=(sql_server_demo)/MSSQLLocalDB;Initial Catalog = testDb;Integrated Security =True;Connect Timeout=20";
        string providerName2 = "System.Data.SqlClient";*/
        string connString = "";
        string factory = "";


        public Controller() { 
            model.FlushText += controllerViewEventHandler; 
            string sdb = ConfigurationManager.AppSettings["dbServer"];
            switch (sdb)
            { 
                case "SQLiteConn":  
                    connString = ConfigurationManager.ConnectionStrings["SQLiteConn"].ConnectionString;
                    factory = ConfigurationManager.ConnectionStrings["SQLiteConn"].ProviderName;
                    break;
                case "LocalSqlServConn":
                    connString = ConfigurationManager.ConnectionStrings["LocalSqlServConn"].ConnectionString;
                    factory = ConfigurationManager.ConnectionStrings["LocalSqlServConn"].ProviderName;
                    break;
            }
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
            model.GetCustomerName(connString, factory, id);
        }
    }
}
