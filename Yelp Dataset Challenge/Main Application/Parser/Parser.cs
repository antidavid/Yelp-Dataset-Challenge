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
        /// Updated August 3rd, 2015 - David FLetcher
        ///     - Updated so that review text would parse appropriately
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
        /// Converts the json business file to a json sql string
        /// of the form :
        ///     business_id, name, full_address, city, state, latitude, longitude, stars, review_count, open
        /// 
        /// Created May 23, 2015 - David Fletcher
        /// Updated July 27, 2015 - David Fletcher
        ///     Adapted to new SQL Layout
        /// </summary>
        /// <param name="jsonStr">The line of the of the json file</param>
        /// <returns>sql insert string</returns>
        public string ProcessBusiness(JsonObject jsonStr)
        {
            // return the insert string
            return "INSERT INTO businessTable (business_id, name, address, city, state, latitude, longitude, stars, review_count, is_open) VALUES ('"
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
        /// of the form :
        ///     business_id, category
        /// 
        /// Created May 25, 2015 - David Fletcher
        /// Updated June 15, 2015 - David Fletcher
        ///     Changed from type StringBuilder to string to combat crash error
        /// Updated June 16, 2015 - David Fletcher
        ///     StringBuilder data type was not error, foreach loop converted to for loop, no longer breaks
        /// Updated July 27, 2015 - David Fletcher
        ///     Adapted to new SQL Layout
        ///     /// </summary>
        /// <param name="jsonStr">The line of the of the json file</param>
        /// <returns>sql insert string</returns>
        public string ProcessBusinessCategories(JsonObject jsonStr)
        {
            string retString = null;
            JsonArray categories = (JsonArray) jsonStr["categories"];

            for (int i = 0; i < categories.Count; i++ )
            {
                retString += "INSERT INTO categoryTable (business_id, category) VALUES ('";
                retString += jsonStr["business_id"].ToString().Replace("\"", "") + "', '";
                retString += CleanForSQL(categories[i].ToString()) + "');\n";
            }
            return retString;
        }

        /// <summary>
        /// Converts the json business files neighborhoods to a json sql string
        /// of the form :
        ///     business_id, hood_name
        /// 
        /// Created May 25th, 2015 - David Fletcher
        /// Updated June 15th, 2015 - David Fletcher
        ///     Changed from type StringBuilder to string to combat crash error
        /// Updated June 16th, 2015 - David Fletcher
        ///     StringBuilder data type was not error, foreach loop converted to for loop, no longer breaks
        ///     Inserts into neighborhoodTable now not categoryTable
        /// Updated July 27th, 2015 - David Fletcher
        ///     Adapted to new SQL Layout
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
                retString += "INSERT INTO neighborhoodTable (business_id, neighborhood_name) VALUES ('";
                retString += jsonStr["business_id"].ToString().Replace("\"", "") + "', '";
                retString += CleanForSQL(neighborhoods[i].ToString()) + "');\n";
            }
            return retString;
        }

        /// <summary>
        /// Converts the json business files hours to a sql insert string
        /// of the form :
        ///     business_id, week_day, open_time, close_time
        ///     
        /// Created June 16th, 2015 - David Fletcher
        /// Updated July 27th, 2015 - David Fletcher
        ///     Adapted to new SQL Layout
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
                retString += "INSERT INTO hoursTable (business_id, week_day, open_time, close_time) VALUES ('";
                retString += jsonStr["business_id"].ToString().Replace("\"", "") + "', '";
                retString += hour + "', '";
                retString += CleanForSQL(((JsonObject)hours[hour])["open"].ToString()) + "', '";
                retString += CleanForSQL(((JsonObject)hours[hour])["close"].ToString()) + "');\n";
            }
            return retString;
        }

        /// <summary>
        /// Converts the json business files hours to a sql insert string
        /// for attributes of the form :
        ///     business_id, attribute_name, attribute_value
        ///     
        /// Created June 17th, 2015 - David Fletcher
        /// Updated June 18th, 2015 - David Fletcher
        ///     Created case for nested attributes
        /// Updated July 27th, 2015 - David Fletcher
        ///     Adapted to new SQL Layout
        /// </summary>
        /// <param name="jsonStr">main line of json file</param>
        /// <returns>sql insert string</returns>
        public string ProcessBusinessAttributes(JsonObject jsonStr)
        {
            string retString = null;
            JsonObject attributes = (JsonObject)jsonStr["attributes"];

            // iterate through attributes
            foreach (string attribute in attributes.Keys)
            {
                JsonObject nestedAttribute = attributes[attribute] as JsonObject;

                if (nestedAttribute == null)
                {
                    retString += "INSERT INTO attributeTable (business_id, attribute_name, attribute_value) VALUES ('";
                    retString += jsonStr["business_id"].ToString().Replace("\"", "") + "', '";
                    retString += attribute + "', '";
                    retString += CleanForSQL(attributes[attribute].ToString()) + "');\n";
                }
                else // iterate through nested attributes
                {
                    foreach (string nestedKey in nestedAttribute.Keys)
                    {
                        retString += "INSERT INTO attributeTable (business_id, attribute_name, attribute_value) VALUES ('";
                        retString += jsonStr["business_id"].ToString().Replace("\"", "") + "', '";
                        retString += nestedKey + "', '";
                        retString += CleanForSQL(nestedAttribute[nestedKey].ToString() + "');\n");
                    }

                }

            }
            return retString;
        }

        /// <summary>
        /// Converts the json checkin file to a sql insert string
        /// for of the form :
        ///     business_id, checkin_info, checkin_amount
        ///     
        /// Created June 20th, 2015 - David Fletcher
        /// Updated July 27, 2015 - David Fletcher
        ///     Adapted to new SQL Layout
        /// </summary>
        /// <param name="jsonStr">main line of json file</param>
        /// <returns>sql insert string</returns>
        public string ProcessCheckin(JsonObject jsonStr)
        {
            string retString = null;
            JsonObject checkIn = (JsonObject)jsonStr["checkin_info"];

            // iterate through checkins
            foreach (string check in checkIn.Keys)
            {
                retString += "INSERT INTO checkinTable (business_id, checkin_info, checkin_amount) VALUES ('";
                retString += jsonStr["business_id"].ToString().Replace("\"", "") + "', '";
                retString += check + "', '";
                retString += CleanForSQL(checkIn[check].ToString()) + "');\n";
            }
            return retString;
        }

        /// <summary>
        /// Converts the json review file to a sql insert string
        /// of the form :
        ///     business_id, user_id, stars, text, date, votes, review_id
        ///     
        /// Created July 9th, 2015 - David Fletcher
        /// Updated July 27, 2015 - David Fletcher
        ///     Adapted to new SQL Layout
        /// Updated August 3rd, 2015 - David Fletcher
        ///     repaired the text portion so that it parses properly
        /// 
        /// Potential Problems... 
        ///     string length for text may be too long unsure at the moment
        ///     may need to adjust date
        /// </summary>
        /// <param name="jsonStr">Line of json file</param>
        /// <returns>sql insert string</returns>
        public string ProcessReview(JsonObject jsonStr)
        {
            string retString = null;
            JsonObject votes = (JsonObject)jsonStr["votes"];

            retString += "INSERT INTO reviewTable (business_id, user_id, review_id, stars, text, date, votes_funny, votes_useful, votes_cool) VALUES ('";
            retString += CleanForSQL(jsonStr["business_id"].ToString()) + "', '";
            retString += CleanForSQL(jsonStr["user_id"].ToString()) + "', '";
            retString += CleanForSQL(jsonStr["review_id"].ToString()) + "', '";
            retString += CleanForSQL(jsonStr["stars"].ToString()) + "', '";
            retString += CleanForSQL(jsonStr["text"].ToString()) + "', '";
            retString += CleanForSQL(jsonStr["date"].ToString());
            foreach (string type in votes.Keys)
            {
                retString +=  "', '" + CleanForSQL(votes[type].ToString());
            }
            retString += "');\n";

            return retString;
        }

        /// <summary>
        /// Converts the json tip file to a sql insert string
        /// of the form :
        ///     business_id, user_id, text, date, likes
        ///     
        /// Created July 9th, 2015 - David Fletcher
        /// Updated July 27, 2015 - David Fletcher
        ///     Adapted to new SQL Layout
        /// 
        /// Potential Problems... 
        ///     string length for text may be too long unsure at the moment
        ///     May need to adjust date
        /// </summary>
        /// <param name="jsonStr">Line of json file</param>
        /// <returns>sql insert string</returns>
        public string ProcessTip(JsonObject jsonStr)
        {
            string retString = null;

            retString += "INSERT INTO tipTable (business_id, user_id, text, date, likes) VALUES ('";
            retString += jsonStr["business_id"].ToString().Replace("\"", "") + "', '";
            retString += jsonStr["user_id"].ToString().Replace("\"", "") + "', '";
            retString += CleanForSQL(jsonStr["text"].ToString()) + "', '";
            retString += CleanForSQL(jsonStr["date"].ToString()) + "', '";
            retString += CleanForSQL(jsonStr["likes"].ToString()) + "');\n";

            return retString;
        }

        /// <summary>
        /// Converts the json users file to a sql insert string
        /// of the form :
        ///     user_id, name, review_count, average_stars, votes_funny, votes_useful, votes_cool, yelping_since, fans
        ///     
        /// Created July 10th, 2015 - David Fletcher
        /// Updated July 27, 2015 - David Fletcher
        ///     Adapted to new SQL Layout
        /// Updated August 3rd, 2015 - David Fletcher
        ///     insert statement modified
        /// </summary>
        /// <param name="jsonStr">Line of json file</param>
        /// <returns>sql insert string</returns>
        public string ProcessUsers(JsonObject jsonStr)
        {
            string retString = null;
            JsonObject votes = (JsonObject)jsonStr["votes"];

            retString += "INSERT INTO userTable (user_id, name, review_count, average_stars, votes_funny, votes_useful, votes_cool, yelping_since, fans) VALUES ('";
            retString += jsonStr["user_id"].ToString().Replace("\"", "") + "', '";
            retString += CleanForSQL(jsonStr["name"].ToString()) + "', '";
            retString += jsonStr["review_count"].ToString() + "', '";
            retString += jsonStr["average_stars"].ToString();
            foreach (string type in votes.Keys)
            {
                retString += "', '" + CleanForSQL(votes[type].ToString());
            }
            retString += "', '" + jsonStr["yelping_since"].ToString() + "', '";
            retString += jsonStr["fans"].ToString() + "');";

            return retString;
        }

        /// <summary>
        /// Converts the json users file to a sql insert string
        /// of the form :
        ///     user_id, year
        ///     
        /// Created July 10th, 2015 - David Fletcher
        /// Updated July 27, 2015 - David Fletcher
        ///     Adapted to new SQL Layout
        /// </summary>
        /// <param name="jsonStr">Line of json file</param>
        /// <returns>sql insert string</returns>
        public string ProcessUsersElite(JsonObject jsonStr)
        {
            string retString = null;
            JsonArray elite = (JsonArray)jsonStr["elite"];

            for (int i = 0; i < elite.Count; i++)
            {
                retString += "INSERT INTO eliteTable(user_id, year) VALUES ('";
                retString += jsonStr["user_id"].ToString().Replace("\"", "") + "', '";
                retString += elite[i].ToString() + "');\n";
            }

            return retString;
        }

        /// <summary>
        /// Converts the json users file to a sql insert string
        /// of the form :
        ///     user_id, friend_id
        ///     
        /// Created July 10th, 2015 - David Fletcher
        /// Updated July 27, 2015 - David Fletcher
        ///     Adapted to new SQL Layout
        /// Updated August 3rd, 2015 - David Fletcher
        ///     insert statement modified
        /// Updated August 4th, 2015 - David Fletcher
        ///     Removed quotes around users friend
        /// </summary>
        /// <param name="jsonStr">Line of json file</param>
        /// <returns>sql insert string</returns>
        public string ProcessUsersFriends(JsonObject jsonStr)
        {
            string retString = null;
            JsonArray friends = (JsonArray)jsonStr["friends"];

            for (int i = 0; i < friends.Count; i++)
            {
                retString += "INSERT INTO friendTable(user_id, friend_id) VALUES ('";
                retString += jsonStr["user_id"].ToString().Replace("\"", "") + "', '";
                retString += friends[i].ToString().Replace("\"", "") + "');\n";
            }

            return retString;
        }

        /// <summary>
        /// Converts the json users file to a sql insert string
        /// of the form :
        ///     user_id, compliment_type, compliment_count
        ///     
        /// Created July 10th, 2015 - David Fletcher
        /// Updated July 27, 2015 - David Fletcher
        ///     Adapted to new SQL Layout
        /// </summary>
        /// <param name="jsonStr">Line of json file</param>
        /// <returns>sql insert string</returns>
        public string ProcessUsersCompliments(JsonObject jsonStr)
        {
            string retString = null;
            JsonObject compliments = (JsonObject)jsonStr["compliments"];

            // iterate through attributes
            foreach (string compliment in compliments.Keys)
            {

                retString += "INSERT INTO complimentTable (user_id, compliment_type, compliment_count) VALUES ('";
                retString += jsonStr["user_id"].ToString().Replace("\"", "") + "', '";
                retString += compliment + "', '";
                retString += CleanForSQL(compliments[compliment].ToString()) + "');\n";


            }
            return retString;
        }
    }
}
