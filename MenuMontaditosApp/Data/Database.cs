using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuMontaditosApp.Data
{
    class Database
    {
        private string connectionString = "server=localhost;database=MenuBarApp;user=root;password=root;";
        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
