using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using static WebApiCoreCode.DBContext;

namespace WebApiCoreCode
{
    public class Model: AbstractModel
    {
        public override string GetCustomerName(string connString, string provider, int id) {
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(provider);

            using (DbConnection conn = dbFactory.CreateConnection()) 
            {
                conn.ConnectionString = connString;
                conn.Open();
                IDbCommand com = conn.CreateCommand();
                com.CommandText = "select nome from clienti where id=@id";
                IDbDataParameter param = com.CreateParameter();
                param.DbType = DbType.Int32;
                param.ParameterName = "@id";
                param.Value = id;
                com.Parameters.Add(param);
                using (IDataReader reader = com.ExecuteReader())
                {
                    while (reader.Read()) {
                        return reader["nome"].ToString();
                    }
                }
            }
            return "";
        }
    
        public override float[] GetAvgAndVariance(string provider,string connString) {
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(provider);

            float[] result = new float[2];
            int count = 0;

            using (DbConnection conn = dbFactory.CreateConnection()) 
            {
                conn.ConnectionString = connString;
                conn.Open();
                IDbCommand com = conn.CreateCommand();
                com.CommandText = "select codice from ordini";
                using (IDataReader reader = com.ExecuteReader())
                {
                    int totale = 0;

                    while (reader.Read()) {
                        totale += int.Parse(reader["codice"].ToString());
                        count++;
                    }
                    result[0] = totale / (float)count;
                }
                using (IDataReader reader = com.ExecuteReader())
                {
                    int totale = 0;
                    while (reader.Read())
                    {
                        totale += (int)Math.Pow(int.Parse(reader["codice"].ToString()) - result[0], 2);
                    }
                    result[1] = totale / (float)count;
                }
                return result;
            }
        }

        public override IEnumerable<string> GetSeries(string provider, string connString)
        {
            throw new NotImplementedException();
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
    }
}