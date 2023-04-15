using Booking.Conversion;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Service;
using Booking.Util;
using System;
using System.Collections.Generic;
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
    public partial class AccommodationReservationView : Window
    {
        public IAccommodationReservationService AccommodationReservationService { get; set; }

        private Accommodation SelectedAccommodation;

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



        public AccommodationReservationView(Accommodation selectedAccommodation)
        {
            InitializeComponent();
            this.DataContext = this;
            SelectedAccommodation = new Accommodation();
            SelectedAccommodation = selectedAccommodation;
            DepartureDay = DateTime.Now;
            ArrivalDay = DateTime.Now;
            //var app = Application.Current as App;

            //AccommodationReservationService = app.AccommodationReservationService;

            //AccommodationReservationService = new AccommodationReservationService();

            AccommodationReservationService = InjectorService.CreateInstance<IAccommodationReservationService>();

        }

        private void ShowAvailableDatesDialog(List<(DateTime, DateTime)> suggestedDateRanges, Accommodation selectedAccommodation)
        {
            ShowDatesAccommodationsReservations dialog = new ShowDatesAccommodationsReservations(suggestedDateRanges, selectedAccommodation);
            dialog.Show();
        }


        private void Button_Click_Reserve(object sender, RoutedEventArgs e)
        {
            if (ArrivalDay > DepartureDay)
            {
                MessageBox.Show("Your arrival day is after departure day!");

            }
            else if ((DepartureDay - ArrivalDay).Days < SelectedAccommodation.MinNumberOfDays)
            {
                MessageBox.Show("Requirements for the minimal number of days for the reservation are not accomplished");

            }
            else if (NumberOfGuests.Equals("") || int.Parse(NumberOfGuests) > SelectedAccommodation.Capacity)
            {
                MessageBox.Show("Number of guests is not valid!");
            }
            else
            {
                bool IsReserved = AccommodationReservationService.Reserve(ArrivalDay, DepartureDay, SelectedAccommodation);

                if (IsReserved)
                {
                    MessageBox.Show("You succesfuly reserve your accommodation for: " + ArrivalDay.ToString("dd/MM/yyyy") + " - " + DepartureDay.ToString("dd/MM/yyyy") + " !");
                }
                else
                {
                    MessageBox.Show("There are no available reservations for the seleceted dates!");

                    List<(DateTime, DateTime)> suggestedDateRanges = AccommodationReservationService.SuggestOtherDates(ArrivalDay, DepartureDay, SelectedAccommodation);

                    ShowAvailableDatesDialog(suggestedDateRanges, SelectedAccommodation);
                }
            }
        }

        private void Button_Click_Cancle(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
