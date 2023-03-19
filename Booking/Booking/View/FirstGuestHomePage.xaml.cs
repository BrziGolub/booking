using Booking.Controller;
using Booking.Model;
using Booking.Model.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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

        private AccommodationContoller accommodationContoller { get; set; }

        private LocationController locationController { get; set; }

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

        public ObservableCollection<string> CityCollection { get; set; }

        public void FillCity(object sender, SelectionChangedEventArgs e)
        {
            CityCollection.Clear();

            var locations = locationController.findAllLocations().Where(l => l.State.Equals(SearchState));

            foreach (Location c in locations)
            {
                CityCollection.Add(c.City);
            }

            CitycomboBox.IsEnabled = true;

        }

        public FirstGuestHomePage()
        {
            InitializeComponent();
            this.DataContext = this;
            var app = Application.Current as App;
            locationController = app.LocationController;
            //povezi accommodationContoller iz APP

            //DA LI TREBA POVEZIT NESTO SA APP?? SERVICE ?
            accommodationContoller = new AccommodationContoller();
            accommodations = new ObservableCollection<Accommodation>(accommodationContoller.GetAll());

            CityCollection = new ObservableCollection<string>();

            accommodationTypes = new List<string>();

            AccommodationDataGrid.ItemsSource = accommodations;
            FillComboBox();
        }

        public void FillComboBox()
        {
            List<string> items = new List<string>();

            using (StreamReader reader = new StreamReader("../../Resources/Data/locations.csv"))
            {
                while (!reader.EndOfStream)
                {

                    string[] fields = reader.ReadLine().Split(',');
                    foreach (var field in fields)
                    {
                        string[] Countries = field.Split('|');
                        items.Add(Countries[1]);
                    }
                }
            }
            var distinctItems = items.Distinct().ToList();

            CountrycomboBox.ItemsSource = distinctItems;

            if (CountrycomboBox.SelectedItem == null)
            {
                CitycomboBox.IsEnabled = false;
            }

        }

        private void Button_Click_Search(object sender, RoutedEventArgs e)
        {
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

            accommodationContoller.ShowAll(accommodations);

        }

        public void BookAccommodation(Accommodation selectedAccommodation)
        {
            AccommodationReservationView dialog = new AccommodationReservationView(selectedAccommodation);
            dialog.Show();
        }

        private void Button_Click_Book(object sender, RoutedEventArgs e)
        {
            if (SelectedAccommodation == null)
            {
                MessageBox.Show("Please select an accommodation to reserve.");
                return;
            }

            BookAccommodation(SelectedAccommodation);
        }
    }
}
