using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using static WebApiCoreCode.DBContext;

namespace WebApiCoreCode
{

    public class ModelEF: AbstractModel
    {
        public override string GetCustomerName(string connString, string provider, int id) {
            using (var db = new DBContext(provider, connString))
            {
                foreach (Cliente c in db.Clienti)
                {
                    if (c.id == id)
                        return c.nome;
                }
            }
            return "";
        }
        public override bool addCustomer(Customer value)
        {
            return true;
        }

        public override bool updateCustomer(int id, Customer value)
        {
            return true;
        }

        public override bool deleteCustomer(int id)
        {
            return true;
        }

    
        public override float[] GetAvgAndVariance(string provider,string connString) {
            using (var db = new DBContext(provider, connString))
            {
                float[] result = new float[2];

                int totale = 0;
                foreach (Ordine order in db.Ordini)
                {
                    totale += order.codice;
                }
                float media = totale / (float)db.Ordini.Count();
                //float media = (float)db.Ordini.Average(o => o.codice);
                result[0] = media;

                totale = 0;
                foreach (Ordine order in db.Ordini)
                {
                    totale += (int)Math.Pow(order.codice - media, 2);
                }
                float varianza = totale / (float)db.Ordini.Count();
                //float varianza = (float)db.Ordini.Average(o => Math.Pow(o.codice - media, 2));
                result[1] = varianza;
                return result;
            }
        }

        public override IEnumerable<string> GetSeries(string provider,string connString) {
            List<string> result = new List<string>();
            using (var db = new DBContext(provider, connString))
            {
                foreach (SerieRecord s in db.Serie)
                {
                    result.Add(s.esempio + " " + s.esempio2 + " " + s.Passengers + " " + s.jewelry);
                }
            }
            return result;
        }
    }
}
