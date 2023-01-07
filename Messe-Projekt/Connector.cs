using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
//using MySql.Data.MySqlClient;
using System.IO;
using System.Diagnostics;
using MySqlConnector;

namespace Messe_Projekt
{
    internal class Connector
    {
        private static MySqlConnection conn;

        public string Server { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }
        public string Name { get; set; }
        public bool IsConnected { get; set; } = false;

        public bool connect(string server = "localhost", string database = "personal", string user = "testUser", string password = "sicher", string port = "3306")
        {
            Server = server;
            Database = database;
            User = user;
            Password = password;
            Port = port;

            string connectionString = $"server={Server}; port={Port}; user id={User}; password={Password}; database={Database}";

            conn = new MySqlConnection(connectionString);
            try
            {
                conn.Open();
                IsConnected = true;
                return true;
            }
            catch
            {
                IsConnected = false;
                return false;
            }

        }
        public bool disconnect()
        {
            if (IsConnected)
            {
                try
                {
                    conn.Close();
                    IsConnected = false;
                    return true;
                }
                catch (Exception e)
                {
                    IsConnected = false;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public List<string> GetColumns()
        {
            string query = $"select COLUMN_NAME from information_schema.columns where table_schema = '{this.Database}' order by table_name,ordinal_position";

            List<string> columns = new List<string>();

            MySqlCommand command = new MySqlCommand(query, conn);
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                columns.Add(reader["COLUMN_NAME"] + "");
            }

            reader.Close();
            return columns;
        }

        public List<string>[] Select(object table)
        {
            var _table = table.ToString();
            string query = $"SELECT * FROM `besucherdaten` {_table}";

            List<string> cols = new List<string>();
            cols = GetColumns();

            //Create Command
            MySqlCommand cmd = new MySqlCommand(query, conn);
            //Create a data reader and Execute the command
            MySqlDataReader dataReader = cmd.ExecuteReader();

            List<string>[] list = new List<string>[cols.Count];
            for (int i = 0; i < list.Length; i++)
            {
                list[i] = new List<string>();
            }

            while (dataReader.Read())
            {
                int j = 0;
                foreach (string col in cols)
                {
                    list[j++].Add(dataReader[col] + "");
                }
            }
            
            dataReader.Close();
            return list;
        }

        public void Insert(string value1, string value2, string value3, double value4, double value5, string value6, bool value7, bool value8, bool value9)
        {
            string query = $"INSERT INTO `besucherdaten` (`vorname`, `nachname`, `straße`, `hausnr`, `plz`, `ort`, `interest1`, `interest2`, `interest3`) VALUES ('{value1}', '{value2}', '{value3}', '{value4}', '{value5}', '{value6}', {value7}, {value8}, {value9});";

            //create command and assign the query and connection from the constructor
            MySqlCommand cmd = new MySqlCommand(query, conn);

            //Execute command
            cmd.ExecuteNonQuery();
        }
    }
}