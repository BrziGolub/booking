using Booking.Controller;
using Booking.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<Accommodation> accommodations;

        private AccommodationContoller accommodationContoller;
        public FirstGuestHomePage()
        {
            InitializeComponent();
            this.DataContext = this;

            accommodationContoller = new AccommodationContoller();
            accommodations = new ObservableCollection<Accommodation>(accommodationContoller.GetAll());

            AccommodationDataGrid.ItemsSource =  accommodations;
        }

        private void Button_Click_Search(object sender, RoutedEventArgs e)
        {
            FirstGuestSearch firstGuestSearch = new FirstGuestSearch();
            firstGuestSearch.Show();
        }
    }
}
