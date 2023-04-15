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

namespace Booking.View.Guest2
{
	public partial class ShowVouchers : Window
	{
		private readonly IVoucherService _voucherService;

		private ObservableCollection<Voucher> vouchers;

		public ShowVouchers()
		{
			InitializeComponent();
			DataContext = this;

			_voucherService = InjectorService.CreateInstance<IVoucherService>();

			vouchers = new ObservableCollection<Voucher>(_voucherService.GetVouchersByUserId(TourService.SignedGuideId));

			DejtaGrid.ItemsSource = vouchers;
		}

		private void ButtonClose(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
