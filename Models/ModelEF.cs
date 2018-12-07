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

        public override IEnumerable<double> GetSeries(string name, string provider,string connString) {
            //List<string> result = new List<string>();
            using (var db = new DBContext(provider, connString))
            {

                switch (name) {
                        case "esempio": 
                            foreach (SerieRecord s in db.Serie.Take(20))
                            {
                                yield return s.esempio.GetValueOrDefault();
                            }
                            break;
                        case "esempio2": 
                            foreach (SerieRecord s in db.Serie.Take(200))
                            {
                                yield return s.esempio2.GetValueOrDefault();
                            }
                            break;
                        case "Passengers": 
                            foreach (SerieRecord s in db.Serie.Take(144))
                            {
                                yield return s.Passengers.GetValueOrDefault();
                            }
                            break;
                        case "jewelry": 
                            foreach (SerieRecord s in db.Serie)
                            {
                                yield return s.jewelry.GetValueOrDefault();
                            }
                            break;
                    }
                
            }
            //return result;
        }

        public override bool addCustomer(string connString, string provider, Cliente value)
        {
            var db = new DBContext(provider, connString);
            try
            {
                db.Clienti.Add(value);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
            finally 
            {
                db.Dispose();
            }
        }

        public override bool updateCustomer(string connString, string provider,int id, Cliente value)
        {
            var db = new DBContext(provider, connString);
            try
            {
                db.Clienti.Find(id).nome = value.nome;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
            finally 
            {
                db.Dispose();
            }
        }

        public override bool deleteCustomer(string connString, string provider,int id)
        {
            var db = new DBContext(provider, connString);
            try
            {
                db.Clienti.Remove(db.Clienti.Find(id));
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
            finally 
            {
                db.Dispose();
            }
        }
    }
}
