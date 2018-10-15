using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using static WebApiCoreCode.DBContext;

namespace WebApiCoreCode
{
    public class ModelEF: AbstractModel
    {
        public override void GetCustomerName(string connString, string provider, string id) {
            using (var db = new DBContext(provider, connString))
            {
                foreach (Cliente c in db.Clienti)
                {
                    if (c.id.ToString().Equals(id))
                        Flush(this, c.nome.ToString());
                }
            }
        }
    
        public override void GetAvgAndVariance(string provider,string connString) {
            using (var db = new DBContext(provider, connString))
            {
                int totale = 0;
                foreach (Ordine order in db.Ordini)
                {
                    totale += order.codice;
                }
                float media = totale / (float)db.Ordini.Count();
                //float media = (float)db.Ordini.Average(o => o.codice);
                Flush(this, "Media: " + media);

                totale = 0;
                foreach (Ordine order in db.Ordini)
                {
                    totale += (int)Math.Pow(order.codice - media, 2);
                }
                float varianza = totale / (float)db.Ordini.Count();
                //float varianza = (float)db.Ordini.Average(o => Math.Pow(o.codice - media, 2));
                Flush(this, "Varianza: " + varianza);
            }
        }
    }
}
