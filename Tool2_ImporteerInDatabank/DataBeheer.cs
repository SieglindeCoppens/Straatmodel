using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

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
            string query = "INSERT INTO"


        }
    }
}
