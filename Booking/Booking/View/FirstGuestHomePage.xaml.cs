using Booking.Controller;
using Booking.Model;
using Booking.Model.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public Boolean IsCheckedApartment { get; set; } = false;
        public Boolean IsCheckedCottage { get; set; } = false;
        public Boolean IsCheckedHouse { get; set; } = false;

        public List<string> accommodationTypes;

        public string SearchName { get; set; } = string.Empty;
        public string SearchState { get; set; } = string.Empty;
        public string SearchCity { get; set; } = string.Empty;
        public string SerachGuests { get; set; } = string.Empty;
        public string SearchReservationDays { get; set; } = string.Empty;

        public Accommodation SelectedAccommodation { get; set; }

        public FirstGuestHomePage()
        {
            InitializeComponent();
            this.DataContext = this;

            var app = Application.Current as App;
            accommodationContoller = app.AccommodationController;
            accommodations = new ObservableCollection<Accommodation>(accommodationContoller.GetAll());

            accommodationTypes = new List<string>();

            AccommodationDataGrid.ItemsSource = accommodations;
        }

        private void Button_Click_Search(object sender, RoutedEventArgs e)
        {
            //svaki put kad ponovo ide na search isprazni prethodnu listu i dodaj noci Accommodation type !!
            accommodationTypes.Clear(); 

            if (IsCheckedApartment)
            {
                accommodationTypes.Add("APARTMENT");
            }
            if (IsCheckedCottage)
            {
                accommodationTypes.Add("COTTAGE");
            }
            if (IsCheckedHouse)
            {
                accommodationTypes.Add("HOUSE");
            }

            accommodationContoller.Search(accommodations, SearchName, SearchCity, SearchState, accommodationTypes, SerachGuests, SearchReservationDays);
        }

        private void Button_Click_ShowAll(object sender, RoutedEventArgs e)
        {
            //resetuj sve ako klikne na show all
            SearchName = string.Empty;
            SearchState = string.Empty;
            SearchCity = string.Empty;
            SerachGuests = string.Empty;
            SearchReservationDays = string.Empty;

            IsCheckedApartment = false;
            IsCheckedCottage = false;
            IsCheckedHouse = false;

            accommodationTypes.Clear();
           

            accommodationContoller.ShowAll(accommodations);
            
        }

        private void Button_Click_Book(object sender, RoutedEventArgs e)
        {
            if(SelectedAccommodation == null)
            {
                MessageBox.Show("You have to select accommodation that you want to reserve! ");
                return;
            }
            AccommodationReservation ar = new AccommodationReservation(SelectedAccommodation);
            ar.Show();
        }
    }
}
