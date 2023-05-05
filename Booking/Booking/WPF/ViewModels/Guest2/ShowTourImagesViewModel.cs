using Booking.Commands;
using Booking.Model;
using Booking.Model.Images;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Booking.WPF.ViewModels.Guest2
{
	public class ShowTourImagesViewModel
	{
		private readonly Window _window;

		public ObservableCollection<TourImage> Images { get; set; }
		public Tour Tour { get; set; }
		public string Location { get; set; }
		public RelayCommand Button_Click_Close { get; set; }

		public ShowTourImagesViewModel(Window window, Tour tour)
		{
			_window = window;
			Tour = tour;
			Location = tour.Location.State + ", " + tour.Location.City;
			Images = new ObservableCollection<TourImage>(tour.Images);
			Button_Click_Close = new RelayCommand(ButtonClose);
		}
		private void ButtonClose(object param)
		{
			_window.Close();
		}
	}
}
