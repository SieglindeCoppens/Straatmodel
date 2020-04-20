using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace Tool3_Versie2
{
    class DataBevrager
    {
        private string connectionString;

        public DataBevrager(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private SqlConnection getConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }

        //bijhouden! 
        public IEnumerable<int> GeefStraatIDs(string gemeente)
        {
            IList<int> straatIDs = new List<int>();

            SqlConnection connection = getConnection();
            string query = "Select * FROM dbo.straat WHERE gemeente = @gemeente";
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                SqlParameter paramId = new SqlParameter();
                paramId.ParameterName = "@gemeente";
                paramId.DbType = DbType.String;
                paramId.Value = gemeente;
                command.Parameters.Add(paramId);
                connection.Open();
                try
                {
                    SqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        int straatID = (int)dataReader["id"];
                        straatIDs.Add(straatID);
                    }
                    dataReader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
                finally
                {
                    connection.Close();
                }
            }
            return straatIDs;
        }

        public List<string> GeefStraatnamenVanGemeente(string gemeente)
        {
            SqlConnection connection = getConnection();
            List<string> straatnamen = new List<string>();

            string query = "SELECT straatnaam FROM dbo.straat WHERE gemeente=@gemeente";

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                SqlParameter paramGemeente = new SqlParameter();
                paramGemeente.ParameterName = "@gemeente";
                paramGemeente.DbType = DbType.String;
                paramGemeente.Value = gemeente;
                command.Parameters.Add(paramGemeente);
                connection.Open();

                try
                {
                    SqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        string straatnaam = (string)dataReader["straatnaam"];
                        straatnamen.Add(straatnaam);
                    }
                    dataReader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    connection.Close();
                }
            }
            straatnamen.Sort();
            return straatnamen;
        }
        //public List<Punt> MaakPunten()
        //{
        //    List<Punt> punten = new List<Punt>();
        //    SqlConnection connection = getConnection();
        //    string query = "Select * FROM dbo.punt WHERE gemeente = @gemeente";
        //}


        public List<Straat> MaakStraat(int straatId)
        {
            List<Straat> straten = new List<Straat>();
            SqlConnection connection = getConnection();
            string queryStraat = "Select * FROM dbo.straat WHERE straatId=@straatId";
            string querySegment = "SELECT * FROM dbo.segment WHERE straatId=@straatId";
            string queryBeginknoop = "SELECT * FROM dbo.knoop WHERE knoopId=@beginknoopId";
            string queryEindknoop = "SELECT * FROM dbo.knoop WHERE knoopId=@eindknoopId";
            string queryPunt = "SELECT * FROM dbo.punt WHERE segmentId=@segmentId";

            using (SqlCommand command1 = connection.CreateCommand())
            using (SqlCommand command2 = connection.CreateCommand())
            using (SqlCommand command3 = connection.CreateCommand())
            using (SqlCommand command4 = connection.CreateCommand())
            using (SqlCommand command5 = connection.CreateCommand())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                command1.Transaction = transaction;
                command2.Transaction = transaction;
                command3.Transaction = transaction;
                command4.Transaction = transaction;
                command5.Transaction = transaction;
                try
                {
                    //Straat maken
                    command1.Parameters.Add(new SqlParameter("@straatId", SqlDbType.Int));
                    
                    command1.CommandText = queryStraat;
                    command1.Parameters["@straatId"].Value = straatId;
                    SqlDataReader dataReader = command1.ExecuteReader();
                    string straatnaam = (string)dataReader["straatnaam"];
                    string gemeente = (string)dataReader["gemeente"];
                    string provincie = (string)dataReader["provincie"];
                    int graafId = (int)dataReader["graafId"];
                    double lengte = (double)dataReader["lengte"];

                    //
                    command2.Parameters.Add(new SqlParameter("@straatId", SqlDbType.Int));
                    command2.CommandText = 

                    }





                    SqlParameter parStraatId = new SqlParameter();
                    parStraatId.ParameterName = "@straatId"
                }

            }

        }
    }
}
