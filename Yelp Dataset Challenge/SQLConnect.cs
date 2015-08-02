using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace Yelp_Dataset_Challenge
{
    class SQLConnect
    {
        // used to open the connection to the DB
        private SqlConnection connection;

        // database credentials
        string serv = "localhost";
        string db = "yelp";
        string intSecurity = "true";

        /// <summary>
        /// entrance to sqlconnector, attempts to connect to DB as a user
        /// 
        /// Created July 11th, 2015 : David Fletcher
        /// Updated July 22nd, 2015 : David Fletcher
        ///     - Converted to SQL
        /// </summary>
        public SQLConnect()
        {
            try
            {
                Initialize();
            }
            catch (SqlException Exception)
            {

            }
        }

        /// <summary>
        /// Initialize the database
        /// 
        /// Created July 11th, 2015 : David Fletcher
        /// Updated July 22nd, 2015 : David Fletcher
        ///     - Converted to SQL
        /// </summary>
        private void Initialize()
        {
            // build the connection string
            string conString = "SERVER=" + serv + ";DATABASE=" + db + ";INTEGRATED SECURITY = " + intSecurity + ";";
            connection = new SqlConnection(conString);
        }

        /// <summary>
        /// close the connection
        /// 
        /// Created July 11th, 2015 : David Fletcher
        /// Updated July 22nd, 2015 : David Fletcher
        ///     - Converted to SQL
        /// </summary>
        /// <returns>boolean of close : true or not close : false</returns>
        private bool openConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (SqlException ex)
            {

            }
            return false;
        }

        /// <summary>
        /// A sql select statement that runs a query string on the database
        /// and returns a list of strings as the result
        /// 
        /// Created July 11th, 2015 : David Fletcher
        /// Updated July 22nd, 2015 : David Fletcher
        ///     - Converted to SQL
        /// </summary>
        /// <param name="queryStr">the query string</param>
        /// <returns>list of strings of the query</returns>
        private bool closeConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (SqlException ex)
            {

            }
            return false;
        }

        /// <summary>
        /// A sql select statement that runs a query string on the database
        /// and returns a list of strings as the result
        /// 
        /// Created July 11th, 2015 : David Fletcher
        /// Updated July 22nd, 2015 : David Fletcher
        ///     - Converted to SQL
        /// Updated August 2nd, 2015 : David Fletcher
        ///     - Added a seperation token
        /// </summary>
        /// <param name="queryStr">the query string</param>
        /// <returns>list of strings of the query</returns>
        public List<string> sqlSelect(string queryStr, bool sepToken)
        {
            List<string> qResult = new List<string>();

            if (this.openConnection() == true)
            {
                SqlCommand cmd = new SqlCommand(queryStr, connection);
                SqlDataReader dRead = cmd.ExecuteReader();

                while (dRead.Read())
                {
                    string temp = "";

                    for (int i = 0; i < dRead.FieldCount; i++)
                    {
                        if (sepToken == false)
                        {
                            temp += dRead.GetString(i).ToString() + " ";
                        }
                        else
                        {
                            temp += dRead.GetString(i).ToString() + ";";
                        }
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
        /// Updated July 22nd, 2015 : David Fletcher
        ///     - Converted to SQL
        /// </summary>
        /// <param name="queryStr">query string</param>
        public void sqlUpdate(string queryStr)
        {
            if (this.openConnection() == true)
            {
                SqlCommand cmd = new SqlCommand(queryStr, connection);
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
        /// Updated July 22nd, 2015 : David Fletcher
        ///     - Converted to SQL
        /// </summary>
        /// <param name="queryStr">input query string</param>
        /// <returns>datatable containing query results</returns>
        public DataTable dTable(string queryStr)
        {
            DataTable dt = new DataTable();

            if (this.openConnection() == true)
            {
                SqlCommand cmd = new SqlCommand(queryStr, connection);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                this.closeConnection();
            }
            return dt;
        }
    }
}
