using Booking.Commands;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Observer;
using Booking.Util;
using Booking.View;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Booking.WPF.ViewModels.Guest1
{
    public class FirstGuestAllReservationsViewModel: IObserver
    {
        public ObservableCollection<AccommodationReservation> Reservations { get; set; }

        public IAccommodationReservationService _accommodationReservationService;

        public INotificationService _notificationService;

        public IAccommodationAndOwnerGradeService accommodationAndOwnerGradeService;
        public AccommodationReservation SelectedReservation { get; set; }
        public NavigationService NavigationService { get; set; }

        public RelayCommand Button_Click_RateAccommodationAndOwner { get; set; }
        public RelayCommand Button_Click_ResheduleAccommodationReservation { get; set; }
        public RelayCommand Button_Click_CancleReservation { get; set; }

        public FirstGuestAllReservationsViewModel(NavigationService navigationService)
        {
            SelectedReservation = new AccommodationReservation();
            accommodationAndOwnerGradeService = InjectorService.CreateInstance<IAccommodationAndOwnerGradeService>();
            _accommodationReservationService = InjectorService.CreateInstance<IAccommodationReservationService>();
            _notificationService = InjectorService.CreateInstance<INotificationService>();

            Reservations = new ObservableCollection<AccommodationReservation>(_accommodationReservationService.GetGeustsReservatonst());

            _accommodationReservationService.Subscribe(this);
            
            NavigationService = navigationService;

            this.Button_Click_RateAccommodationAndOwner = new RelayCommand(ButtonRateAccommodationAndOwner);
            this.Button_Click_ResheduleAccommodationReservation = new RelayCommand(ButtonResheduleAccommodationReservation);
            this.Button_Click_CancleReservation = new RelayCommand(ButtonCancleReservation);
        }

        private void ButtonRateAccommodationAndOwner(object param)
        {
            var notificationManager = new NotificationManager();
            NotificationContent content = new NotificationContent { Title = "Permission denied!", Message = "You are unable to rate you accomodation and owner", Type = NotificationType.Error };


            if (accommodationAndOwnerGradeService.PermissionForRating(SelectedReservation))
            {
                notificationManager.Show(content, areaName: "WindowArea", expirationTime: TimeSpan.FromSeconds(30));
            }
            else
            {
                NavigationService.Navigate(new RateAccommodationAndOwner(SelectedReservation, NavigationService));
            }
        }

        private void ButtonResheduleAccommodationReservation(object param)
        {
            var notificationManager = new NotificationManager();

            if (SelectedReservation == null)
            {
                NotificationContent content = new NotificationContent { Title = "Permission denied!", Message = "Please select your reservation!", Type = NotificationType.Error };
                notificationManager.Show(content, areaName: "WindowArea", expirationTime: TimeSpan.FromSeconds(5));
            }
            else
            {
                NavigationService.Navigate(new ReshaduleAccommodationReservation(SelectedReservation, NavigationService));
            }
        }

        private void ButtonCancleReservation(object param)
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
            else if (SelectedReservation == null)
            {
                NotificationContent content = new NotificationContent { Title = "Permission denied!", Message = "Please select your reservation!", Type = NotificationType.Error };
                notificationManager.Show(content, areaName: "WindowArea", expirationTime: TimeSpan.FromSeconds(5));
            }
            else
            {
                notificationManager.Show(contentDenied, areaName: "WindowArea", expirationTime: TimeSpan.FromSeconds(30));
            }
        }
        public void Update()
        {
            Reservations.Clear();
            //ovde logicko brisanje
            foreach (var reservation in _accommodationReservationService.GetGeustsReservatonst())
            {
                Reservations.Add(reservation);
            }
        }


    }
}
