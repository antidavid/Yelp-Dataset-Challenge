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
            businessListInit();
        }

        public void businessListInit()
        {
            string sqlString = "SELECT name FROM businessTable";
            SQLConnect con = new SQLConnect();
            List<string> list = con.sqlSelect(sqlString);

            foreach (string item in list)
            {
                businessList.Items.Add(item);
            }
        }
    }
}
