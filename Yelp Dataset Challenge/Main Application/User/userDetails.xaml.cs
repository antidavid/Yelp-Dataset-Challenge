﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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

            // populate base information
            nameLabel.Content = details[1];
            reviewStarsFansLabel.Content = "reviews : " + details[2] + " stars : " + details[3].Substring(0,3) + " fans : " + details[8];
            votesLabel.Content = "votes funny : " + details[4] + " cool : " + details[5] + " useful : " + details[6];
            dateLabel.Content = "yelping since : " + details[7].Replace("\"", "");

            // populate friends
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

            // populate compliment list
            sqlQuery = "SELECT compliment_type, compliment_count FROM complimentTable WHERE user_id LIKE '" + uID.Trim() + "' ORDER BY compliment_type ASC";
            List<string> compliments = new List<string>();
            compliments = con.sqlSelect(sqlQuery, true);

            for (int i = 0; i < compliments.Count; i++)
            {
                string[] compliment = compliments[i].Split(';');
                complimentsList.Items.Add(compliment[0].ToString().Trim() + " : " + compliment[1].ToString().Trim());
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
