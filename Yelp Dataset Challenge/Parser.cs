using System;
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
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        private string CleanForSQL(string inStr)
        {
            String outStr = Encoding.GetEncoding("iso-8859-1").GetString(Encoding.UTF8.GetBytes(inStr));
            outStr = outStr.Replace("\"", "").Replace("'", " ").Replace(@"\n", " ").Replace(@"\u000a", " ").Replace("\\", " ").Replace("é", "e").Replace("ê", "e").Replace("Ã¼", "A").Replace("Ã", "A").Replace("¤", "").Replace("©", "c").Replace("¶", "");
            outStr = Regex.Replace(outStr, @"[^\u0020-\u007E]", "?");

            //Only get he first maxLength chars. Set maxLength to the max length of your attribute.
            return outStr.Substring(0, Math.Min(outStr.Length, maxLength));
        }

        /// <summary>
        /// Converts the json business file to a json sql file
        /// 
        /// Created May 23, 2015 - David Fletcher
        /// </summary>
        /// <param name="jsonStr">The line of the of the json file</param>
        /// <returns>sql insert string</returns>
        public string ProcessBusiness(JsonObject jsonStr)
        {
            return "INSERT INTO businessTable (business_id, name, full_address, city, state, latitude, longitude, stars, review_count, open) VALUES ('"
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

    }
}
