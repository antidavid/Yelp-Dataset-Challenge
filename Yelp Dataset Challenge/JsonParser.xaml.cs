using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
    }
}
