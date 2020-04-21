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


        public Straat MaakStraat(int straatId)
        {
            Straat straat = null;
            SqlConnection connection = getConnection();
            SqlConnection connection2 = getConnection();        //om meerdere readers tegelijk te kunnen uitvoeren! 

            string queryStraat = "Select * FROM dbo.straat WHERE straatId=@straatId";
            string querySegment = "SELECT * FROM dbo.segment WHERE straatId=@straatId";
            string queryBeginknoop = "SELECT * FROM dbo.knoop WHERE knoopId=@beginknoopId";
            string queryEindknoop = "SELECT * FROM dbo.knoop WHERE knoopId=@eindknoopId";
            string queryPunt = "SELECT * FROM dbo.punt WHERE segmentId=@segmentId";

            using (SqlCommand command1 = connection.CreateCommand())
            using (SqlCommand command2 = connection.CreateCommand())
            using (SqlCommand command3 = connection2.CreateCommand())
            using (SqlCommand command4 = connection2.CreateCommand())
            using (SqlCommand command5 = connection2.CreateCommand())
            {
                connection.Open();
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
                    double lengte = (double)dataReader["lengte"];
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
                        double bx = (double)reader["x"];
                        double by = (double)reader["y"];
                        Punt beginknooppunt = new Punt(bx, by);
                        Knoop beginknoop = new Knoop(beginknoopId, beginknooppunt);
                        reader.Close();

                        //eindknoop
                        command4.Parameters.Add(new SqlParameter("@eindknoopId", SqlDbType.Int));
                        command4.CommandText = queryEindknoop;
                        command4.Parameters["@eindknoopId"].Value = eindknoopId;
                        reader = command4.ExecuteReader();
                        reader.Read();
                        double ex = (double)reader["x"];
                        double ey = (double)reader["y"];
                        Punt eindknooppunt = new Punt(ex, ey);
                        Knoop eindknoop = new Knoop(eindknoopId, eindknooppunt);
                        reader.Close();

                        //punten
                        List<Punt> punten = new List<Punt>();

                        command5.Parameters.Add(new SqlParameter("@segmentId", SqlDbType.Int));
                        command5.CommandText = queryPunt;
                        command5.Parameters["@segmentId"].Value = segmentId;
                        reader = command5.ExecuteReader();
                        while (reader.Read())
                        {
                            Punt punt = new Punt((double)reader["x"], (double)reader["y"]);
                            punten.Add(punt);
                        }
                        reader.Close();

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
    }
}
