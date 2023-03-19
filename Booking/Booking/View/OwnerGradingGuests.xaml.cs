using Booking.Controller;
using Booking.Model;
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
using System.Windows.Shapes;

namespace Booking.View
{
    /// <summary>
    /// Interaction logic for OwnerGradingGuests.xaml
    /// </summary>
    public partial class OwnerGradingGuests : Window
    {
        public AccommodationReservationController accommodationReservationController;
        public ObservableCollection<AccommodationReservation> reservations { get; set; }
        public AccommodationReservation SelectedReservation { get; set; }
        public OwnerGradingGuests()
        {
            InitializeComponent();
            this.DataContext = this;
            var app = Application.Current as App;
            accommodationReservationController = app.AccommodationReservationController;
            reservations = new ObservableCollection<AccommodationReservation>(accommodationReservationController.GetAllUngradedReservations());
        }
        private void Button_Click_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_Grade(object sender, RoutedEventArgs e)
        {
            GradingWindow gradingWindow = new GradingWindow(SelectedReservation.Id, this);
            gradingWindow.Show();
        }

        public void RefreshWindow()
        {
            List<AccommodationReservation> accomodationReservations = accommodationReservationController.GetAllUngradedReservations();
            reservations.Clear();
            foreach (AccommodationReservation accommodationReservation in accomodationReservations)
            {
                reservations.Add(accommodationReservation);
            }
        }
    }
}
