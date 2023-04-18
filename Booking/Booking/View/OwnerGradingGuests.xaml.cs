using Booking.Domain.ServiceInterfaces;
using Booking.Model;
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
using System.Windows.Shapes;

namespace Booking.View
{
    /// <summary>
    /// Interaction logic for OwnerGradingGuests.xaml
    /// </summary>
    public partial class OwnerGradingGuests : Window
    {
        public IAccommodationReservationService AccommodationReservationService { get; set; }
        public ObservableCollection<AccommodationReservation> reservations { get; set; }
        public AccommodationReservation SelectedReservation { get; set; }
        public OwnerGradingGuests()
        {
            InitializeComponent();
            this.DataContext = this;
            AccommodationReservationService = InjectorService.CreateInstance<IAccommodationReservationService>();

            reservations = new ObservableCollection<AccommodationReservation>(AccommodationReservationService.GetAllUngradedReservations());
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
            List<AccommodationReservation> accomodationReservations = AccommodationReservationService.GetAllUngradedReservations();
            reservations.Clear();
            foreach (AccommodationReservation accommodationReservation in accomodationReservations)
            {
                reservations.Add(accommodationReservation);
            }
        }
    }
}
