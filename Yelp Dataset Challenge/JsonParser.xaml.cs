using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Json;
using System.Threading;

namespace Yelp_Dataset_Challenge
{
    /// <summary>
    /// Interaction logic for JsonParser.xaml
    /// </summary>
    public partial class JsonParser : Window
    {
        public JsonParser()
        {
            InitializeComponent();
        }
        /// <summary>
        /// gets the business path of the .json file for parsing
        /// 
        /// Created : May 18th, 2015 - David Fletcher
        /// Updated : May 18th, 2015 - David Fletcher
        ///     - Add support for function call
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void businessPathButton_Click(object sender, RoutedEventArgs e)
        {
            // open a file dialog to find the file with the appended filter
            string filePath = DialogBox("JSON file (*.json) | *business.json");

            if (filePath != null)
            {
                // append text to the appropriate text box
                businessPath.Text = filePath;
            }
        }

        /// <summary>
        /// gets the checkin path of the .json file for parsing
        /// 
        /// Created : May 18th, 2015 - David Fletcher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkinPathButton_Click(object sender, RoutedEventArgs e)
        {
            // open a file dialog to find the file with the appended filter
            string filePath = DialogBox("JSON file (*.json) | *checkin.json");

            if (filePath != null)
            {
                // append text to the appropriate text box
                checkinPath.Text = filePath;
            }
        }

        /// <summary>
        /// gets the review path of the .json file for parsing
        /// 
        /// Created : May 18th, 2015 - David Fletcher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reviewPathButton_Click(object sender, RoutedEventArgs e)
        {
            // open a file dialog to find the file with the appended filter
            string filePath = DialogBox("JSON file (*.json) | *review.json");

            if (filePath != null)
            {
                // append text to the appropriate text box
                reviewPath.Text = filePath;
            }
        }

        /// <summary>
        /// gets the tip path of the .json file for parsing
        /// 
        /// Created : May 18th, 2015 - David Fletcher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tipPathButton_Click(object sender, RoutedEventArgs e)
        {
            // open a file dialog to find the file with the appended filter
            string filePath = DialogBox("JSON file (*.json) | *tip.json");

            if (filePath != null)
            {
                // append text to the appropriate text box
                tipPath.Text = filePath;
            }
        }

        /// <summary>
        /// gets the user path of the .json file for parsing
        /// 
        /// Created : May 18th, 2015 - David Fletcher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void userPathButton_Click(object sender, RoutedEventArgs e)
        {
            // open a file dialog to find the file with the appended filter
            string filePath = DialogBox("JSON file (*.json) | *user.json");

            if (filePath != null)
            {
                // append text to the appropriate text box
                userPath.Text = filePath;
            }
        }

        /// <summary>
        /// Creates the dialog box with the filter applied by the user
        /// 
        /// Created : May 18th, 2015 - David Fletcher
        /// </summary>
        /// <param name="filter">the filter for the dialog box</param>
        /// <returns></returns>
        private string DialogBox(string filter)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // apply the filter to our OpenFileDialog
            dlg.Filter = filter;

            // run the file dialog
            Nullable<bool> result = dlg.ShowDialog();

            // check whether file was selected return value if it was found
            if (result == true)
            {
                return dlg.FileName;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Starting point for conversion of the business json file
        /// 
        /// Created : May 22nd, 2015 - David Fletcher
        /// Updated : May 23rd, 2015 - David Fletcher
        ///     - Added comments
        ///     - Added functionality to write the main business.sql file
        /// Updated : May 25th, 2015 - David Fletcher
        ///     - Added creation of neighborhood, and category
        /// Updated : June 16th, 2015 - David Fletcher 
        ///     - Repaired crashing bug when parsing neighborhood and categories
        ///     - Added creation and population of hours
        /// Updated : June 18th, 2015 - David Fletcher
        ///     - Added creation and population of attributes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void convertBusiness_Click(object sender, RoutedEventArgs e)
        {
            string line;
            // try converting it to the sql file
            try
            {
                conversionText.Foreground = Brushes.Green;
                Parser jsonToSQL = new Parser();

                // open streams
                StreamReader jsonFile = new StreamReader(businessPath.Text);        
                StreamWriter businessSqlFile = new StreamWriter("business.sql");
                StreamWriter categorySqlFile = new StreamWriter("category.sql");
                StreamWriter neighborhoodSqlFile = new StreamWriter("neighborhood.sql");
                StreamWriter hoursSqlFile = new StreamWriter("hours.sql");
                StreamWriter attributesSqlFile = new StreamWriter("attributes.sql");
                // loop through the json file line by line and convert the line to sql
                while ((line = jsonFile.ReadLine()) != null)
                {
                    JsonObject jsonStr  = (JsonObject)JsonObject.Parse(line);

                    // main business category
                    businessSqlFile.WriteLine(jsonToSQL.ProcessBusiness(jsonStr));
                    neighborhoodSqlFile.WriteLine(jsonToSQL.ProcessBusinessNeighborhoods(jsonStr));
                    categorySqlFile.WriteLine(jsonToSQL.ProcessBusinessCategories(jsonStr));
                    hoursSqlFile.WriteLine(jsonToSQL.ProcessBusinessHours(jsonStr));
                    attributesSqlFile.WriteLine(jsonToSQL.ProcessBusinessAttributes(jsonStr));
                }

                // close all streams
                jsonFile.Close();
                businessSqlFile.Close();
                categorySqlFile.Close();
                neighborhoodSqlFile.Close();
                hoursSqlFile.Close();
                attributesSqlFile.Close();

            }
            // if path isn't found display an error message
            catch
            {
                conversionText.Foreground = Brushes.Red;
                conversionText.Text += "Please assign a path to the business.json file\n";
            }
            
        }

        /// <summary>
        /// Starting point for conversion of the checkin json file
        /// 
        /// Created : May 22nd, 2015 - David Fletcher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void convertCheckin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                conversionText.Foreground = Brushes.Green;
                string line;
                StreamReader jsonFile = new StreamReader(checkinPath.Text);
                StreamWriter checkinSqlFile = new StreamWriter("checkin.sql");

                Parser jsonToSql = new Parser();

                while ((line = jsonFile.ReadLine()) != null)
                {
                    JsonObject jsonStr = (JsonObject)JsonObject.Parse(line);

                    checkinSqlFile.WriteLine(jsonToSql.ProcessCheckin(jsonStr));
                }
                jsonFile.Close();
                checkinSqlFile.Close();
            }
            catch
            {
                conversionText.Foreground = Brushes.Red;
                conversionText.Text += "Please assign a path to the checkin.json file\n";
            }
        }

        /// <summary>
        /// Starting point for conversion of the review json file
        /// 
        /// Created : May 22nd, 2015 - David Fletcher
        /// Updated : July 9th, 2015 - David Fletcher
        ///     - Implemented Functionality
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void convertReview_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                conversionText.Foreground = Brushes.Green;
                string line;
                StreamReader jsonFile = new StreamReader(reviewPath.Text);
                StreamWriter reviewSqlFile = new StreamWriter("review.sql");

                Parser jsonToSql = new Parser();

                while ((line = jsonFile.ReadLine()) != null)
                {
                    JsonObject jsonStr = (JsonObject)JsonObject.Parse(line);

                    reviewSqlFile.WriteLine(jsonToSql.ProcessReview(jsonStr));
                }
                jsonFile.Close();
                reviewSqlFile.Close();
            }
            catch
            {
                conversionText.Foreground = Brushes.Red;
                conversionText.Text += "Please assign a path to the review.json file\n";
            }
        }

        /// <summary>
        /// Starting point for conversion of the tip json file
        /// 
        /// Created : May 22nd, 2015 - David Fletcher
        /// Updated : July 9th, 2015 - David Fletcher
        ///     - Implemented Functionality
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void convertTip_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                conversionText.Foreground = Brushes.Green;
                string line;
                StreamReader jsonFile = new StreamReader(tipPath.Text);
                StreamWriter tipSqlFile = new StreamWriter("tip.sql");

                Parser jsonToSql = new Parser();

                while ((line = jsonFile.ReadLine()) != null)
                {
                    JsonObject jsonStr = (JsonObject)JsonObject.Parse(line);

                    tipSqlFile.WriteLine(jsonToSql.ProcessTip(jsonStr));
                }
                jsonFile.Close();
                tipSqlFile.Close();
            }
            catch
            {
                conversionText.Foreground = Brushes.Red;
                conversionText.Text += "Please assign a path to the tip.json file\n";
            }
        }

        /// <summary>
        /// Starting point for conversion of the user json file
        /// 
        /// Created : May 22nd, 2015 - David Fletcher
        /// Updated : July 10th, 2015 - David Fletcher
        ///     - Implemented Functionality
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void convertUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                conversionText.Foreground = Brushes.Green;
                string line;
                StreamReader jsonFile = new StreamReader(userPath.Text);
                StreamWriter userSqlFile = new StreamWriter("users.sql");
                StreamWriter eliteSqlFile = new StreamWriter("elite.sql");
                StreamWriter friendSqlFile = new StreamWriter("friend.sql");
                StreamWriter complimentSqlFile = new StreamWriter("complimnet.sql");

                Parser jsonToSql = new Parser();

                while ((line = jsonFile.ReadLine()) != null)
                {
                    JsonObject jsonStr = (JsonObject)JsonObject.Parse(line);

                    userSqlFile.WriteLine(jsonToSql.ProcessUsers(jsonStr));
                    eliteSqlFile.WriteLine(jsonToSql.ProcessUsersElite(jsonStr));
                    friendSqlFile.WriteLine(jsonToSql.ProcessUsersFriends(jsonStr));
                    complimentSqlFile.WriteLine(jsonToSql.ProcessUsersCompliments(jsonStr));
                }
                jsonFile.Close();
                userSqlFile.Close();
                eliteSqlFile.Close();
                friendSqlFile.Close();
                complimentSqlFile.Close();
            }
            catch
            {
                conversionText.Foreground = Brushes.Red;
                conversionText.Text += "Please assign a path to the user.json file\n";
            }
        }

        /// <summary>
        /// Starting point for conversion all of the json files
        /// 
        /// Created : May 22nd, 2015 - David Fletcher
        /// Updated : June 20th, 2015 - David Fletcher
        ///     Added conversion for business.json to appropriate sql files
        ///     Added conversion for checkin.json to appropriate sql files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void convertAll_Click(object sender, RoutedEventArgs e)
        {
            string line;
            Parser jsonToSQL = new Parser();
         
            // convert business to sql
            try
            {
                conversionText.Foreground = Brushes.Green;
                

                // open streams
                StreamReader jsonFile = new StreamReader(businessPath.Text);
                StreamWriter businessSqlFile = new StreamWriter("business.sql");
                StreamWriter categorySqlFile = new StreamWriter("category.sql");
                StreamWriter neighborhoodSqlFile = new StreamWriter("neighborhood.sql");
                StreamWriter hoursSqlFile = new StreamWriter("hours.sql");
                StreamWriter attributesSqlFile = new StreamWriter("attributes.sql");
                // loop through the json file line by line and convert the line to sql
                while ((line = jsonFile.ReadLine()) != null)
                {
                    JsonObject jsonStr = (JsonObject)JsonObject.Parse(line);

                    // main business category
                    businessSqlFile.WriteLine(jsonToSQL.ProcessBusiness(jsonStr));
                    neighborhoodSqlFile.WriteLine(jsonToSQL.ProcessBusinessNeighborhoods(jsonStr));
                    categorySqlFile.WriteLine(jsonToSQL.ProcessBusinessCategories(jsonStr));
                    hoursSqlFile.WriteLine(jsonToSQL.ProcessBusinessHours(jsonStr));
                    attributesSqlFile.WriteLine(jsonToSQL.ProcessBusinessAttributes(jsonStr));
                }

                // close all streams
                jsonFile.Close();
                businessSqlFile.Close();
                categorySqlFile.Close();
                neighborhoodSqlFile.Close();
                hoursSqlFile.Close();
                attributesSqlFile.Close();

            }
            // if path isn't found display an error message
            catch
            {
                conversionText.Foreground = Brushes.Red;
                conversionText.Text += "business.json not provided\n";
            }

            // convert checkin to sql
            try
            {
                conversionText.Foreground = Brushes.Green;
                StreamReader jsonFile = new StreamReader(checkinPath.Text);
                StreamWriter checkinSqlFile = new StreamWriter("checkin.sql");

                while ((line = jsonFile.ReadLine()) != null)
                {
                    JsonObject jsonStr = (JsonObject)JsonObject.Parse(line);

                    checkinSqlFile.WriteLine(jsonToSQL.ProcessCheckin(jsonStr));
                }
                jsonFile.Close();
                checkinSqlFile.Close();
            }
            catch
            {
                conversionText.Foreground = Brushes.Red;
                conversionText.Text += "checkin.json not provided\n";
            }
        }
    }
}
