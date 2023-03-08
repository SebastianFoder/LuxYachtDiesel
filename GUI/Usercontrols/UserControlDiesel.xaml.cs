using BIZ;
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

namespace GUI
{
    /// <summary>
    /// Interaction logic for UserControlDiesel.xaml
    /// </summary>
    public partial class UserControlDiesel : UserControl
    {
        ClassBIZ biz;

        /// <summary>
        /// Initializes a new instance of the UserControlDiesel UserControl with the specified biz 
        /// and sets it as the DataContext of the MainGrid.
        /// </summary>
        /// <param name="inBiz">The biz to use.</param>
        public UserControlDiesel(ClassBIZ inBiz)
        {
            InitializeComponent();
            biz = inBiz;
            MainGrid.DataContext = biz;
        }

        /// <summary>
        /// Attempts to insert a new order into the database and displays an error message 
        /// if all the necessary data has not been inputted , data is not within the range, or if an exception occurs.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The event data.</param>
        private void SaveTrade_Click(object sender, RoutedEventArgs e)
        {
            // Checks if the user has inputted all necessary data
            if (biz.customerPrice > 0D)
            {
                // Checks if the user has under 5.000 Liters
                if (Convert.ToInt32(biz.inputLiters) < 5000)
                {
                    MessageBox.Show("ERROR!!!\nMUST INPUT ATLEAST 5.000 LITERS!!!", "ERROR!!!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    // Checks if the user has above 150.000 Liters
                    if (Convert.ToInt32(biz.inputLiters) > 150000)
                    {
                        MessageBox.Show("ERROR!!!\nINPUT IS ABOVE 150.000 LITERS", "ERROR!!!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        try
                        {
                            if (biz.InsertOrderInDB() <= 0)
                            {
                                MessageBox.Show($"ERROR IN DB!!", "ERROR!!", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"ERROR IN DB!! \n{ex.Message}", "ERROR!!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                
            }
            else
            {
                MessageBox.Show("YOU HAVENT INPUTTED ALL THE DATA", "ERROR!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Calls the RegretNewOrderForDB method from biz when the cancel button is clicked.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The event data.</param>
        private void RegredTrade_Click(object sender, RoutedEventArgs e)
        {
            biz.RegretNewOrderForDB();
        }
    }
}
