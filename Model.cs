using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace WebApiCoreCode
{
    public class Model
    {
        public delegate void viewEventHandler(object sender, string textToWrite);
        public event viewEventHandler FlushText;

        public void doSomething()
        {
            for (int i = 0; i < 10; i++) 
                FlushText(this, $"i={i}");
        }

        public void goQuery(string sqLiteConnString, string query) {
            IDbConnection conn = new SQLiteConnection(sqLiteConnString);
            conn.Open();
            IDbCommand com = conn.CreateCommand();
            com.CommandText = query;
            IDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                FlushText(this, reader[0] + " " + reader[1]);
            }
            reader.Close();
            conn.Close();
        }

        public void GetCustomerName(string connString, string factory, string id) {
            DbProviderFactories.RegisterFactory("System.Data.SQLite", "System.Data.SQLite.SQLiteFactory, System.Data.SQLite"/*.SQLiteFactory.Instance*/);
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(factory);

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
                        FlushText(this, "nome: " + reader["nome"]);
                    }
                }
            }
        }
    }
}
