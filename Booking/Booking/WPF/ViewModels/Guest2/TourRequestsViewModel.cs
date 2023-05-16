using Booking.Commands;
using Booking.Domain.Model;
using Booking.Domain.ServiceInterfaces;
using Booking.Service;
using Booking.Util;
using Booking.View;
using Booking.WPF.Views.Guest2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Booking.WPF.ViewModels.Guest2
{
	public class TourRequestsViewModel
	{
		private readonly Window _window;
		private ITourRequestService _tourRequestService;

		public ObservableCollection<TourRequest> TourRequests { get; set; }

		public RelayCommand Button_Click_Close { get; set; }
		public RelayCommand Button_Click_Request { get; set; }

		public TourRequestsViewModel(Window window)
		{
			_window = window;
			_tourRequestService = InjectorService.CreateInstance<ITourRequestService>();

			TourRequests = new ObservableCollection<TourRequest>(_tourRequestService.GetRequestsByUserId(TourService.SignedGuideId));

			InitializeCommands();
		}

		private void InitializeCommands()
		{
			Button_Click_Close = new RelayCommand(ButtonClose);
			Button_Click_Request = new RelayCommand(ButtonRequest);
		}

		private void ButtonClose(object param)
		{
			_window.Close();
		}

		private void ButtonRequest(object param)
		{
			CreateTourRequestView view = new CreateTourRequestView();
			view.ShowDialog();
		}
	}
}
