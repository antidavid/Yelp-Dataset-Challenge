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
    /// Interaction logic for User.xaml
    /// </summary>
    public partial class User : Window
    {
        private Dictionary<int, string> userDict = new Dictionary<int, string>();

        public User()
        {
            InitializeComponent();
            
        }

        /// <summary>
        /// Generates a list of users based upon search
        /// 
        /// Created : August 7th, 2015 - David Fletcher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            string sqlQuery = "SELECT user_id, name FROM userTable WHERE name LIKE '" + searchBox.Text + "%' OR name LIKE ' " + searchBox.Text + "%' ORDER BY name ASC";

            SQLConnect con = new SQLConnect();

            List<string> names = con.sqlSelect(sqlQuery, true);

            userDict.Clear();
            string[] elements;
            for (int i = 0; i < names.Count; i++)
            {
                elements = names[i].Split(';');
                userDict.Add(i, elements[0]);
                usersListBox.Items.Add(elements[1].ToString().Trim());
            }


        }

        /// <summary>
        /// Starts the user detail menu
        /// 
        /// Created : August 7th, 2015 - David Fletcher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void friendsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            userDetails view = new userDetails(userDict[(sender as ListBox).SelectedIndex]);

            view.Show();
        }
    }
}
