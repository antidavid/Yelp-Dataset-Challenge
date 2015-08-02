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
        /// </summary>
        /// <param name="bID"></param>
        private void initializeLabels(string bID)
        {
            string sqlQuery = "SELECT address FROM businessTable WHERE business_id LIKE '" + bID + "';";

            SQLConnect con = new SQLConnect();
            List<string> list = new List<string>();

            list = con.sqlSelect(sqlQuery);

            addressLabel.Content = list[0];

            sqlQuery = "SELECT city, state FROM businessTable WHERE business_id LIKE '" + bID + "';";

            list.Clear();

            list = con.sqlSelect(sqlQuery);
            cityStateLabel.Content = list[0];

            sqlQuery = "SELECT latitude, longitude FROM businessTable WHERE business_id LIKE '" + bID + "';";

            list.Clear();

            list = con.sqlSelect(sqlQuery);
            latLongLabel.Content = list[0];

            sqlQuery = "SELECT stars, review_count FROM businessTable WHERE business_id LIKE '" + bID + "';";

            list.Clear();

            list = con.sqlSelect(sqlQuery);
            starsReviewLabel.Content = list[0];
        }
    }
}
