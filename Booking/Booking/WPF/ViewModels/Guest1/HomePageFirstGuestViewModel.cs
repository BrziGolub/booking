using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Util;
using Booking.View;
using Booking.WPF.Views.Guest1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Booking.WPF.ViewModels.Guest1
{
    public class HomePageFirstGuestViewModel
    {
        private ObservableCollection<Accommodation> Accommodations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public List<string> accommodationTypes;
        public IAccommodationService AccommodationService { get; set; }
        public ILocationService LocationService { get; set; }

        public Boolean IsCheckedApartment { get; set; } = false;
        public Boolean IsCheckedCottage { get; set; } = false;
        public Boolean IsCheckedHouse { get; set; } = false;
        public string SearchName { get; set; } = string.Empty;
        public string SearchState { get; set; } = string.Empty;
        public string SearchCity { get; set; } = string.Empty;
        public string SerachGuests { get; set; } = string.Empty;
        public string SearchReservationDays { get; set; } = string.Empty;


        public ObservableCollection<string> CityCollection { get; set; }
        public ObservableCollection<string> CountrycomboBox { get; set; }
       
        public bool CityComboboxEnabled { get; set; }

        public void FillCity(object sender, SelectionChangedEventArgs e)
        {
            CityCollection.Clear();

            var locations = LocationService.GetAll().Where(l => l.State.Equals(SearchState));

            foreach (Location c in locations)
            {
                CityCollection.Add(c.City);
            }

            CityComboboxEnabled = true;

            if (SearchState.Equals("All"))
            {
                CityComboboxEnabled = false;
            }
        }

        public NavigationService NavigationService { get; set; }

        public HomePageFirstGuestViewModel(NavigationService navigationService)
        {

            LocationService = InjectorService.CreateInstance<ILocationService>();
            AccommodationService = InjectorService.CreateInstance<IAccommodationService>();

            Accommodations = new ObservableCollection<Accommodation>(AccommodationService.GetAllSuper().OrderByDescending(a => a.Owner.Super));

            CityCollection = new ObservableCollection<string>();
            CountrycomboBox = new ObservableCollection<string>();
            NavigationService = navigationService;

            accommodationTypes = new List<string>();

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

            // CountrycomboBox = distinctItems;
            UpdateCountryComboBox(distinctItems);

            if (SearchState == null)
            {
                CityComboboxEnabled = false;
            }
        }

        public void UpdateCountryComboBox(List<string> coutries)
        {
            CountrycomboBox.Clear();
            foreach(string str in coutries)
            {
                CountrycomboBox.Add(str);
            }
        }


        private void ButtonSearch(object sender, RoutedEventArgs e)
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

            AccommodationService.Search(Accommodations, SearchName, SearchCity, SearchState, accommodationTypes, SerachGuests, SearchReservationDays);
        }

        private void ButtonShowAll(object sender, RoutedEventArgs e)
        {

            AccommodationService.ShowAll(Accommodations);

        }

        //za ovo napraviti Page:
        private void ButtonBook(object sender, RoutedEventArgs e)
        {

            if (SelectedAccommodation == null)
            {
                MessageBox.Show("Please select an accommodation to reserve.");
                return;
            }
            AccommodationReservationView dialog = new AccommodationReservationView(SelectedAccommodation);
            dialog.Show();
        }

        private void ButtonShowImages(object sender, RoutedEventArgs e)
        {

            NavigationService.Navigate(new ShowAccommodationImages(SelectedAccommodation));

        }


    }
}
