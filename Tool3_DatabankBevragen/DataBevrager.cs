using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


//Voor deze tool heb ik bij de security eigenschappen van de sql server op 'SQL Server and Windows Authentication mode' moeten zetten 
namespace Tool3_DatabankBevragen
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



        //Aparte geefknopenlijst? 
        public void GeefStraatinfo(int straatID)
        {
            List<int> knopen = GeefKnopenVanStraat(straatID);


        }
        private List<int> GeefKnopenVanStraat(int straatID)
        {
            SqlConnection connection = getConnection();
            List<int> knoopIDs = new List<int>();

            //aliassen schrijven om later de juiste id te kunnen opvragen! 
            string queryBeginknopen = "SELECT k.id as kid FROM dbo.straat st INNER JOIN dbo.segment se ON st.id=se.straatId INNER JOIN dbo.knoop k on se.beginknoop=k.id WHERE st.id=@id";
            string queryEindknopen = "SELECT k.id as kid FROM dbo.straat st INNER JOIN dbo.segment se ON st.id=se.straatId INNER JOIN dbo.knoop k on se.eindknoop=k.id WHERE st.id=@id";


            using (SqlCommand command1 = connection.CreateCommand())
            using (SqlCommand command2 = connection.CreateCommand())
            {
                command1.CommandText = queryBeginknopen;
                command2.CommandText = queryEindknopen;
                SqlParameter parId = new SqlParameter();
                parId.ParameterName = "@id";
                parId.SqlDbType = SqlDbType.Int;
                parId.Value = straatID;
                command1.Parameters.Add(parId);
                SqlParameter parId2 = new SqlParameter();
                parId2.ParameterName = "@id";
                parId2.SqlDbType = SqlDbType.Int;
                parId2.Value = straatID;
                command2.Parameters.Add(parId2);

                connection.Open();
                try
                {
                    SqlDataReader dataReader = command1.ExecuteReader();
                    while (dataReader.Read())
                    {
                        int knoopID = (int)dataReader["kid"];
                        knoopIDs.Add(knoopID);
                    }
                    dataReader.Close();
                    SqlDataReader dataReader2 = command2.ExecuteReader();
                    while (dataReader2.Read())
                    {
                        int knoopID = (int)dataReader2["kid"];
                        knoopIDs.Add(knoopID);
                    }
                    dataReader2.Close();
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
            IEnumerable<int> distinctKnoopIDs = knoopIDs.Distinct();
            return distinctKnoopIDs.ToList();
        }
    }
}
