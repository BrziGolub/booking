﻿using Booking.Model;
using Booking.Service;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Booking.View
{
    /// <summary>
    /// Interaction logic for FirstGuestAllReservations.xaml
    /// </summary>
    public partial class FirstGuestAllReservations : Page
    {
      

        public ObservableCollection<AccommodationReservation> _reservations;
        public AccommodationReservationService _accommodationReservationService;
        public AccommodationReservation SelectedReservation { get; set; }
        public FirstGuestAllReservations()
        {
            InitializeComponent();
            this.DataContext = this;
            var app = Application.Current as App;
            _accommodationReservationService = app.AccommodationReservationService;
            _reservations = new ObservableCollection<AccommodationReservation>(_accommodationReservationService.GetGeustsReservatonst());
           
            ReservationsDataGrid.ItemsSource = _reservations;

            findWidthForReservationsDataGrid();
        }

        //ovo je da postavi sirinu DataGrida
        public void findWidthForReservationsDataGrid()
        {
            double totalWidth = 0;
            foreach (DataGridColumn column in ReservationsDataGrid.Columns)
            {
                if (column.ActualWidth > 0)
                {
                    totalWidth += column.ActualWidth;
                }
            }
            ReservationsDataGrid.Width = totalWidth;
        }

        private void Button_Click_RateAccommodationAndOwner(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RateAccommodationAndOwner(SelectedReservation));
        }
    }
}
