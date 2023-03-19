﻿using Booking.Controller;
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
    /// Interaction logic for OwnerHomePage.xaml
    /// </summary>
    public partial class OwnerHomePage : Window
    {
        public AccommodationReservationController accommodationReservationController;
        public ObservableCollection<AccommodationReservation> reservations { get; set; }
        public OwnerHomePage()
        {
            InitializeComponent();
            this.DataContext = this;
            var app = Application.Current as App;
            accommodationReservationController = app.AccommodationReservationController;
            reservations = new ObservableCollection<AccommodationReservation>(accommodationReservationController.GetAllUngradedReservations());
            if (reservations.Count != 0)
            {
                MessageBox.Show("Number of guests to grade: " + reservations.Count.ToString());
            }
        }

        private void RegisterAccomodation(object sender, RoutedEventArgs e)
        {
            OwnerRegisterAccommodation ownerRegisterAccommodation = new OwnerRegisterAccommodation();
            ownerRegisterAccommodation.Show();
        }

        private void GradingGuests(object sender, RoutedEventArgs e)
        {
            OwnerGradingGuests ownerGradingGuests = new OwnerGradingGuests();
            ownerGradingGuests.Show();
        }
    }
}
