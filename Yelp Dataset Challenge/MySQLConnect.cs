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

        /// <summary>
        /// entrance to mysqlconnector, attempts to connect to DB as a user
        /// 
        /// Created July 11th, 2015 : David Fletcher
        /// </summary>
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

        /// <summary>
        /// Initialize the database
        /// 
        /// Created July 11th, 2015 : David Fletcher
        /// </summary>
        private void Initialize()
        {
            // build the connection string
            string conString = "SERVER=" + serv + ";DATABASE=" + db + ";UID=" + uid + ";PASSWORD=" + pass + ";";
            connection = new MySqlConnection(conString);
        }

        /// <summary>
        /// open the connection
        /// 
        /// Created July 11th, 2015 : David Fletcher
        /// </summary>
        /// <returns>boolean of open : true or not open : false</returns>
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

        /// <summary>
        /// close the connection
        /// 
        /// Created July 11th, 2015 : David Fletcher
        /// </summary>
        /// <returns>boolean of close : true or not close : false</returns>
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

        /// <summary>
        /// A sql select statement that runs a query string on the database
        /// and returns a list of strings as the result
        /// 
        /// Created July 11th, 2015 : David Fletcher
        /// </summary>
        /// <param name="queryStr">the query string</param>
        /// <returns>list of strings of the query</returns>
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

        /// <summary>
        /// Executes non query strings on the database
        /// 
        /// Created July 11th, 2015 : David Fletcher
        /// </summary>
        /// <param name="queryStr">query string</param>
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

        /// <summary>
        /// executes a query string that fills a datatable for 
        /// queries that return more than one item per entry
        /// 
        /// Created July 11th, 2015 : David Fletcher
        /// </summary>
        /// <param name="queryStr">input query string</param>
        /// <returns>datatable containing query results</returns>
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