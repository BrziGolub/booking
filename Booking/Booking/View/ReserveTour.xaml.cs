﻿using Booking.Model.DAO;
using Booking.Controller;
using System;
using System.Collections.Generic;
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
using Booking.Model;
using Booking.Service;

namespace Booking.View
{
	public partial class ReserveTour : Window
	{
		private TourReservationService _tourReservationService;

		public int NumberOfPassengers { get; set; }
		public Tour Tour { get; set; }

		public ReserveTour(Tour tour)
		{
			InitializeComponent();
			DataContext = this;

			_tourReservationService = new TourReservationService();

			Tour = tour;
		}

		private void Reserve(object sender, RoutedEventArgs e)
		{
			int availability = _tourReservationService.CheckAvailability(Tour.Id);
			if (NumberOfPassengers > 0)
			{
				if (availability >= NumberOfPassengers)
				{
					_tourReservationService.CreateTourReservation(Tour, NumberOfPassengers);
					MessageBox.Show("Tour created");
					Close();
				}
				else if (availability < NumberOfPassengers && availability > 0)
				{
					MessageBox.Show("Not enough available space, please choose another option or tour");
					LabelRemainingSpace.Content = "Available space left: " + availability.ToString();
				}
				else
				{
					MessageBox.Show("Tour is already full");
					// Function call: calls method from parent window
					((SecondGuestHomePage)this.Owner).TourSearch(Tour.Location.State, Tour.Location.City, "", "", "");
					Close();
				}
			}
		}

		private void Cancel(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}