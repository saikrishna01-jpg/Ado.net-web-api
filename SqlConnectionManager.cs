using System.Data.SqlClient;

namespace WebApplication1
{
    public class SqlConnectionManager
    {
        public static readonly string ConnectionString = "Data Source=LAPTOP-P0HHU1HQ;Initial Catalog=test;Integrated Security=True";
        public static SqlConnection _connection;

        private SqlConnectionManager() { }

        public static SqlConnection GetConnection()
        {
            if(_connection == null)
            {
                _connection = new SqlConnection(ConnectionString);
                _connection.Open();
            }
            else if(_connection.State!=System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }
            return _connection;
        }

        public static void CloseConnection()
        {
            if(_connection!=null && _connection.State ==  System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }
}
