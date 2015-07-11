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
        /// of the form :
        ///     business_id, name, full_address, city, state, latitude, longitude, stars, review_count, open
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
        /// of the form :
        ///     business_id, category
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
        /// of the form :
        ///     business_id, hood_name
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

        /// <summary>
        /// Converts the json business files hours to a sql insert string
        /// for attributes of the form :
        ///     business_id, attribute_name, attribute_value
        ///     
        /// Created June 17th, 2015 - David Fletcher
        /// Updated June 18th, 2015 - David Fletcher
        ///     Created case for nested attributes
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
                    retString += "INSERT IGNORE INTO attributeTable (business_id, attribute_name, attribute_value) VALUES ('";
                    retString += jsonStr["business_id"].ToString().Replace("\"", "") + "', '";
                    retString += attribute + "', '";
                    retString += CleanForSQL(attributes[attribute].ToString()) + "');\n";
                }
                else // iterate through nested attributes
                {
                    foreach (string nestedKey in nestedAttribute.Keys)
                    {
                        retString += "INSERT IGNORE INTO attributeTable (business_id, attribute_name, attribute_value) VALUES ('";
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
                retString += "INSERT IGNORE INTO checkinTable (business_id, checkin_info, checkin_amount) VALUES ('";
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

            retString += "INSERT IGNORE INTO reviewTable (business_id, user_id, review_id, stars, text, date, votes_funny, votes_useful, votes_cool) VALUES ('";
            retString += jsonStr["business_id"].ToString().Replace("\"", "") + "', '";
            retString += jsonStr["user_id"].ToString().Replace("\"", "") + "', '";
            retString += jsonStr["review_id"].ToString().Replace("\"", "") + "', '";
            retString += CleanForSQL(jsonStr["stars"].ToString()) + "', '";
            retString += jsonStr["text"].ToString().Replace("\"", "") + "', '";
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

            retString += "INSERT IGNORE INTO tipTable (business_id, user_id, text, date, likes) VALUES ('";
            retString += jsonStr["business_id"].ToString().Replace("\"", "") + "', '";
            retString += jsonStr["user_id"].ToString().Replace("\"", "") + "', '";
            retString += jsonStr["text"].ToString().Replace("\"", "") + "', '";
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
        /// </summary>
        /// <param name="jsonStr">Line of json file</param>
        /// <returns>sql insert string</returns>
        public string ProcessUsers(JsonObject jsonStr)
        {
            string retString = null;
            JsonObject votes = (JsonObject)jsonStr["votes"];

            retString += "INSERT IGNORE INTO usersTable (user_id, name, review_count, average_stars, votes_funny, votes_useful, votes_cool, yelping_since, fans) VALUES ('";
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
        /// </summary>
        /// <param name="jsonStr">Line of json file</param>
        /// <returns>sql insert string</returns>
        public string ProcessUsersElite(JsonObject jsonStr)
        {
            string retString = null;
            JsonArray elite = (JsonArray)jsonStr["elite"];

            for (int i = 0; i < elite.Count; i++)
            {
                retString += "INSERT IGNORE INTO eliteTable(user_id, year) VALUES ('";
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
        /// </summary>
        /// <param name="jsonStr">Line of json file</param>
        /// <returns>sql insert string</returns>
        public string ProcessUsersFriends(JsonObject jsonStr)
        {
            string retString = null;
            JsonArray friends = (JsonArray)jsonStr["friends"];

            for (int i = 0; i < friends.Count; i++)
            {
                retString += "INSERT IGNORE INTO friendsTable(user_id, friend_id) VALUES ('";
                retString += jsonStr["user_id"].ToString().Replace("\"", "") + "', '";
                retString += friends[i].ToString() + "');\n";
            }

            return retString;
        }

        /// <summary>
        /// Converts the json users file to a sql insert string
        /// of the form :
        ///     user_id, compliment, compliment_count
        ///     
        /// Created July 10th, 2015 - David Fletcher
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

                retString += "INSERT IGNORE INTO complimentTable (user_id, compliment, compliment_count) VALUES ('";
                retString += jsonStr["user_id"].ToString().Replace("\"", "") + "', '";
                retString += compliment + "', '";
                retString += CleanForSQL(compliments[compliment].ToString()) + "');\n";


            }
            return retString;
        }
    }
}
