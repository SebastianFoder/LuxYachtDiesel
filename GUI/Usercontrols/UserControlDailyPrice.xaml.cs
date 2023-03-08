using BIZ;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace GUI
{
    /// <summary>
    /// Interaction logic for UserControlDailyPrice.xaml
    /// </summary>
    public partial class UserControlDailyPrice : UserControl
    {
        ClassBIZ biz;

        /// <summary>
        /// Initializes a new instance of the UserControlDailyPrice UserControl with the specified biz 
        /// and sets it as the DataContext of the MainGrid.
        /// </summary>
        /// <param name="inBiz">The biz to use.</param>
        public UserControlDailyPrice(ClassBIZ inBiz)
        {
            InitializeComponent();
            biz = inBiz;
            MainGrid.DataContext = biz;
        }

        /// <summary>
        /// Attempts to start a new process to navigate to the price website when the link is clicked. 
        /// If an exception occurs, shows an error message with the exception details.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The event data.</param>
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
                e.Handled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ERROR OPENING BROWSER!! \n{ex.Message}", "ERROR!!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Attempts to save a new diesel price to the database and displays an error message if the price is zero or negative, 
        /// or if an exception occurs.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The event data.</param>
        private void Save_Update_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToDouble(biz.diselPriceInput) > 0)
            {
                try
                {
                    if (biz.NewDieselPriceForDB() <= 0)
                    {
                        MessageBox.Show($"ERROR IN DB!!", "ERROR!!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"ERROR IN DB!! \n{ex.Message}", "ERROR!!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("ERROR!!\nInputted value must be above 0!!", "ERROR!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }            
        }

        /// <summary>
        /// Event handler for the Loaded event of the MainGrid control.
        /// Continuously updates the current time in the biz object every second.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The event data.</param>
        private async void MainGrid_Loaded(object sender, RoutedEventArgs e)
        {
            while (true)
            {
                biz.currentTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").ToUpperInvariant();
                await Task.Delay(1000);
            }
        }
    }
}
