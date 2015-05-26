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
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void convertBusiness_Click(object sender, RoutedEventArgs e)
        {

            // try converting it to the sql file
            try
            {
                conversionText.Foreground = Brushes.Green;
                Parser jsonToSQL = new Parser();

                StreamReader jsonFile = new StreamReader(businessPath.Text);
                StreamWriter businessSqlFile = new StreamWriter("business.sql");

                string line = jsonFile.ReadLine();
                int counter = 0;

                // loop through the json file line by line and convert the line to sql
                while ((line = jsonFile.ReadLine()) != null)
                {
                    JsonObject jsonStr = (JsonObject)JsonObject.Parse(line);

                    

                    // main business category

                    businessSqlFile.WriteLine(jsonToSQL.ProcessBusiness(jsonStr));
                    businessSqlFile.WriteLine(jsonToSQL.ProcessBusinessNeighborhoods(jsonStr));
                    businessSqlFile.WriteLine(jsonToSQL.ProcessBusinessCategories(jsonStr));
                }
                jsonFile.Close();
                businessSqlFile.Close();

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

        }

        /// <summary>
        /// Starting point for conversion of the review json file
        /// 
        /// Created : May 22nd, 2015 - David Fletcher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void convertReview_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Starting point for conversion of the tip json file
        /// 
        /// Created : May 22nd, 2015 - David Fletcher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void convertTip_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Starting point for conversion of the user json file
        /// 
        /// Created : May 22nd, 2015 - David Fletcher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void convertUser_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Starting point for conversion all of the json files
        /// 
        /// Created : May 22nd, 2015 - David Fletcher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void convertAll_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
