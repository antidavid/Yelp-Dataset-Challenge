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
        /// Updated June 15, 2015 - David Fletcher
        ///     Changed from type StringBuilder to string to combat crash error
        /// Updated June 16, 2015 - David Fletcher
        ///     StringBuilder data type was not error, foreach loop converted to for loop, no longer breaks
        /// </summary>
        /// <param name="jsonStr">The line of the of the json file</param>
        /// <returns>sql insert string</returns>
        public string ProcessBusinessCategories(JsonObject jsonStr)
        {
            string retString = null;
            JsonArray categories = (JsonArray) jsonStr["categories"];

            for (int i = 0; i < categories.Count; i++ )
            {
                retString += "INSERT IGNORE INTO categoryTable (business_id, category) VALUES ('";
                retString += jsonStr["business_id"].ToString().Replace("\"", "") + "', '";
                retString += CleanForSQL(categories[i].ToString()) + ");\n";
            }
            return retString;
        }

        /// <summary>
        /// Converts the json business files neighborhoods to a json sql string
        /// 
        /// Created May 25, 2015 - David Fletcher
        /// Updated June 15, 2015 - David Fletcher
        ///     Changed from type StringBuilder to string to combat crash error
        /// Updated June 16, 2015 - David Fletcher
        ///     StringBuilder data type was not error, foreach loop converted to for loop, no longer breaks
        ///     Inserts into neighborhoodTable now not categoryTable
        /// </summary>
        /// <param name="jsonStr">The line of the of the json file</param>
        /// <returns>sql insert string</returns>
        public string ProcessBusinessNeighborhoods(JsonObject jsonStr)
        {
            string retString = null;
            JsonArray neighborhoods = (JsonArray)jsonStr["neighborhoods"];

            // iterate through the neighborhoods
            for (int i = 0; i < neighborhoods.Count; i++ )
            {
                retString += "INSERT IGNORE INTO neighborhoodTable (business_id, hood_name) VALUES ('";
                retString += jsonStr["business_id"].ToString().Replace("\"", "") + "', '";
                retString += CleanForSQL(neighborhoods[i].ToString()) + ");\n";
            }
            return retString;
        }

        /// <summary>
        /// Converts the json business files hours to a sql insert string
        /// of the form :
        ///     business_id, week_day, open_time, close_time
        ///     
        /// Created June 16, 2015 - David Fletcher
        /// </summary>
        /// <param name="jsonStr">The main line of the json file</param>
        /// <returns>sql insert string</returns>
        public string ProcessBusinessHours(JsonObject jsonStr)
        {
            string retString = null;
            JsonObject hours = (JsonObject)jsonStr["hours"];

            // iterate through the hours
            foreach (string hour in hours.Keys)
            {
                retString += "INSERT IGNORE INTO hoursTable (business_id, week_day, open_time, close_time) VALUES ('";
                retString += jsonStr["business_id"].ToString().Replace("\"", "") + "', '";
                retString += hour + "', '";
                retString += CleanForSQL(((JsonObject)hours[hour])["open"].ToString()) + "', '";
                retString += CleanForSQL(((JsonObject)hours[hour])["close"].ToString()) + "');\n";
            }
            return retString;
        }

    }
}
