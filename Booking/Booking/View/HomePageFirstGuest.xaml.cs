using Booking.Model;
using Booking.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

namespace Booking.View
{
    /// <summary>
    /// Interaction logic for HomePageFirstGuest.xaml
    /// </summary>
    public partial class HomePageFirstGuest : Page
    {

        private ObservableCollection<Accommodation> _accommodations;
        public AccommodationService AccommodationService { get; set; }
        public LocationService LocationService { get; set; }

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

            var locations = LocationService.GetAll().Where(l => l.State.Equals(SearchState));

            foreach (Location c in locations)
            {
                CityCollection.Add(c.City);
            }

            CitycomboBox.IsEnabled = true;

            if (SearchState.Equals("All"))
            {
                CitycomboBox.IsEnabled = false;
            }

        }

        public HomePageFirstGuest()
        {
            InitializeComponent();
            var app = Application.Current as App;
            LocationService = app.LocationService;
            AccommodationService = app.AccommodationService;
            _accommodations = new ObservableCollection<Accommodation>(AccommodationService.GetAll());

            CityCollection = new ObservableCollection<string>();

            accommodationTypes = new List<string>();

            AccommodationDataGrid.ItemsSource = _accommodations;
            FillComboBox();
        }

        public void FillComboBox()
        {
            List<string> items = new List<string>() { "All" };

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

            AccommodationService.Search(_accommodations, SearchName, SearchCity, SearchState, accommodationTypes, SerachGuests, SearchReservationDays);
        }

        private void Button_Click_ShowAll(object sender, RoutedEventArgs e)
        {

            AccommodationService.ShowAll(_accommodations);

        }

        public void BookAccommodation(Accommodation selectedAccommodation)
        {
            AccommodationReservationView dialog = new AccommodationReservationView(selectedAccommodation);
            dialog.Show();
        }

        private void Button_Click_Book(object sender, RoutedEventArgs e)
        {
            //ovo izmeni
            if (SelectedAccommodation == null)
            {
                MessageBox.Show("Please select an accommodation to reserve.");
                return;
            }

            BookAccommodation(SelectedAccommodation);
        }

        
    }
}
