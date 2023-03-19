using Booking.Controller;
using Booking.Model;
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

        private AccommodationReservationController accommodationReservationController;

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

            accommodationReservationController = new AccommodationReservationController(); //Ovo povezi sa app
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
                //ovo promeni ime, milovic je promenio resi konflikte
                Booking.Model.AccommodationReservation newReservation = new Booking.Model.AccommodationReservation(SelectedAccommodation, SelectedDates.StartDate, SelectedDates.EndDate);

                accommodationReservationController.SaveReservation(newReservation);

                MessageBox.Show("You succesfuly reserve your accommodation for: " + SelectedDates.StartDate.ToString("dd/MM/yyyy") + " - " + SelectedDates.EndDate.ToString("dd/MM/yyyy") + " !");
            }

        }
    }

    public class Range
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
