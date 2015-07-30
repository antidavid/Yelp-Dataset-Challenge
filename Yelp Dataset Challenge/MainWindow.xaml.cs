/****************************************************************************
 * Yelp Dataset Challenge                                                   *
 * Author : David Fletcher                                                  *
 * Created : May 18th, 2015                                                 *
 * Launguages : C#                                                          *
 ***************************************************************************/

using System.Windows;

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
