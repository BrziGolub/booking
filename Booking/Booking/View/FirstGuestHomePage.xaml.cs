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

namespace Booking.View
{
    /// <summary>
    /// Interaction logic for FirstGuestHomePage.xaml
    /// </summary>
    public partial class FirstGuestHomePage : Window
    {
        public FirstGuestHomePage()
        {
            InitializeComponent();
        }

        private void Button_Click_Search(object sender, RoutedEventArgs e)
        {
            FirstGuestSearch firstGuestSearch = new FirstGuestSearch();
            firstGuestSearch.Show();
        }
    }
}
