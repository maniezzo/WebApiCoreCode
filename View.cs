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
                Console.WriteLine("----------------");
                Console.WriteLine("0 - start");
                Console.WriteLine("1 - get customer name");
                Console.WriteLine("2 - get orders");
                Console.WriteLine("3 - exit");
                Console.Write("Insert an input: ");
                manage(Console.ReadLine());
            }
        }

        public void manage(string userInput) {
            switch (userInput) 
            {
                case "start": 
                case "0":
                    doSomething(this, new EventArgs());
                    break;
                case "get customer name":
                case "1":
                    Console.Write("Insert the id: ");
                    var id = Console.ReadLine();
                    getClientName(id);
                    break;
                case "get orders":
                case "2":
                    GetAvgAndVariance();
                    break;
                case "exit":
                case "3":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
        }

        private void GetAvgAndVariance()
        {
            C.GetAvgAndVariance();
        }

        private void viewEventHandler(object sender, string textToWrite)
        {
            Console.Write(textToWrite + Environment.NewLine);
        }

        private void doSomething(object sender, EventArgs e)
        {
            C.doSomething(); 
        }

        private void getClientName(string id) {
            C.getClientName(id);
        }
    }
}