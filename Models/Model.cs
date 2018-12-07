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

        public override IEnumerable<double> GetSeries(string name, string provider, string connString)
        {
            throw new NotImplementedException();
        }

        public override bool addCustomer(string connString, string provider,Cliente value)
        {
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(provider);

            using (DbConnection conn = dbFactory.CreateConnection()) 
            {
                conn.ConnectionString = connString;
                conn.Open();
                IDbCommand com = conn.CreateCommand();
                com.CommandText = "insert into clienti values (@id,@name);";
                IDbDataParameter param1 = com.CreateParameter();
                param1.DbType = DbType.Int32;
                param1.ParameterName = "@id";
                param1.Value = value.id;
                com.Parameters.Add(param1);
                IDbDataParameter param2 = com.CreateParameter();
                param2.DbType = DbType.String;
                param2.ParameterName = "@name";
                param2.Value = value.nome;
                com.Parameters.Add(param2);
                if (com.ExecuteNonQuery()>0) return true;
            }
            return false;
        }

        public override bool updateCustomer(string connString, string provider,int id, Cliente value)
        {
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(provider);

            using (DbConnection conn = dbFactory.CreateConnection()) 
            {
                conn.ConnectionString = connString;
                conn.Open();
                IDbCommand com = conn.CreateCommand();
                com.CommandText = "update clienti set nome=@name where id=@id;";
                IDbDataParameter param1 = com.CreateParameter();
                param1.DbType = DbType.Int32;
                param1.ParameterName = "@id";
                param1.Value = id;
                com.Parameters.Add(param1);
                IDbDataParameter param2 = com.CreateParameter();
                param2.DbType = DbType.String;
                param2.ParameterName = "@name";
                param2.Value = value.nome;
                com.Parameters.Add(param2);
                if (com.ExecuteNonQuery()>0) return true;
            }
            return false;
        }

        public override bool deleteCustomer(string connString, string provider,int id)
        {
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(provider);

            using (DbConnection conn = dbFactory.CreateConnection()) 
            {
                conn.ConnectionString = connString;
                conn.Open();
                IDbCommand com = conn.CreateCommand();
                com.CommandText = "delete from clienti where id=@id;";
                IDbDataParameter param1 = com.CreateParameter();
                param1.DbType = DbType.Int32;
                param1.ParameterName = "@id";
                param1.Value = id;
                com.Parameters.Add(param1);
                if (com.ExecuteNonQuery()>0) return true;
            }
            return false;
        }
    }
}