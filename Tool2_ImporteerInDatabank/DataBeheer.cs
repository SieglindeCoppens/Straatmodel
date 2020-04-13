using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Data;

namespace Tool2_ImporteerInDatabank
{
    class DataBeheer
    {
        private string connectionString;

        public DataBeheer(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private SqlConnection getConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }

        public void voegStratenToe(List<List<string>> stratenInfo)
        {
            SqlConnection connection = getConnection();
            string query = "INSERT INTO dbo.straat(id, straatnaam, gemeente, provincie, graafId, lengte) VALUES(@id, @straatnaam, @gemeente, @provincie, @graafId, @lengte)";
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                //foreach uit try of in try? 
                
                
                    try
                    {
                        command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@straatnaam", SqlDbType.NVarChar));
                        command.Parameters.Add(new SqlParameter("@gemeente", SqlDbType.NVarChar));
                        command.Parameters.Add(new SqlParameter("@provincie", SqlDbType.NVarChar));
                        command.Parameters.Add(new SqlParameter("@graafId", SqlDbType.NVarChar));
                        command.Parameters.Add(new SqlParameter("@lengte", SqlDbType.NVarChar));
                        command.CommandText = query;
                    for (int i=0;i<stratenInfo[0].Count;i++)
                        {
                            command.Parameters["@id"].Value = int.Parse(stratenInfo[0][i]);
                            command.Parameters["@straatnaam"].Value = stratenInfo[1][i];
                            command.Parameters["@gemeente"].Value = stratenInfo[2][i];
                            command.Parameters["@provincie"].Value = stratenInfo[3][i];
                            command.Parameters["@graafId"].Value = int.Parse(stratenInfo[4][i]);
                            command.Parameters["@lengte"].Value = stratenInfo[5][i];
                            command.ExecuteNonQuery();
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
        public void VoegSegmentenToe(List<List<string>> segmentInfo)
        {
            SqlConnection connection = getConnection();
            string query = "INSERT INTO dbo.segment(id, beginknoop, eindknoop, straatId, puntenlijst) VALUES(@id, @beginknoop, @eindknoop, @straatId, @puntenlijst)";
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                //foreach uit try of in try? 


                try
                {
                    command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@beginknoop", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@eindknoop", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@straatId", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@puntenlijst", SqlDbType.NVarChar));
                    command.CommandText = query;
                    for (int i = 0; i < segmentInfo[0].Count; i++)
                    {
                        
                        command.Parameters["@id"].Value = int.Parse(segmentInfo[0][i]);
                        command.Parameters["@beginknoop"].Value = int.Parse(segmentInfo[1][i]);
                        command.Parameters["@eindknoop"].Value = int.Parse(segmentInfo[2][i]);
                        command.Parameters["@straatId"].Value = int.Parse(segmentInfo[3][i]);
                        command.Parameters["@puntenlijst"].Value = segmentInfo[4][i];
                        command.ExecuteNonQuery();
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
        public void VoegKnopenToe(List<List<string>> knoopInfo)
        {
            SqlConnection connection = getConnection();
            string query = "INSERT INTO dbo.knoop(id, x, y) VALUES(@id, @x, @y)";
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                //foreach uit try of in try? 


                try
                {
                    command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@x", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@y", SqlDbType.NVarChar));
                    command.CommandText = query;
                    for (int i = 0; i < knoopInfo[0].Count; i++)
                    {
                        
                        command.Parameters["@id"].Value = int.Parse(knoopInfo[0][i]);
                        command.Parameters["@x"].Value = knoopInfo[1][i];
                        command.Parameters["@y"].Value = knoopInfo[2][i];

                        command.ExecuteNonQuery();
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
    }
}
