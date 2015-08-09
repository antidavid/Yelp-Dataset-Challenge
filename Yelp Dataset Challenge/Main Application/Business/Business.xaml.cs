using System.Collections.Generic;
using System.Windows;

namespace Yelp_Dataset_Challenge
{
    /// <summary>
    /// Interaction logic for business.xaml
    /// </summary>
    public partial class Business : Window
    {
        
        public Business(string bID)
        {
            InitializeComponent();
            initializeLabels(bID);
        }

        /// <summary>
        /// Initialize all labels for the business page
        /// 
        /// Created July 30th, 2015 - David Fletcher
        /// Updated August 1st, 2015 - David Fletcher
        ///     - Added city state label update 
        ///     - Added lat and long label
        ///     - Added stars and review count label
        /// Updated August 2nd, 2015 - David Fletcher
        ///     - Removed city state label
        ///     - added support for seperation tags
        /// Updated August 5th, 2015 - David Fletcher
        ///     - Converted to textbox to allow for scrolling
        /// Updated August 6th, 2015 - David Fletcher
        ///     - Added stars, date and username to reviews
        /// Updated August 7th, 2015 - David Fletcher
        ///     - Simplified initializer to reduce sql queries and to help optimize
        ///     - Added hours display
        ///     - Added tips display
        /// </summary>
        /// <param name="bID"></param>
        private void initializeLabels(string bID)
        {
            string sqlQuery = "SELECT * FROM businessTable WHERE business_id LIKE '" + bID + "';";
            //string sqlQuery = "SELECT address FROM businessTable WHERE business_id LIKE '" + bID + "';";

            SQLConnect con = new SQLConnect();
            List<string> list = new List<string>();
            string[] elements;

            list = con.sqlSelect(sqlQuery, true);
            elements = list[0].Split(';');

            addressLabel.Content = elements[2];
            latLongLabel.Content = "latitude : " + elements[5] + " longitude : " + elements[6];
            starsReviewLabel.Content = "stars : " + elements[7] + " review count : " + elements[8];

            sqlQuery = "SELECT week_day, open_time, close_time FROM hoursTable WHERE business_id LIKE '" + bID + "' ORDER BY CASE WHEN week_day = 'Sunday' THEN 1 WHEN week_day = 'Monday' THEN 2 WHEN week_day = 'Tuesday' THEN 3 WHEN week_day = 'Wednesday' THEN 4 WHEN week_day = 'Thursday' THEN 5 WHEN week_day = 'Friday' THEN 6 WHEN week_day = 'Saturday' THEN 7 END ASC; ";
            list.Clear();
            list = con.sqlSelect(sqlQuery, true);

            for (int i = 0; i < list.Count; i+=2)
            {
                if (i == 0)
                {
                    textBox.Text += "hours of operation\n";
                    textBox.Text += "--------------------------------------------------------------------------\n";
                }
                elements = list[i].Split(';');
                textBox.Text += elements[0] + " open : " + elements[1] + " close : " + elements[2] + "\n";
            }

            // attributes
            sqlQuery = "SELECT attribute_name, attribute_value FROM attributeTable WHERE business_id LIKE '" + bID + "' ORDER BY attribute_name ASC;";
            list = con.sqlSelect(sqlQuery, true);

            for(int i = 0; i < list.Count; i+=2)
            {
                if (i == 0)
                {
                    textBox.Text += "attributes\n";
                    textBox.Text += "--------------------------------------------------------------------------\n";
                }

                elements = list[i].Split(';');
                textBox.Text += elements[0] + " " + elements[1] + "\n";
            }
            textBox.Text += "\n";

            // reviews
            sqlQuery = "SELECT * FROM reviewTable WHERE business_id LIKE '" + bID + "';";
            list.Clear();
            list = con.sqlSelect(sqlQuery, true);

            // cycle through reviews
            for (int i = 0; i < list.Count; i++)
            {
                if (i == 0)
                {
                    textBox.Text += "reviews\n";
                    textBox.Text += "--------------------------------------------------------------------------\n";
                }

                elements = list[i].Split(';');
                sqlQuery = "SELECT name FROM userTable WHERE user_id LIKE '" + elements[1] + "';";

                List<string> name = new List<string>();
                name = con.sqlSelect(sqlQuery, false);

                textBox.Text += name[0] + " date : " + elements[5] + " stars : " + elements[3] + "\n";
                textBox.Text += elements[4] + "\n";
                textBox.Text += "votes funny : " + elements[6] + " useful : " + elements[7] + " cool : " + elements[8] + "\n\n";
            }

            // tips
            sqlQuery = "SELECT * FROM tipTable WHERE business_id LIKE '" + bID + "';";
            list.Clear();
            list = con.sqlSelect(sqlQuery, true);

            for (int i = 0; i < list.Count; i++)
            {
                if (i == 0)
                {
                    textBox.Text += "tips\n";
                    textBox.Text += "--------------------------------------------------------------------------\n";
                }

                elements = list[i].Split(';');

                sqlQuery = "SELECT name FROM userTable WHERE user_id LIKE '" + elements[1] + "';";

                List<string> name = new List<string>();
                name = con.sqlSelect(sqlQuery, false);

                textBox.Text += name[0] + " date : " + elements[3] + " likes : " + elements[4] + "\n";
                textBox.Text += elements[2] + "\n\n";
            }

            // checkins
            sqlQuery = "SELECT checkin_info, checkin_amount FROM checkinTable WHERE business_id LIKE '" + bID + "';";
            list.Clear();
            list = con.sqlSelect(sqlQuery, true);

            for (int i = 0; i < list.Count; i++)
            {
                if (i == 0)
                {
                    textBox.Text += "checkin\n";
                    textBox.Text += "--------------------------------------------------------------------------\n";
                }

                elements = list[i].Split(';');

                textBox.Text += elements[0] + " : " + elements[1] + " times\n";
            }
        }
    }
}
