using Booking.Commands;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Service;
using Booking.Util;
using Booking.View;
using Booking.WPF.Views.Guest1;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace Booking.WPF.ViewModels.Guest1
{
    public class BookAccommodationViewModel:INotifyPropertyChanged , IDataErrorInfo 
    {
        public IAccommodationReservationService AccommodationReservationService { get; set; }

        public String AccommodationLabel { get; set; } = String.Empty;

        public Accommodation SelectedAccommodation;


        public event PropertyChangedEventHandler PropertyChanged;


        public string _numberOfGuests;
        public string NumberOfGuests
        {
            get => _numberOfGuests;
            set
            {
                if (_numberOfGuests != value)
                {
                    _numberOfGuests = value;
                    OnPropertyChanged();
                }
            }

        }

        public DateTime _arrivalDay;
        public DateTime ArrivalDay
        {
            get => _arrivalDay;
            set
            {
                if (_arrivalDay != value)
                {
                    _arrivalDay = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime _departureDay;
        public DateTime DepartureDay
        {
            get => _departureDay;
            set
            {
                if (_departureDay != value)
                {
                    _departureDay = value;
                    OnPropertyChanged();
                }
            }
        }
        public RelayCommand Reserve_Button_Click { get; set; }
        public RelayCommand Cancle_Button_Click { get; set; }

        public NavigationService NavigationService { get; set; }


        public string Error => null;
        private Regex _numberOfGuestsRegex = new Regex("^[1-9][0-9]?$");

        public string this[string columnName]
        {
            get
            {
                if (columnName == "NumberOfGuests")
                {
                   if(NumberOfGuests == String.Empty)
                   {
                       
                        return "This filed is required!";
                   }
                   else
                   {
                        Match match = _numberOfGuestsRegex.Match(NumberOfGuests);
                        if (!match.Success)
                        {
                            return "Number of guests is not in correct format!";
                        }
                   }
                    
                }
                return null;
            }
        }

        private readonly string[] _validatedProperties = { "NumberOfGuests" };

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }

        public BookAccommodationViewModel(Accommodation selectedAccommodation, NavigationService navigationService)
        {
            SelectedAccommodation = selectedAccommodation;
            SetAccommodationLabel();
            Reserve_Button_Click = new RelayCommand(ReserveButton);
            Cancle_Button_Click = new RelayCommand(CancleButton);
            DepartureDay = DateTime.Now;
            ArrivalDay = DateTime.Now;
            AccommodationReservationService = InjectorService.CreateInstance<IAccommodationReservationService>();
            NavigationService = navigationService;
            NumberOfGuests = String.Empty;
        }

        public void SetAccommodationLabel()
        {
            AccommodationLabel = SelectedAccommodation.Name + "-" + SelectedAccommodation.Location.State + "-" + SelectedAccommodation.Location.City;
        }

        public void ReserveButton(object param)
        {
            var notificationManager = new NotificationManager();

            if (IsValid == false)
            {
                NotificationContent content = new NotificationContent { Title = "Worning!", Message = "You have to put number of guests!", Type = NotificationType.Warning };
                notificationManager.Show(content, areaName: "WindowArea", expirationTime: TimeSpan.FromSeconds(5));
                return;
            }

           
            if (ArrivalDay > DepartureDay)
            {
                NotificationContent content = new NotificationContent { Title = "Permission denied!", Message = "Your ARRIVAL day is after DEPARTURE day", Type = NotificationType.Error };
                notificationManager.Show(content, areaName: "WindowArea", expirationTime: TimeSpan.FromSeconds(5));
            }
            else if ((DepartureDay - ArrivalDay).Days < SelectedAccommodation.MinNumberOfDays)
            {
                NotificationContent content = new NotificationContent { Title = "Permission denied!", Message = "Requirements for the minimal number of days for the reservation are not accomplished", Type = NotificationType.Error };
                notificationManager.Show(content, areaName: "WindowArea", expirationTime: TimeSpan.FromSeconds(5));
            }
            else if (NumberOfGuests.Equals("") || int.Parse(NumberOfGuests) > SelectedAccommodation.Capacity)
            {
                NotificationContent content = new NotificationContent { Title = "Permission denied!", Message = "Number of guests is not valid!", Type = NotificationType.Error };
                notificationManager.Show(content, areaName: "WindowArea", expirationTime: TimeSpan.FromSeconds(5));
            }
            else
            {
                bool IsReserved = AccommodationReservationService.Reserve(ArrivalDay, DepartureDay, SelectedAccommodation);

                if (IsReserved)
                {

                    NotificationContent content = new NotificationContent { Title = "Congratulations!", Message = "You succesfuly reserve your accommodation for: " + ArrivalDay.ToString("dd/MM/yyyy") + " - " + DepartureDay.ToString("dd/MM/yyyy") + " !", Type = NotificationType.Success };
                    notificationManager.Show(content, areaName: "WindowArea", expirationTime: TimeSpan.FromSeconds(5));
                    NavigationService.Navigate(new FirstGuestAllReservations(NavigationService));
                }
                else
                {
                    List<(DateTime, DateTime)> suggestedDateRanges = AccommodationReservationService.SuggestOtherDates(ArrivalDay, DepartureDay, SelectedAccommodation);

                    ShowAvailableDatesDialog(suggestedDateRanges, SelectedAccommodation);
                }
            }
        }

        public void CancleButton(object param)
        {
            NavigationService.GoBack();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ShowAvailableDatesDialog(List<(DateTime, DateTime)> suggestedDateRanges, Accommodation selectedAccommodation)
        {
            NavigationService.Navigate(new AccommodationDateSugestions(suggestedDateRanges, selectedAccommodation));
        }

    }
}
