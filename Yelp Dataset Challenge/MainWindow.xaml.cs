/****************************************************************************
 * Yelp Dataset Challenge                                                   *
 * Author : David Fletcher                                                  *
 * Created : May 18th, 2015                                                 *
 * Launguages : C#                                                          *
 ***************************************************************************/

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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Yelp_Dataset_Challenge
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Launch the parseJson file
        /// 
        /// Created : May 18th, 2015 - David Fletcher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void parseJson_Click(object sender, RoutedEventArgs e)
        {
            JsonParser temp = new JsonParser();

            temp.Show();
        }

        private void appLaunch_Click(object sender, RoutedEventArgs e)
        {
            mainApp temp = new mainApp();

            temp.Show();
        }

    }
}
