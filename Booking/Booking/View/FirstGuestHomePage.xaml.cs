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

        //public ObservableCollection<AccommodationType> AccommodationTypes { get; set; }

        public Boolean IsCheckedApartment { get; set; } = false;
        public Boolean IsCheckedCottage { get; set; } = false;
        public Boolean IsCheckedHouse { get; set; } = false;

        public List<string> accommodationTypes;

        public string SearchName { get; set; } = string.Empty;
        public string SearchState { get; set; } = string.Empty;
        public string SearchCity { get; set; } = string.Empty;
       // public AccommodationType SearchType { get; set; } 
        public string SerachGuests { get; set; } = string.Empty;
        public string SearchReservationDays { get; set; } = string.Empty;



        public FirstGuestHomePage()
        {
            InitializeComponent();
            this.DataContext = this;

            accommodationContoller = new AccommodationContoller();
            accommodations = new ObservableCollection<Accommodation>(accommodationContoller.GetAll());

            accommodationTypes = new List<string>();

           // var types = Enum.GetValues(typeof(AccommodationType)).Cast<AccommodationType>();
           // AccommodationTypes = new ObservableCollection<AccommodationType>(types);

            //AccommodationTypesComboBox.ItemsSource = AccommodationTypes; //ovo nije dobro ne radi
            AccommodationDataGrid.ItemsSource = accommodations;
        }

        private void Button_Click_Search(object sender, RoutedEventArgs e)
        {
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

        private void Button_Click_Book(object sender, RoutedEventArgs e)
        {
            AccommodationReservation ar = new AccommodationReservation();
            ar.Show();
        }
    }
}
