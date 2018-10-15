public abstract class AbstractModel
{
    public delegate void viewEventHandler(object sender, string textToWrite);
    public event viewEventHandler FlushText;
    public void doSomething()
    {
        for (int i = 0; i < 10; i++) 
            FlushText(this, $"i={i}");
        
    }
    public abstract void GetCustomerName(string connString, string provider, string id);
    public abstract void GetAvgAndVariance(string provider,string connString);

    public void Flush(object o, string s)
    {
        this.FlushText(o, s);
    }
}