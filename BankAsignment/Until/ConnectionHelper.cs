using MySql.Data.MySqlClient;

namespace BankAsignment.Until
{
    public class ConnectionHelper
    {
        private static readonly string DATA_SERVER = "localhost";
        private static readonly string DATA_PORT = "3306";
        private static readonly string DATABASE_NAME = "t2012e-bank";
        private static readonly string DATABASE_USER = "root";
        private static readonly string DATABASE_PWD = "";
        private static readonly string DATABASE_MODE = "none";
        private static MySqlConnection cnn;

        public static MySqlConnection getConnection() {
            if (cnn == null)
            {
                cnn = new MySqlConnection($"server={DATA_SERVER};" +
                                          $"port={DATA_PORT};" +
                                          $"uid={DATABASE_USER};" +
                                          $"password={DATABASE_PWD};" +
                                          $"database={DATABASE_NAME};" +
                                          $"SslMode={DATABASE_MODE};");
            }
            return cnn;
        }
    }
}