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
using BIZ;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ClassBIZ biz;
        UserControlCustomer UCC;
        UserControlDailyPrice UCDP;
        UserControlDiesel UCD;
        UserControlSupplier UCS;

        /// <summary>
        /// Initializes a new instance of the MainWindow and sets up the data context and user controls 
        /// for displaying customer, daily price, diesel, and supplier data. 
        /// If the daily price for today has not been set, selects the DailyPrice tab.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            biz = new ClassBIZ();
            MainGrid.DataContext = biz;
            UCC = new UserControlCustomer(biz);
            UCDP = new UserControlDailyPrice(biz);
            UCD = new UserControlDiesel(biz);
            UCS = new UserControlSupplier(biz);
            Diesel.Content = UCD;
            Customer.Content = UCC;
            Suppiler.Content = UCS;
            DailyPrice.Content = UCDP;
            if (!biz.tabControlEnabled)
            {
                TabControl.SelectedIndex = 3;
            }
        }

        /// <summary>
        /// Calls the openexhangerates API every 5 minutes to update the rates
        /// in the biz to allow the most up to date rates
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">The event data.</param>
        private async void MainGrid_Loaded(object sender, RoutedEventArgs e)
        {
            while (true)
            {
                await biz.GetAllCurrencysWebAPI();
                await Task.Delay(300000);
            }
        }
    }
}
