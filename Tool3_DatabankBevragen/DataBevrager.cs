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

        //Probeersel: welke van de twee is sneller? 
        public void GeefStraatInfo2(int straatID)
        {
            string query = "SELECT st.straatnaam, st.id, st.gemeente, st.provincie, st.graafId, k.id, k.x, k.y, se.id, se.beginknoop, se.eindknoop, p.id, p.x, p.y  " +
                "FROM dbo.straat st INNER JOIN dbo.segment se ON st.id = se.straatId " +
                "INNER JOIN dbo.knoop k ON se.beginknoop = k.id " +
                "LEFT JOIN dbo.punt p ON p.segmentId = se.id " +
                "WHERE st.id = @id" +
                "UNION " +
                "SELECT st.straatnaam, st.id, st.gemeente, st.provincie, st.graafId, k.id, k.x, k.y, se.id, se.beginknoop, se.eindknoop, p.id, p.x, p.y " +
                "FROM dbo.straat st INNER JOIN dbo.segment se ON st.id = se.straatId " +
                "INNER JOIN dbo.knoop k ON se.eindknoop = k.id " +
                "LEFT JOIN dbo.punt p ON p.segmentId = se.id " +
                "WHERE st.id = @id"; 
        }


        //Aantal knopen tellen?? IS DIT EFFICIENT??
        //string queryKnopen = "SELECT DISTINCT k.id FROM dbo.straat st INNER JOIN dbo.segment se ON st.id=se.straatId INNER JOIN dbo.knoop k ON se.beginknoop = k.id WHERE st.id=@id UNION SELECT DISTINCT k.id FROM dbo.straat st INNER JOIN dbo.segment se ON st.id = se.straatId INNER JOIN dbo.knoop k ON se.eindknoop = k.id WHERE st.id =@id";
        //Aparte geefknopenlijst? 
        public void GeefStraatinfo(int straatID)
        {
            List<string> straatGegevens = GeefStraatStringEnAantalSegmenten(straatID);
            Console.WriteLine($"{straatID}, {straatGegevens[0]}, {straatGegevens[1]}, {straatGegevens[2]}");
            Console.WriteLine(straatGegevens[3]);

            List<List<string>> knopen = GeefKnopenVanStraat(straatID);
            Console.WriteLine("Aantal knopen : "+ knopen[0].Count);
            Console.WriteLine("Aantal segmenten : "+straatGegevens[4]);

            //2 connecties naar de databank om de twee readers te kunnen uitvoeren??
            SqlConnection connection = getConnection();
            SqlConnection connection2 = getConnection();

            string querySegmenten = "SELECT se.id, se.beginknoop, se.eindknoop FROM dbo.knoop k INNER JOIN dbo.segment se ON se.beginknoop=k.id WHERE k.id=@id AND se.straatId=@straatID " +
                "UNION " +
                "SELECT se.id, se.beginknoop, se.eindknoop FROM dbo.knoop k INNER JOIN dbo.segment se ON se.eindknoop = k.id WHERE k.id = @id AND se.straatId=@straatID";

            string queryPunten = "SELECT p.x, p.y FROM dbo.segment se INNER JOIN dbo.punt p ON p.segmentId=se.id WHERE se.id=@segmentId";

            using (SqlCommand command = connection.CreateCommand())
            using (SqlCommand command2 = connection2.CreateCommand())
            {
                command.CommandText = querySegmenten;
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@straatID", SqlDbType.Int));
                connection.Open();
                try
                {
                    for(int i=0;i < knopen[0].Count;i++)
                    {
                        Console.WriteLine($"Knoop[{knopen[0][i]},[{knopen[1][i]},{knopen[2][i]}]]");
                        command.Parameters["@id"].Value = knopen[0][i];
                        command.Parameters["@straatID"].Value = straatID;
                        SqlDataReader reader = command.ExecuteReader();
                        

                        while (reader.Read()) //overloopt dus de segmenten
                        {
                            int segmentId = (int)reader["id"];
                            int beginknoop = (int)reader["beginknoop"];
                            int eindknoop = (int)reader["eindknoop"];
                            Console.WriteLine($"     [segment:{segmentId},begin:{beginknoop},eind:{eindknoop}]");
                            int indexb = knopen[0].IndexOf(beginknoop.ToString());
                            Console.WriteLine($"            ({knopen[1][indexb]},{knopen[2][indexb]})");

                            connection2.Open();

                            command2.Parameters.Add(new SqlParameter("@segmentId", SqlDbType.Int));
                            command2.CommandText = queryPunten;

                            command2.Parameters["@segmentId"].Value = segmentId;

                            SqlDataReader puntReader = command2.ExecuteReader();

                            while (puntReader.Read()) //overloopt de punten
                            {
                                string x = (string)puntReader["x"];
                                string y = (string)puntReader["y"];
                                Console.WriteLine($"            ({x},{y})");
                            }

                            command2.Parameters.Clear();
                            puntReader.Close();
                            connection2.Close();
                            //eindknoop coordinaten nog vinden! 
                            int indexe = knopen[0].IndexOf(eindknoop.ToString());
                            Console.WriteLine($"            ({knopen[1][indexe]},{knopen[2][indexe]})");
                        }
                        reader.Close();
                    }

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
        }
        public List<List<string>> GeefKnopenVanStraat(int straatID)
        {
            SqlConnection connection = getConnection();
            List<string> knoopIDs = new List<string>();
            List<string> xs = new List<string>();
            List<string> ys = new List<string>();

            //aliassen schrijven om later de juiste id te kunnen opvragen! 
            //UNION voert ook een distinct uit op zijn resultaten!! 
            string queryKnopen = "SELECT k.id as kid, k.x, k.y FROM dbo.straat st INNER JOIN dbo.segment se ON st.id=se.straatId INNER JOIN dbo.knoop k on se.beginknoop=k.id WHERE st.id=@id " +
                "UNION " +
                "SELECT k.id as kid, k.x, k.y FROM dbo.straat st INNER JOIN dbo.segment se ON st.id=se.straatId INNER JOIN dbo.knoop k on se.eindknoop=k.id WHERE st.id=@id";

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = queryKnopen;
                SqlParameter parId = new SqlParameter();
                parId.ParameterName = "@id";
                parId.SqlDbType = SqlDbType.Int;
                parId.Value = straatID;
                command.Parameters.Add(parId);

                connection.Open();
                try
                {
                    SqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        int knoopID = (int)dataReader["kid"];
                        string x = (string)dataReader["x"];
                        string y = (string)dataReader["y"];

                        knoopIDs.Add(knoopID.ToString());
                        xs.Add(x);
                        ys.Add(y);

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
            //IEnumerable<int> distinctKnoopIDs = knoopIDs.Distinct();
            List<List<string>> knopen = new List<List<string>>();
            knopen.Add(knoopIDs);
            knopen.Add(xs);
            knopen.Add(ys);

            return knopen;
        }

        public List<string> GeefStraatnamenVanGemeente(string gemeente)
        {
            SqlConnection connection = getConnection();
            List<string> straatnamen = new List<string>();

            string query = "SELECT straatnaam FROM dbo.straat WHERE gemeente=@gemeente";

            using(SqlCommand command = connection.CreateCommand())
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

        //public List<int> GeefSegmentenVanKnoop(int knoopId)
        //{
        //    SqlConnection connection = getConnection();
        //    List<int> segmentIds = new List<int>();

        //    string query = "SELECT se.id AS seid  FROM dbo.knoop k INNER JOIN dbo.segment se ON se.beginknoop=k.id WHERE k.id=@kid " +
        //        "UNION " +
        //        "SELECT se.id AS seid FROM dbo.knoop k INNER JOIN dbo.segment se ON se.eindknoop = k.id WHERE k.id =@kid";

        //    using (SqlCommand command = connection.CreateCommand())
        //    {
        //        command.CommandText = query;
        //        SqlParameter parKid = new SqlParameter();
        //        parKid.ParameterName = "@kid";
        //        parKid.SqlDbType = SqlDbType.Int;
        //        parKid.Value = knoopId;
        //        command.Parameters.Add(parKid);

        //        connection.Open();
        //        try
        //        {
        //            SqlDataReader dataReader = command.ExecuteReader();
        //            while (dataReader.Read())
        //            {
        //                int segmentId = (int)dataReader["kid"];
        //                segmentIds.Add(segmentId);
        //            }
        //            dataReader.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex);
        //        }
        //        finally
        //        {
        //            connection.Close();
        //        }
        //    }
        //    return segmentIds;

        //}

        public List<string> GeefStraatStringEnAantalSegmenten(int straatId)
        {
            SqlConnection connection = getConnection();
            List<string> straatGegevens = new List<string>();


            string query = "SELECT straatnaam, gemeente, provincie, graafId FROM dbo.straat WHERE id=@id";
            string queryAS = "SELECT COUNT(*) FROM dbo.segment WHERE straatId=@id";

            using (SqlCommand command = connection.CreateCommand())
            //using (SqlCommand command2 = connection.CreateCommand())
            {
                command.CommandText = query;
                SqlParameter parId = new SqlParameter();
                parId.ParameterName = "@id";
                parId.SqlDbType = SqlDbType.Int;
                parId.Value = straatId;
                command.Parameters.Add(parId);
                //command2.CommandText = queryAS;
                //SqlParameter parId2 = new SqlParameter();
                //parId2.ParameterName = "@id";
                //parId2.SqlDbType = SqlDbType.Int;
                //parId2.Value = straatId;
                //command2.Parameters.Add(parId2);

                connection.Open();
                try
                {
                    SqlDataReader dataReader = command.ExecuteReader();
                    dataReader.Read();
                    string straatnaam = (string)dataReader["straatnaam"];
                    string gemeente = (string)dataReader["gemeente"];
                    string provincie = (string)dataReader["provincie"];
                    int graafId = (int)dataReader["graafId"];
                    dataReader.Close();

                    command.CommandText = queryAS;
                    int aantalSegmenten = (int)command.ExecuteScalar();

                    straatGegevens.Add(straatnaam);
                    straatGegevens.Add(gemeente);
                    straatGegevens.Add(provincie);
                    straatGegevens.Add(graafId.ToString());
                    straatGegevens.Add(aantalSegmenten.ToString());
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
            return straatGegevens;
        }

        //private GeefAantalWegsegmenten(int straatId)
        //{
        //    SqlConnection connection = getConnection();


        //}
    }
}
