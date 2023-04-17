using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Observer;
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

namespace Booking.View
{
    /// <summary>
    /// Interaction logic for OwnerDateMove.xaml
    /// </summary>
    public partial class OwnerDateMove : Window, IObserver
    {
        public ObservableCollection<AccommodationReservationRequests> Requests { get; set; }
        public AccommodationReservationRequests SelectedAccommodationReservationRequest { get; set; }
        public IAccommodationReservationRequestService AccommodationReservationRequestService { get; set; }
        public OwnerDateMove()
        {
            InitializeComponent();
            this.DataContext = this;
            AccommodationReservationRequestService = InjectorService.CreateInstance<IAccommodationReservationRequestService>();

            AccommodationReservationRequestService.Subscribe(this);
            Requests = new ObservableCollection<AccommodationReservationRequests>(AccommodationReservationRequestService.GetSeeableDateChanges());
        }

        public void Update()
        {
            Requests.Clear();

            foreach (AccommodationReservationRequests request in AccommodationReservationRequestService.GetSeeableDateChanges())
            {
                Requests.Add(request);
            }
        }
        private void Button_Click_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Button_Click_Reject(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Button_Click_Accept(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
