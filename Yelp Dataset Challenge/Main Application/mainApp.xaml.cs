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
        private Dictionary<int, string> businessDict = new Dictionary<int, string>();

        public mainApp()
        {
            InitializeComponent();
            //businessListInit();
            initializeContent();
            initializeStars();
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
            stateComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Action that occurs when the search button is clicked
        /// 
        /// Created July 28th, 2015 - David Fletcher
        /// Updated July 29th, 2015 - David Fletcher
        ///     Added support for stars condition
        ///     Added search functionality
        ///     Added zip searching
        /// Updated July 30th, 2015 - David Fletcher
        ///     Changed to support a dictionary for finding business_id 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            bool where = false;
            businessList.Items.Clear();

            string sqlString = "SELECT business_id, name FROM businessTable ";
            // check if state is selected
            if (stateComboBox.SelectedIndex > -1)
            {
                sqlString = appendCond(sqlString, where);
                sqlString += "state LIKE '" + stateComboBox.Text.Trim() + "' ";

                where = true;
            }
            
            // check if city is selected
            if (cityComboBox.Text.Trim() != "any" && cityComboBox.SelectedIndex > -1)
            {
                sqlString = appendCond(sqlString, where);
                sqlString += "city LIKE '" + cityComboBox.Text.Trim() + "' ";

                where = true;
            }

            // check if star count is selected
            if (starsComboBox.Text.Trim() != "any" && starsComboBox.SelectedIndex > -1)
            {
                sqlString = appendCond(sqlString, where);
                sqlString += "stars >= " + starsComboBox.Text.Trim() + " ";

                where = true;
            }

            // check if search box is populated
            if (!string.IsNullOrEmpty(searchTextBox.Text.Trim()))
            {
                sqlString = appendCond(sqlString, where);
                sqlString += "name LIKE '%" + searchTextBox.Text.Trim() + "%' ";

                where = true;
            }

            // search by zip code
            if (!string.IsNullOrEmpty(zipTextBox.Text.Trim()))
            {
                sqlString = appendCond(sqlString, where);
                sqlString += "address LIKE '% " + zipTextBox.Text.Trim() + "' ";

                where = true;
            }

            sqlString += "ORDER BY name ASC;";

            SQLConnect con = new SQLConnect();
            List<string> list = con.sqlSelect(sqlString);

            businessDict.Clear();

            for (int i = 0; i < list.Count; i++)
            {
                businessDict.Add(i, list[i].Substring(0, list[i].IndexOf(' ')));

                businessList.Items.Add(list[i].Substring(list[i].IndexOf(' ') + 1));
            }

        }

        /// <summary>
        /// Populate the city combobox when the state combobox has been changed.
        /// 
        /// Created July 28th, 2015 - David Fletcher
        /// Updated July 29th, 2015 - David Fletcher
        ///     - Fixed bug 1
        /// 
        /// Fixes Required : 
        ///     1) Need to fix update... not updating when state is changing properly using old one, Fixed : July 29th, 2015 - David Fletcher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cityComboBox.Items.Clear();

            cityComboBox.Items.Add("any");

            string sqlString = "SELECT DISTINCT city FROM businessTable WHERE state LIKE '";
            //sqlString += stateComboBox.Text.Trim() + "' ORDER BY city ASC;";
            sqlString += (sender as ComboBox).SelectedItem.ToString().Trim() + "' ORDER BY city ASC;";

            SQLConnect con = new SQLConnect();
            List<string> list = con.sqlSelect(sqlString);

            foreach (string city in list)
            {
                cityComboBox.Items.Add(city);
            }

            cityComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// determines whether to append an and or a where to the query string
        /// 
        /// CREATED July 28th, 2015 - David Fletcher
        /// UPDATED July 29th, 2015 - David Fletcher
        ///     Added the ability to determine between where and and
        /// </summary>
        /// <param name="appendString"></param>
        /// <param name="isWhere"></param>
        /// <returns></returns>
        private string appendCond(string appendString, bool where)
        {
            if (where == false)
            {
                return appendString + "WHERE ";
            }
            else
            {
                return appendString + "AND ";
            }
        }

        /// <summary>
        /// Populates the starsComboBox, allows users to filter by stars
        /// 
        /// Created July 29th, 2015 - David Fletcher
        /// </summary>
        private void initializeStars()
        {
            starsComboBox.Items.Add("any");
            for (double i = 0; i <= 5; i += 0.5)
            {
                starsComboBox.Items.Add(i);
            }
        }

        private void businessList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //businessSqlString += "AND name LIKE '" + (sender as ListBox).SelectedItem.ToString().Trim() + "';";
            Business view = new Business(businessDict[(sender as ListBox).SelectedIndex]);
            view.businessLabel.Content = (sender as ListBox).SelectedItem.ToString().Trim();

            //view.bID = businessDict[(sender as ListBox).SelectedIndex];

            view.Show();
        }
    }
}
