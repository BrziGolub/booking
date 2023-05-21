using Booking.Commands;
using Booking.Domain.Model;
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

namespace Booking.WPF.ViewModels.Guest2
{
	public class TourNotificationViewModel
	{
		private readonly Window _window;
		private readonly ITourNotificationService _notificationService;

		public ObservableCollection<TourNotification> Notifications { get; set; }

		public TourNotification SelectedNotification { get; set; }

		public RelayCommand Button_Click_Close { get; set; }
		public RelayCommand Button_Click_ShowTour { get; set; }

		public TourNotificationViewModel(Window window)
		{
			_window = window;
			_notificationService = InjectorService.CreateInstance<ITourNotificationService>();

			Notifications = new ObservableCollection<TourNotification>(_notificationService.GetByUserId(TourService.SignedGuideId));

			InitializeCommands();
		}

		private void InitializeCommands()
		{
			Button_Click_Close = new RelayCommand(ButtonClose);
			Button_Click_ShowTour = new RelayCommand(ButtonShowTour);
		}

		private void ButtonClose(object param)
		{
			_window.Close();
		}

		private void ButtonShowTour(object param)
		{
			string message = SelectedNotification.Tour.Name + "\n"
				+ SelectedNotification.Tour.Location.State + ", " + SelectedNotification.Tour.Location.City + "\n"
				+ SelectedNotification.Tour.Language + "\n"
				+ SelectedNotification.Tour.StartTime + "\n"
				+ SelectedNotification.Tour.Duration + "\n"
				+ SelectedNotification.Tour.Description;
			MessageBox.Show(message);
		}
	}
}
