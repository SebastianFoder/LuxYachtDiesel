using BIZ;
using Repository;
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
    /// Interaction logic for UserControlSupplier.xaml
    /// </summary>
    public partial class UserControlSupplier : UserControl
    {
        ClassBIZ biz;

        /// <summary>
        /// Initializes a new instance of the UserControlSupplier UserControl with the specified biz 
        /// and sets it as the DataContext of the MainGrid.
        /// </summary>
        /// <param name="inBiz">The biz to use.</param>
        public UserControlSupplier(ClassBIZ inBiz)
        {
            InitializeComponent();
            biz = inBiz;
            MainGrid.DataContext = biz;
        }

        /// <summary>
        /// Handles the Click event of the create button, and allows for editing and 
        /// makes a blank supplier to edit in response.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ButtonOpret_Click(object sender, RoutedEventArgs e)
        {
            biz.UCSIsReadOnly = false;
            biz.tabControlEnabled = false;
            biz.fallbackSupplier = new ClassSupplier();
        }

        /// <summary>
        /// Handles the Click event of the edit button, and allows for editing in response.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ButtonRediger_Click(object sender, RoutedEventArgs e)
        {
            biz.UCSIsReadOnly = false;
            biz.tabControlEnabled = false;
        }

        /// <summary>
        /// Handles the Click event of the save button and attempts to update or insert supplier data into the database, 
        /// and displays an error message if necessary.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ButtonGem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (biz.UpdateOrInsertSupplierInDB() <= 0)
                {
                    MessageBox.Show($"ERROR IN DB!!", "ERROR!!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ERROR IN DB!! \n{ex.Message}", "ERROR!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Calls the RegretUpdateOrNewSupplierForDB method from biz when the cancel button is clicked.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The event data.</param>
        private void ButtonFortryd_Click(object sender, RoutedEventArgs e)
        {
            biz.RegretUpdateOrNewSupplierForDB();
        }
    }
}
