using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for mainApp.xaml
    /// </summary>
    public partial class mainApp : Window
    {
        public mainApp()
        {
            InitializeComponent();
            //businessListInit();
            initializeContent();
        }

        /// <summary>
        /// Initialize the page
        /// 
        /// Created July 28th, 2015 - David Fletcher
        /// </summary>
        public void initializeContent()
        {
            string sqlString = "SELECT DISTINCT state FROM businessTable ORDER BY state ASC;";
            SQLConnect con = new SQLConnect();
            List<string> states = con.sqlSelect(sqlString);

            foreach (string item in states)
            {
                stateComboBox.Items.Add(item);
            }
        }

        /// <summary>
        /// Action that occurs when the search button is clicked
        /// 
        /// Created July 28th, 2015 - David Fletcher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            string sqlString = "SELECT name FROM businessTable ";
            if (stateComboBox.SelectedIndex > -1)
            {
                sqlString += "WHERE state LIKE '" + stateComboBox.Text.Trim() + "';";
            }

            SQLConnect con = new SQLConnect();
            List<string> list = con.sqlSelect(sqlString);

            foreach (string item in list)
            {
                businessList.Items.Add(item);
            }
        }


    }
}
