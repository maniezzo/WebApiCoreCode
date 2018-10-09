using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using static WebApiCoreCode.DBContext;

namespace WebApiCoreCode
{
    public class Model: AbstractModel
    {
        public override void GetCustomerName(string connString, string provider, string id) {
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
                        Flush(this, "nome: " + reader["nome"]);
                    }
                }
            }
        }
    
        public override void GetDescr(string provider,string connString) {
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(provider);

            using (DbConnection conn = dbFactory.CreateConnection()) 
            {
                conn.ConnectionString = connString;
                conn.Open();
                IDbCommand com = conn.CreateCommand();
                com.CommandText = "select codice from ordini";
                using (IDataReader reader = com.ExecuteReader())
                {
                    while (reader.Read()) {
                        Flush(this, "codice: " + reader["codice"]);
                    }
                }
            }
        }
    }
}