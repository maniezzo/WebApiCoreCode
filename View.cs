using System;

namespace WebApiCoreCode
{
    public class View
    {
        Controller C = new Controller();
        public View() {
            C.FlushText += viewEventHandler;
        }

        public void play() {
            while (true) {
                Console.Write("Insert an input: ");
                var userInput = Console.ReadLine();
                manage(userInput);
            }
        }

        public void manage(string userInput) {
            switch (userInput) 
            {
                case "start": 
                    doSomething(this, new EventArgs());
                    break;
                case "go query":
                    Console.Write("Insert a query: ");
                    var query = Console.ReadLine();
                    goQuery(query);
                    break;
                case "get client name":
                    Console.Write("Insert the id: ");
                    var id = Console.ReadLine();
                    getClientName(id);
                    break;
                case "exit":
                    Environment.Exit(0);
                    break;
            }
        }

        private void viewEventHandler(object sender, string textToWrite)
        {
            Console.Write(textToWrite + Environment.NewLine);
        }

        private void doSomething(object sender, EventArgs e)
        {
            C.doSomething(); 
        }

        private void goQuery(string query) {
            C.goQuery(query);
        }

        private void getClientName(string id) {
            C.getClientName(id);
        }


    }
}
