using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace Yelp_Dataset_Challenge
{
    class MySQLConnect
    {
        // used to open the connection to the DB
        private MySqlConnection connection;

        // database credentials
        string serv = "localhost";
        string db = "yelpdata";
        string uid = "user";
        string pass = "pass";

        public MySQLConnect()
        {
            try
            {
                Initialize();
            }
            catch (MySqlException Exception)
            {

            }
        }

        private void Initialize()
        {
            // build the connection string
            string conString = "SERVER=" + serv + ";DATABASE=" + db + ";UID=" + uid + ";PASSWORD=" + pass + ";";
            connection = new MySqlConnection(conString);
        }

        private bool openConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {

            }
            return false;
        }

        private bool closeConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {

            }
            return false;
        }

        public List<string> sqlSelect (string queryStr)
        {
            List<string> qResult = new List<string>();

            if (this.openConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(queryStr, connection);
                MySqlDataReader dRead = cmd.ExecuteReader();

                while (dRead.Read())
                {
                    string temp = "";

                    for (int i = 0; i < dRead.FieldCount; i++)
                    {
                        temp += dRead.GetString(i).ToString() + " ";
                    }
                    qResult.Add(temp);
                }
                dRead.Close();
                this.closeConnection();
            }
            return qResult;
        }

        public void sqlUpdate (string queryStr)
        {
            if (this.openConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(queryStr, connection);
                try
                {
                    cmd.ExecuteNonQuery();
                    
                }
                catch (Exception Ex) { }
                finally
                {
                    this.closeConnection();
                }

            }
        }

        public DataTable dTable (string queryStr)
        {
            DataTable dt = new DataTable();

            if (this.openConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(queryStr, connection);
                MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                sda.Fill(dt);
                this.closeConnection();
            }
            return dt;
        }
    }
}