using Booking.Model;
using Booking.Observer;
using Booking.Service;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Booking.View
{
    /// <summary>
    /// Interaction logic for FirstGuestAllReservations.xaml
    /// </summary>
    public partial class FirstGuestAllReservations : Page, IObserver
    {
      

        public ObservableCollection<AccommodationReservation> _reservations;
        public AccommodationReservationService _accommodationReservationService;
        public AccommodationReservation SelectedReservation { get; set; }
        public FirstGuestAllReservations()
        {
            InitializeComponent();
            this.DataContext = this;
            var app = Application.Current as App;
            _accommodationReservationService = app.AccommodationReservationService;
            
            _reservations = new ObservableCollection<AccommodationReservation>(_accommodationReservationService.GetGeustsReservatonst());
            _accommodationReservationService.Subscribe(this);
            ReservationsDataGrid.ItemsSource = _reservations;

            setWidthForReservationsDataGrid();

        }

        public bool IsFiveDaysRuleAcomlished(AccommodationReservation  selectedReservation)
        {
            if(selectedReservation == null)
            {
                return false;

            }else if(selectedReservation.ArrivalDay <= DateTime.Now && DateTime.Now >= selectedReservation.ArrivalDay.AddDays(5))
            {
                return true;
            }
            return false;
        }


        public void setWidthForReservationsDataGrid()
        {
            double totalWidth = 0;
            foreach (DataGridColumn column in ReservationsDataGrid.Columns)
            {
                if (column.ActualWidth > 0)
                {
                    totalWidth += column.ActualWidth;
                }
            }
            ReservationsDataGrid.Width = totalWidth;
        }

        private void Button_Click_RateAccommodationAndOwner(object sender, RoutedEventArgs e)
        {
            if (IsFiveDaysRuleAcomlished(SelectedReservation))
            {
                MessageBox.Show("You are unable to rate you accomodation and owner");
            }
            else
            {
                NavigationService.Navigate(new RateAccommodationAndOwner(SelectedReservation));
            }
        }

        private void Button_Click_ResheduleAccommodationReservation(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ReshaduleAccommodationReservation(SelectedReservation));
        }

        private void Button_Click_CancleReservation(object sender, RoutedEventArgs e)
        {
            bool cancle = _accommodationReservationService.IsAbleToCancleResrvation(SelectedReservation);
            if (cancle)
            {
                _accommodationReservationService.Delete(SelectedReservation);
                MessageBox.Show("Your reservation is canceled!"); 
            }
            else
            {
                MessageBox.Show("You are unable to cancle reservation!");
            }
        }

        public void Update()
        {
            _reservations.Clear();
            foreach(var reservation in _accommodationReservationService.GetGeustsReservatonst())
            {
                _reservations.Add(reservation);
            }
        }
    }
}
