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
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Service;
using Booking.Util;

namespace Booking.View
{
	public partial class ReserveTour : Window
	{
        private ITourReservationService _tourReservationService;
		private IVoucherService _voucherService;

		private ObservableCollection<Voucher> vouchers;

		public int NumberOfPassengers { get; set; }

		public Tour Tour { get; set; }

		public Voucher SelectedVoucher { get; set; }

		public bool IsVoucherChecked
		{
			get { return (bool)GetValue(IsCheckBoxCheckedProperty); }
			set { SetValue(IsCheckBoxCheckedProperty, value); }
		}

		public static readonly DependencyProperty IsCheckBoxCheckedProperty =
			DependencyProperty.Register("IsCheckBoxChecked", typeof(bool),
			typeof(ReserveTour), new UIPropertyMetadata(false));

		public ReserveTour(Tour tour)
		{
			InitializeComponent();
			DataContext = this;

			_tourReservationService = InjectorService.CreateInstance<ITourReservationService>();
			_voucherService = InjectorService.CreateInstance<IVoucherService>();

			Tour = tour;
			vouchers = new ObservableCollection<Voucher>(_voucherService.GetVouchersByUserId(TourService.SignedGuideId));

			DejtaGrid.ItemsSource = vouchers;
		}

		private void Reserve(object sender, RoutedEventArgs e)
		{
			int availability = _tourReservationService.CheckAvailability(Tour.Id);
			if (NumberOfPassengers > 0)
			{
				if (availability >= NumberOfPassengers)
				{
					if (IsVoucherChecked)
					{
						if (SelectedVoucher == null)
						{
							MessageBox.Show("Voucher is not selected!");
						}
						else
						{
							ReserveATourWithVoucher();
						}
					}
					else
					{
						ReserveATour();
					}
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
					((SecondGuestHomePage)this.Owner).ReserveTourSearch(Tour.Location.State, Tour.Location.City, Tour.Id);
					Close();
				}
			}
		}

		private void ReserveATour()
		{
			_tourReservationService.CreateTourReservation(Tour, NumberOfPassengers);
			MessageBox.Show("Tour reserved");
			Close();
		}

		private void ReserveATourWithVoucher()
		{
			MessageBox.Show(SelectedVoucher.Id.ToString() + " is selected");
			_voucherService.RedeemVoucher(SelectedVoucher);
			ReserveATour();
		}

		private void Cancel(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
