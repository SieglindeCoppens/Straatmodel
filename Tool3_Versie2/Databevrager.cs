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

        public Straat MaakStraat(string straatnaam, string gemeentenaam)
        {
            Straat straat = null;
            int straatId = 0;
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
                    command.Parameters["@gemeente"].Value = gemeentenaam;
                    SqlDataReader dataReader = command.ExecuteReader();
                    dataReader.Read();
                    straatId = (int)dataReader["id"];
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
            return MaakStraat(straatId);
        }

        public Straat MaakStraat(int straatId)
        {
            Straat straat = null;
            SqlConnection connection = getConnection();
            SqlConnection connection2 = getConnection();        //om meerdere readers tegelijk te kunnen uitvoeren! 

            string queryStraat = "Select * FROM dbo.straat WHERE id=@straatId";
            string querySegment = "SELECT * FROM dbo.segment WHERE straatId=@straatId";
            string queryBeginknoop = "SELECT * FROM dbo.knoop WHERE id=@beginknoopId";
            string queryEindknoop = "SELECT * FROM dbo.knoop WHERE id=@eindknoopId";
            string queryPunt = "SELECT * FROM dbo.punt WHERE segmentId=@segmentId";

            using (SqlCommand command1 = connection.CreateCommand())
            using (SqlCommand command2 = connection.CreateCommand())
            using (SqlCommand command3 = connection2.CreateCommand())
            using (SqlCommand command4 = connection2.CreateCommand())
            using (SqlCommand command5 = connection2.CreateCommand())
            {
                connection.Open();
                connection2.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                command1.Transaction = transaction;
                command2.Transaction = transaction;
                //
                SqlTransaction transaction2 = connection2.BeginTransaction();
                command3.Transaction = transaction2;
                command4.Transaction = transaction2;
                command5.Transaction = transaction2;
                try
                {
                    //Straat maken
                    command1.Parameters.Add(new SqlParameter("@straatId", SqlDbType.Int));
                    
                    command1.CommandText = queryStraat;
                    command1.Parameters["@straatId"].Value = straatId;
                    SqlDataReader dataReader = command1.ExecuteReader();
                    dataReader.Read();
                    string straatnaam = (string)dataReader["straatnaam"];
                    string gemeente = (string)dataReader["gemeente"];
                    string provincie = (string)dataReader["provincie"];
                    int graafId = (int)dataReader["graafId"];
                    float lengte = (float)dataReader["lengte"];
                    dataReader.Close();

                    //Segmenten
                    List<Segment> segmenten = new List<Segment>();

                    command2.Parameters.Add(new SqlParameter("@straatId", SqlDbType.Int));
                    command2.CommandText = querySegment;
                    command2.Parameters["@straatId"].Value = straatId;
                    dataReader = command2.ExecuteReader();
                    while (dataReader.Read())
                    {

                        int segmentId = (int)dataReader["id"];
                        int beginknoopId = (int)dataReader["beginknoop"];
                        int eindknoopId = (int)dataReader["eindknoop"];

                        //beginknoop
                        command3.Parameters.Add(new SqlParameter("@beginknoopId", SqlDbType.Int));
                        command3.CommandText = queryBeginknoop;
                        command3.Parameters["@beginknoopId"].Value = beginknoopId;
                        SqlDataReader reader = command3.ExecuteReader();
                        reader.Read();
                        string bx = (string)reader["x"];
                        string by = (string)reader["y"];
                        Punt beginknooppunt = new Punt(double.Parse(bx), double.Parse(by));
                        Knoop beginknoop = new Knoop(beginknoopId, beginknooppunt);
                        reader.Close();
                        command3.Parameters.Clear();

                        //eindknoop
                        command4.Parameters.Add(new SqlParameter("@eindknoopId", SqlDbType.Int));
                        command4.CommandText = queryEindknoop;
                        command4.Parameters["@eindknoopId"].Value = eindknoopId;
                        reader = command4.ExecuteReader();
                        reader.Read();
                        string ex = (string)reader["x"];
                        string ey = (string)reader["y"];
                        Punt eindknooppunt = new Punt(double.Parse(ex), double.Parse(ey));
                        Knoop eindknoop = new Knoop(eindknoopId, eindknooppunt);
                        reader.Close();
                        command4.Parameters.Clear();

                        //punten
                        List<Punt> punten = new List<Punt>();

                        command5.Parameters.Add(new SqlParameter("@segmentId", SqlDbType.Int));
                        command5.CommandText = queryPunt;
                        command5.Parameters["@segmentId"].Value = segmentId;
                        reader = command5.ExecuteReader();
                        while (reader.Read())
                        {
                            Punt punt = new Punt(double.Parse((string)reader["x"]), double.Parse((string)reader["y"]));
                            punten.Add(punt);
                        }
                        reader.Close();
                        command5.Parameters.Clear();

                        Segment segment = new Segment(segmentId, beginknoop, eindknoop, punten);
                        segmenten.Add(segment);
                    }
                    dataReader.Close();

                    Graaf graaf = new Graaf(segmenten, graafId);
                    straat = new Straat(straatId, straatnaam, graaf, provincie, gemeente, lengte);

                    return straat;
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
            return straat;

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

            string query = "SELECT st.straatnaam FROM dbo.straat st INNER JOIN dbo.segment se ON se.straatId=st.id INNER JOIN dbo.knoop k ON se.beginknoop=@knoopId WHERE NOT st.id=@straatId " +
                "UNION " +
                "SELECT st.straatnaam FROM dbo.straat st INNER JOIN dbo.segment se ON se.straatId= st.id INNER JOIN dbo.knoop k ON se.eindknoop=@knoopId WHERE NOT st.id=@straatId";

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                connection.Open();

                try
                {
                    for (int i = 0; i < knopen[0].Count; i++)
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
                            if (!aangrenzendeStraten.Contains(straatnaam))
                            {
                                aangrenzendeStraten.Add(straatnaam);
                            }
                                 
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
    }
}
