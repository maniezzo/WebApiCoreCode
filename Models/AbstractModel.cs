using System;
using System.Collections.Generic;
using static WebApiCoreCode.DBContext;

public abstract class AbstractModel
{
    public delegate void viewEventHandler(object sender, string textToWrite);
    public event viewEventHandler FlushText;
    public void doSomething()
    {
        for (int i = 0; i < 10; i++) 
            FlushText(this, $"i={i}");
        
    }
    public abstract string GetCustomerName(string connString, string provider, int id);
    public abstract float[] GetAvgAndVariance(string provider,string connString);
    public abstract IEnumerable<double> GetSeries(string provider,string connString);
    
    public void Flush(object o, string s)
    {
        this.FlushText(o, s);
    }

    public abstract bool addCustomer(string connString, string provider,Cliente value);
    public abstract bool updateCustomer(string connString, string provider,int id, Cliente value);
    public abstract bool deleteCustomer(string connString, string provider,int id);
}
