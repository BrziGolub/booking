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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Booking.Model;
using Booking.Service;

namespace Booking.View
{
    /// <summary>
    /// Interaction logic for ReshaduleAccommodationReservation.xaml
    /// </summary>
    public partial class ReshaduleAccommodationReservation : Page
    {

        public String AccommodationLabel { get; set; } = String.Empty;
        public String ReservedDaysLabel { get; set; } = String.Empty;
        public AccommodationReservation AccommodationReservation { get; set; }
        public AccommodationReservationRequestService AccommodationReservationRequestsService { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public DateTime _arrivalDay;
        public DateTime NewArrivalDay
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
        public DateTime NewDepartureDay
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

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public ReshaduleAccommodationReservation(AccommodationReservation SelectedReservation)
        {
            InitializeComponent();
            this.DataContext = this;
            AccommodationReservation = SelectedReservation;
            AccommodationReservationRequestsService = new AccommodationReservationRequestService();//ovo povezi sa app
            AccommodationLabel = SetAccommodationLabel(SelectedReservation);
            ReservedDaysLabel = SetReservedDaysLabel(SelectedReservation);
            NewDepartureDay = DateTime.Now;
            NewArrivalDay = DateTime.Now;

        }

        private String SetAccommodationLabel(AccommodationReservation accommodationReservation)
        {
            return accommodationReservation.Accommodation.Name + "-" + accommodationReservation.Accommodation.Location.State + "-" + accommodationReservation.Accommodation.Location.City + "-" + accommodationReservation.Accommodation.Type;

        }

        private String SetReservedDaysLabel(AccommodationReservation accommodationReservation)
        {
            return accommodationReservation.ArrivalDay.ToShortDateString() + "-" + accommodationReservation.DepartureDay.ToShortDateString();
        }

        private void Button_Click_SendRequest(object sender, RoutedEventArgs e)
        {
            AccommodationReservationRequestsService.Create(AccommodationReservation, NewArrivalDay, NewDepartureDay);
        }
    }
}
