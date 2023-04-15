using Booking.Domain.Model;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Observer;
using Booking.Service;
using Booking.Util;
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
    public partial class FirstGuestAllReservations : Page, IObserver
    {
        public ObservableCollection<AccommodationReservation> _reservations;
        
        public IAccommodationReservationService _accommodationReservationService;

        public INotificationService _notificationService;

        public IAccommodationAndOwnerGradeService accommodationAndOwnerGradeService;
        public AccommodationReservation SelectedReservation { get; set; }

        public FirstGuestAllReservations()
        {
            InitializeComponent();
            this.DataContext = this;
            SelectedReservation = new AccommodationReservation();
            accommodationAndOwnerGradeService = InjectorService.CreateInstance<IAccommodationAndOwnerGradeService>();
            _accommodationReservationService = InjectorService.CreateInstance<IAccommodationReservationService>();
            _notificationService = InjectorService.CreateInstance<INotificationService>();

            _reservations = new ObservableCollection<AccommodationReservation>(_accommodationReservationService.GetGeustsReservatonst());

            _accommodationReservationService.Subscribe(this);
            ReservationsDataGrid.ItemsSource = _reservations;

            SetWidthForReservationsDataGrid();

        }
        public void SetWidthForReservationsDataGrid()
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
            /*if (accommodationAndOwnerGradeService.PermissionForRating(SelectedReservation))
            {
                MessageBox.Show("You are unable to rate you accomodation and owner");
            }
            else
            {
                NavigationService.Navigate(new RateAccommodationAndOwner(SelectedReservation));
            }*/
            NavigationService.Navigate(new RateAccommodationAndOwner(SelectedReservation));
        }

        private void Button_Click_ResheduleAccommodationReservation(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ReshaduleAccommodationReservation(SelectedReservation));
        }

        private void Button_Click_CancleReservation(object sender, RoutedEventArgs e)
        {
            bool cancel = _accommodationReservationService.IsAbleToCancleResrvation(SelectedReservation);
            Notification newNotification = new Notification();

            if (cancel)
            {
                newNotification.Text = "Reservation for: " + SelectedReservation.Accommodation.Name + " " + SelectedReservation.ArrivalDay.ToShortDateString() + " - " + SelectedReservation.DepartureDay.ToShortDateString() + " is cancled guest: " + SelectedReservation.Guest.Username.ToString();
                newNotification.User = SelectedReservation.Accommodation.Owner;
                newNotification.IsRead = false;
                _notificationService.CreateCancellationNotification(newNotification);

                _accommodationReservationService.Delete(SelectedReservation);
                MessageBox.Show("Your reservation is cancelled!"); 
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
