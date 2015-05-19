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
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void businessPathButton_Click(object sender, RoutedEventArgs e)
        {
            // create a new OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // apply the filter to our OpenFileDialog
            dlg.Filter = "JSON file (*.json) | *business.json";

            // run the file dialog
            Nullable<bool> result = dlg.ShowDialog();

            // if file is selected
            if (result == true)
            {
                // put file in the businesspath text box for use later
                businessPath.Text = dlg.FileName;
            }
        }
    }
}
