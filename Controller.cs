using System;

namespace WebApiCoreCode
{
    public class Controller
    {
        Model model = new Model();
        public delegate void viewEventHandler(object sender, string textToWrite); 
        public event viewEventHandler FlushText;
        string connString = @"Data Source=testDB.db; Version=3";


        public Controller() { 
            model.FlushText += controllerViewEventHandler; 
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
            model.getClientName(connString, id);
        }
    }
}
