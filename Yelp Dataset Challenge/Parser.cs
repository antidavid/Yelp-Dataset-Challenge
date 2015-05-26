using System;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Json;

namespace Yelp_Dataset_Challenge
{
    class Parser
    {
        private int maxLength = 10000;

        /// <summary>
        /// cleans non alphanumeric characters from the sql string
        /// 
        /// Created May 23, 2015 - David Fletcher
        /// Updated May 25, 2015 - David Fletcher
        ///     - Simplified and cleaned up
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        private string CleanForSQL(string inStr)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9 ]");

            inStr = inStr.Replace(@"\n", " ").Replace(@"\u000a", " ");

            String outStr = rgx.Replace(inStr, "");

            return outStr;
        }

        /// <summary>
        /// Converts the json business file to a json sql string
        /// 
        /// Created May 23, 2015 - David Fletcher
        /// </summary>
        /// <param name="jsonStr">The line of the of the json file</param>
        /// <returns>sql insert string</returns>
        public string ProcessBusiness(JsonObject jsonStr)
        {
            // return the insert string
            return "INSERT IGNORE INTO businessTable (business_id, name, full_address, city, state, latitude, longitude, stars, review_count, open) VALUES ('"
                   + jsonStr["business_id"].ToString().Replace("\"", "") + "', '"
                   + CleanForSQL(jsonStr["name"].ToString()) + "', '"
                   + CleanForSQL(jsonStr["full_address"].ToString()) + "', '"
                   + CleanForSQL(jsonStr["city"].ToString()) + "', '"
                   + CleanForSQL(jsonStr["state"].ToString()) + "', '"
                   + jsonStr["latitude"].ToString().Replace("\"", "") + "', '"
                   + jsonStr["longitude"].ToString().Replace("\"", "") + "', '"
                   + jsonStr["stars"].ToString().Replace("\"", "") + "', '"
                   + jsonStr["review_count"].ToString().Replace("\"", "") + "', '"
                   + jsonStr["open"].ToString().Replace("\"", "") + "');";
        }

        /// <summary>
        /// Converts the json business files categories to a json sql string
        /// 
        /// Created May 25, 2015 - David Fletcher
        /// </summary>
        /// <param name="jsonStr">The line of the of the json file</param>
        /// <returns>sql insert string</returns>
        public string ProcessBusinessCategories(JsonObject jsonStr)
        {
            StringBuilder insertString = new StringBuilder();
            JsonArray categories = (JsonArray) jsonStr["categories"];

            foreach (int i in categories)
            {
                insertString.Append("INSERT IGNORE INTO categoryTable (business_id, category) VALUES ('");
                insertString.Append(jsonStr["business_id"].ToString().Replace("\"", "") + "', '");
                insertString.Append(CleanForSQL(categories[i].ToString()) + ");\n");
            }
            return insertString.ToString();
        }

        /// <summary>
        /// Converts the json business files neighborhoods to a json sql string
        /// 
        /// Created May 25, 2015 - David Fletcher
        /// </summary>
        /// <param name="jsonStr">The line of the of the json file</param>
        /// <returns>sql insert string</returns>
        public string ProcessBusinessNeighborhoods(JsonObject jsonStr)
        {
            StringBuilder insertString = new StringBuilder();
            JsonArray neighborhoods = (JsonArray)jsonStr["neighborhoods"];

            foreach (int i in neighborhoods)
            {
                insertString.Append("INSERT IGNORE INTO categoryTable (business_id, hood_name) VALUES ('");
                insertString.Append(jsonStr["business_id"].ToString().Replace("\"", "") + "', '");
                insertString.Append(CleanForSQL(neighborhoods[i].ToString()) + ");\n");
            }
            return insertString.ToString();
        }

    }
}
