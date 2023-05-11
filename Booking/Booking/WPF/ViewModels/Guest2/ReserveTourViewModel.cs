using Booking.Commands;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Service;
using Booking.Util;
using Booking.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Booking.WPF.ViewModels.Guest2
{
    public class ReserveTourViewModel
    {
        private readonly Window _window;
        private SecondGuestHomePageViewModel Mrk;
        private ITourReservationService _tourReservationService;
        private IVoucherService _voucherService;

        private int _availability;

        public static readonly DependencyProperty IsCheckBoxCheckedProperty =
            DependencyProperty.Register("IsCheckBoxChecked", typeof(bool),
            typeof(ReserveTour), new UIPropertyMetadata(false));

        public bool IsVoucherChecked
        {
            get { return (bool)_window.GetValue(IsCheckBoxCheckedProperty); }
            set { _window.SetValue(IsCheckBoxCheckedProperty, value); }
        }
        public ObservableCollection<Voucher> Vouchers { get; set; }
        public int NumberOfPassengers { get; set; }
        public Tour Tour { get; set; }
        public Voucher SelectedVoucher { get; set; }
        public string Label { get; set; }
        public RelayCommand Button_Click_Reserve { get; set; }
        public RelayCommand Button_Click_Close { get; set; }

        public ReserveTourViewModel(Window window, Tour tour, SecondGuestHomePageViewModel mrk)
        {
            _window = window;
            Tour = tour;
            Mrk = mrk;

            _tourReservationService = InjectorService.CreateInstance<ITourReservationService>();
            _voucherService = InjectorService.CreateInstance<IVoucherService>();

            Vouchers = new ObservableCollection<Voucher>(_voucherService.GetVouchersByUserId(TourService.SignedGuideId));

            _availability = _tourReservationService.CheckAvailability(Tour.Id);

            Label = "Available space left: " + _availability.ToString();

            Button_Click_Reserve = new RelayCommand(Reserve);
            Button_Click_Close = new RelayCommand(Cancel);
        }

        private void Reserve(object param)
        {
            if (NumberOfPassengers > 0)
            {
                if (_availability >= NumberOfPassengers)
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
                else if (_availability < NumberOfPassengers && _availability > 0)
                {
                    MessageBox.Show("Not enough available space, please choose another option or tour");
                }
                else
                {
                    MessageBox.Show("Tour is already full");
                    // Function call: calls method from parent window
                    Mrk.ReserveTourSearch(Tour.Location.State, Tour.Location.City, Tour.Id);
                    CloseWindow();
                }
            }
        }

        private void ReserveATour()
        {
            _tourReservationService.CreateTourReservation(Tour, NumberOfPassengers);
            MessageBox.Show("Tour reserved");
            CloseWindow();
        }

        private void ReserveATourWithVoucher()
        {
            _voucherService.RedeemVoucher(SelectedVoucher);
            ReserveATour();
        }

        private void Cancel(object sender)
        {
            CloseWindow();
        }

        private void CloseWindow()
        {
            _window.Close();
        }
    }
}
