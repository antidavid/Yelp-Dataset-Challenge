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
    /// Interaction logic for userDetails.xaml
    /// 
    /// Created : August 7th, 2015 - David Fletcher
    /// </summary>
    public partial class userDetails : Window
    {
        private Dictionary<int, string> friendDict = new Dictionary<int, string>();

        public userDetails(string uID)
        {
            InitializeComponent();
            initializeMenu(uID);
        }

        /// <summary>
        /// initialize the friend details window
        /// 
        /// Created : August 7th, 2015 - David Fletcher
        /// </summary>
        /// <param name="uID"></param>
        private void initializeMenu(string uID)
        {
            string sqlQuery = "SELECT * FROM userTable WHERE user_id LIKE '" + uID + "';";
            SQLConnect con = new SQLConnect();
            List<string> userDetails = new List<string>();

            userDetails = con.sqlSelect(sqlQuery, true);

            string[] details = userDetails[0].Split(';');

            nameLabel.Content = details[1];
            reviewStarsFansLabel.Content = "reviews : " + details[2] + " stars : " + details[3].Substring(0,3) + " fans : " + details[8];
            votesLabel.Content = "votes funny : " + details[4] + " cool : " + details[5] + " useful : " + details[6];
            dateLabel.Content = "yelping since : " + details[7].Replace("\"", "");

            sqlQuery = "SELECT friend_id FROM friendTable WHERE user_id LIKE '" + uID.Trim() + "';";
            List<string> friends = new List<string>();
            friends = con.sqlSelect(sqlQuery, false);
            for (int i = 0; i < friends.Count; i++)
            {
                sqlQuery = "SELECT name FROM userTable WHERE user_id LIKE '" + friends[i].Trim() + "';";
                userDetails.Clear();
                userDetails = con.sqlSelect(sqlQuery, false);
                if (userDetails.Count > 0)
                {
                    friendDict.Add(i, friends[i].Trim());
                    friendList.Items.Add(userDetails[0].ToString());
                }
            }
        }

        /// <summary>
        /// Launch a new userDetails window containing the information about the friends 
        /// 
        /// Created August 7th, 2015 - David Fletcher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void friendList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            userDetails view = new userDetails(friendDict[(sender as ListBox).SelectedIndex]);

            view.Show();
        }
    }
}
