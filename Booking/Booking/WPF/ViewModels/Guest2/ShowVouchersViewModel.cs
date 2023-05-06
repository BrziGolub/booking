using Booking.Commands;
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
	public class ShowVouchersViewModel
	{
		private readonly Window _window;
		private readonly IVoucherService _voucherService;

		public ObservableCollection<Voucher> Vouchers { get; set; }
		public RelayCommand Button_Click_Close { get; set; }

		public ShowVouchersViewModel(Window window)
		{
			_window = window;
			_voucherService = InjectorService.CreateInstance<IVoucherService>();

			Vouchers = new ObservableCollection<Voucher>(_voucherService.GetVouchersByUserId(TourService.SignedGuideId));
			Button_Click_Close = new RelayCommand(ButtonClose);
		}

		private void ButtonClose(object param)
		{
			_window.Close();
		}
	}
}
