using Booking.Domain.Model;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Observer;
using Booking.Service;
using Booking.Util;
using Notifications.Wpf.Controls;
using Notifications.Wpf;
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
            var notificationManager = new NotificationManager();
            NotificationContent content = new NotificationContent { Title = "Permission denied!", Message = "You are unable to rate you accomodation and owner" , Type = NotificationType.Error };
           

            if (accommodationAndOwnerGradeService.PermissionForRating(SelectedReservation))
            {
                notificationManager.Show(content, areaName: "WindowArea", expirationTime: TimeSpan.FromSeconds(30));
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
            bool cancel = _accommodationReservationService.IsAbleToCancleResrvation(SelectedReservation);

            var notificationManager = new NotificationManager();
            NotificationContent contentDenied = new NotificationContent { Title = "Permission denied!", Message = "You are unable to cancle yout resevartion!", Type = NotificationType.Error };
            NotificationContent contentAllowed = new NotificationContent { Title = "Succesful!", Message = "Your reservation is cancelled!", Type = NotificationType.Success };

            if (cancel)
            {
                _notificationService.MakeCancellationNotification(SelectedReservation);
                _accommodationReservationService.Delete(SelectedReservation);

                notificationManager.Show(contentAllowed, areaName: "WindowArea", expirationTime: TimeSpan.FromSeconds(5));
            }
            else
            {
                notificationManager.Show(contentDenied, areaName: "WindowArea", expirationTime: TimeSpan.FromSeconds(30));
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
