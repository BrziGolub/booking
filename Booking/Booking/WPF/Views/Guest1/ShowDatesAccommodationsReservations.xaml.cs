using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Service;
using Booking.Util;
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
    /// Interaction logic for ShowDatesAccommodationsReservations.xaml
    /// </summary>
    public partial class ShowDatesAccommodationsReservations : Window
    {
        public ObservableCollection<Range> Ranges { get; set; }

        public Range SelectedDates { get; set; }

        public Accommodation SelectedAccommodation { get; set; }

        //public AccommodationReservationService AccommodationReservationService { get; set; }
        public IAccommodationReservationService AccommodationReservationService { get; set; }

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


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ShowDatesAccommodationsReservations(List<(DateTime, DateTime)> ranges, Accommodation selectedAccommodation)
        {
            InitializeComponent();
            this.DataContext = this;

            Ranges = new ObservableCollection<Range>(ranges.Select(r => new Range { StartDate = r.Item1, EndDate = r.Item2 }).ToList());
            SelectedAccommodation = selectedAccommodation;
            //var app = Application.Current as App;
            //AccommodationReservationService = app.AccommodationReservationService;
            AccommodationReservationService = InjectorService.CreateInstance<IAccommodationReservationService>();
        }

        private void Button_Click_Cancle(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_ReserveDate(object sender, RoutedEventArgs e)
        {

            if (int.Parse(NumberOfGuests) > SelectedAccommodation.Capacity)
            {
                MessageBox.Show("Number of guests is not valid!");

            }
            else if (SelectedDates == null)
            {
                MessageBox.Show("You have to SELECT DATES you want to reserve!");
            }
            else
            {

                //ovde je GRESKA U KONTORKUTORU FALI JOS JEDAN PARAMETAR
                //NIJE uvezan gost sa rezervacijom kad se pravi nova rezervacija
               //Booking.Model.AccommodationReservation newReservation = new Booking.Model.AccommodationReservation(SelectedAccommodation, SelectedDates.StartDate, SelectedDates.EndDate, AccommodationReservationService.SignedFirstGuestId );

              // AccommodationReservationService.SaveReservation(newReservation);

               // MessageBox.Show("You succesfuly reserve your accommodation for: " + SelectedDates.StartDate.ToString("dd/MM/yyyy") + " - " + SelectedDates.EndDate.ToString("dd/MM/yyyy") + " !");
            }

        }
    }

    public class Range
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
