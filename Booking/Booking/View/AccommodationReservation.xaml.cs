using Booking.Controller;
using Booking.Conversion;
using Booking.Model;
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
    public partial class AccommodationReservation : Window
    {
        private AccommodationReservationController accommodationReservationContoller;

        private Accommodation SelectedAccommodation;
   

        public event PropertyChangedEventHandler PropertyChanged;

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

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AccommodationReservation(Accommodation selectedAccommodation)
        {
            InitializeComponent();
            this.DataContext = this;
            SelectedAccommodation = new Accommodation();
            SelectedAccommodation = selectedAccommodation;
            DepartureDay = DateTime.Now;
            ArrivalDay = DateTime.Now;

            accommodationReservationContoller = new AccommodationReservationController();
        }

        private void Button_Click_Reserve(object sender, RoutedEventArgs e)
        {
             
            Boolean IsReserved = accommodationReservationContoller.Reserve(ArrivalDay, DepartureDay, SelectedAccommodation);

            if(IsReserved)
            {
                MessageBox.Show("You succesfuly reserve your accommodation for:  " + ArrivalDay.ToString("dd/MM/yyyy") + " - " + DepartureDay.ToString("dd/MM/yyyy") + " !");
            }
            else
            {
                MessageBox.Show("There are no available reservations for the seleceted dates!");

              
                //prikaze listu svih mogucih datuma za SELEKOTVANI SMESTAJ 

                List<(DateTime,DateTime)> ranges = accommodationReservationContoller.SuggestOtherDates(ArrivalDay, DepartureDay, SelectedAccommodation);

                //OVO TREBA U WPF U DATA GRID prikazati nemam pojma kako  
                //NOVI PROZOR SA DATA GRID ako hoce ponovo da rezervise 
            }
        }
    }
}
