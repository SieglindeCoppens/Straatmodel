using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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

        //Aantal knopen tellen?? IS DIT EFFICIENT??
        //string queryKnopen = "SELECT DISTINCT k.id FROM dbo.straat st INNER JOIN dbo.segment se ON st.id=se.straatId INNER JOIN dbo.knoop k ON se.beginknoop = k.id WHERE st.id=@id UNION SELECT DISTINCT k.id FROM dbo.straat st INNER JOIN dbo.segment se ON st.id = se.straatId INNER JOIN dbo.knoop k ON se.eindknoop = k.id WHERE st.id =@id";
        //Aparte geefknopenlijst? 


        public void GeefStraatinfo(string gemeente, string straatnaam)
        {
            SqlConnection connection = getConnection();
            string query = "SELECT id FROM dbo.straat WHERE straatnaam=@straatnaam AND gemeente=@gemeente";

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.Add(new SqlParameter("@straatnaam", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@gemeente", SqlDbType.NVarChar));
                connection.Open();
                try
                {
                    command.Parameters["@straatnaam"].Value = straatnaam;
                    command.Parameters["@gemeente"].Value = gemeente;
                    SqlDataReader dataReader = command.ExecuteReader();
                    dataReader.Read();
                    int straatId = (int)dataReader["id"];
                    dataReader.Close();
                    GeefStraatinfo(straatId);
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
        public void GeefStraatinfo(int straatID)
        {
            List<string> straatGegevens = GeefStraatStringEnAantalSegmenten(straatID);
            Console.WriteLine($"{straatID}, {straatGegevens[0]}, {straatGegevens[1]}, {straatGegevens[2]}");
            Console.WriteLine(straatGegevens[3]);

            List<List<string>> knopen = GeefKnopenVanStraat(straatID);
            Console.WriteLine("Aantal knopen : " + knopen[0].Count);
            Console.WriteLine("Aantal segmenten : " + straatGegevens[4]);

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
                    for (int i = 0; i < knopen[0].Count; i++)
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

        public List<string> GeefStraatStringEnAantalSegmenten(int straatId)
        {
            SqlConnection connection = getConnection();
            List<string> straatGegevens = new List<string>();


            string query = "SELECT straatnaam, gemeente, provincie, graafId FROM dbo.straat WHERE id=@id";
            string queryAS = "SELECT COUNT(*) FROM dbo.segment WHERE straatId=@id";

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                SqlParameter parId = new SqlParameter();
                parId.ParameterName = "@id";
                parId.SqlDbType = SqlDbType.Int;
                parId.Value = straatId;
                command.Parameters.Add(parId);

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

        public void GeefProvincieoverzicht(string provincie)
        {
            List<string> gemeenten = GeefGemeentenVoorProvincie(provincie);

            using StreamWriter writer = File.CreateText(@$"C:\Users\Sieglinde\Documents\Programmeren\Labo_Straatmodel\{provincie}.txt");

            SqlConnection connection = getConnection();
            string queryAantal = "SELECT COUNT(DISTINCT straatnaam) FROM dbo.straat WHERE provincie=@provincie AND gemeente=@gemeente";
            string queryStraten = "SELECT straatnaam, lengte FROM dbo.straat WHERE provincie=@provincie AND gemeente=@gemeente ORDER BY lengte";

            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                try
                {
                    for (int i = 0; i < gemeenten.Count; i++)
                    {
                        command.CommandText = queryAantal;
                        SqlParameter parGemeente = new SqlParameter();
                        parGemeente.ParameterName = "@gemeente";
                        parGemeente.SqlDbType = SqlDbType.NVarChar;
                        parGemeente.Value = gemeenten[i];
                        command.Parameters.Add(parGemeente);
                        SqlParameter parProv = new SqlParameter();
                        parProv.ParameterName = "@provincie";
                        parProv.SqlDbType = SqlDbType.NVarChar;
                        parProv.Value = provincie;
                        command.Parameters.Add(parProv);

                        int aantalstraten = (int)command.ExecuteScalar();

                        writer.WriteLine($"{gemeenten[i]} : {aantalstraten}");

                        command.CommandText = queryStraten;
                        SqlDataReader dataReader = command.ExecuteReader();

                        while (dataReader.Read())
                        {
                            string straatnaam = (string)dataReader["straatnaam"];
                            float lengte = (float)dataReader["lengte"];
                            writer.WriteLine($"   o   {straatnaam}, {lengte}");
                            command.Parameters.Clear();
                        }
                        dataReader.Close();

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

        //foreach (string gemeente in gemeenten)
        //IN DE CONNECTION? PARAMETER GEWOON AANPASSEN -> MAAR  1 connectie! 

        //string query2 = "SELECT COUNT(DISTINCT gemeente) FROM dbo.straat WHERE provincie=@provincie";
        //string aantalGemeenten = (command.ExecuteScalar().ToString()); //string??

        public List<string> GeefGemeentenVoorProvincie(string provincie)
        {
            List<string> gemeenten = new List<string>();
            SqlConnection connection = getConnection();

            string query1 = "SELECT DISTINCT gemeente FROM dbo.straat WHERE provincie=@provincie";

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query1;
                SqlParameter parProv = new SqlParameter();
                parProv.ParameterName = "@provincie";
                parProv.SqlDbType = SqlDbType.NVarChar;
                parProv.Value = provincie;
                command.Parameters.Add(parProv);

                connection.Open();
                try
                {
                    command.CommandText = query1;
                    SqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        string gemeentenaam = (string)dataReader["gemeente"];
                        gemeenten.Add(gemeentenaam);
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

            gemeenten.Sort();
            return gemeenten;
        }

        public List<string> GeefAangrenzendeStraten(int straatID)
        {
            List<string> aangrenzendeStraten = new List<string>();

            List<List<string>> knopen = GeefKnopenVanStraat(straatID);
            SqlConnection connection = getConnection();

            string query = "SELECT DISTINCT st.straatnaam FROM dbo.straat st INNER JOIN dbo.segment se ON se.straatId=st.id INNER JOIN dbo.knoop k ON se.beginknoop=@knoopId WHERE NOT st.id=@straatId " +
                "UNION " +
                "SELECT DISTINCT st.straatnaam FROM dbo.straat st INNER JOIN dbo.segment se ON se.straatId= st.id INNER JOIN dbo.knoop k ON se.eindknoop=@knoopId WHERE NOT st.id=@straatId";

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                connection.Open();

                try
                {
                    for (int i = 0; i < knopen.Count; i++)
                    {
                        SqlParameter parKnoop = new SqlParameter();
                        parKnoop.ParameterName = "@knoopId";
                        parKnoop.SqlDbType = SqlDbType.Int;
                        parKnoop.Value = knopen[0][i];
                        command.Parameters.Add(parKnoop);
                        SqlParameter parStraatId = new SqlParameter();
                        parStraatId.ParameterName = "@straatId";
                        parStraatId.SqlDbType = SqlDbType.Int;
                        parStraatId.Value = straatID;
                        command.Parameters.Add(parStraatId);

                        SqlDataReader dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                        {
                            string straatnaam = (string)dataReader["straatnaam"];
                            aangrenzendeStraten.Add(straatnaam);
                        }
                        dataReader.Close();
                        command.Parameters.Clear();
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
            return aangrenzendeStraten;
        }
    }
}
//Dit van elke knoop doen!
//eerst knopen zoeken met deze query
//SELECT k.id as kid, k.x, k.y FROM dbo.straat st INNER JOIN dbo.segment se ON st.id= se.straatId INNER JOIN dbo.knoop k on se.beginknoop= k.id WHERE st.id= 70207
//UNION
//SELECT k.id as kid, k.x, k.y FROM dbo.straat st INNER JOIN dbo.segment se ON st.id= se.straatId INNER JOIN dbo.knoop k on se.eindknoop= k.id WHERE st.id= 70207;


//SELECT DISTINCT st.straatnaam FROM dbo.straat st INNER JOIN dbo.segment se ON se.straatId=st.id INNER JOIN dbo.knoop k ON se.beginknoop= 1356590
//UNION
//SELECT DISTINCT st.straatnaam FROM dbo.straat st INNER JOIN dbo.segment se ON se.straatId= st.id INNER JOIN dbo.knoop k ON se.eindknoop= 1356590
//;
