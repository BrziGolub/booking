using Booking.Model;
using Booking.Observer;
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
using System.Windows.Shapes;

namespace Booking.View
{
    /// <summary>
    /// Interaction logic for OwnerHomePage.xaml
    /// </summary>
    public partial class OwnerHomePage : Window, IObserver
    {
        //public AccommodationReservationController accommodationReservationController;
        public AccommodationReservationService AccommodationReservationService { get; set; }
        public ObservableCollection<AccommodationReservation> Reservations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public AccommodationService _accommodationService { get; set; }
        public OwnerHomePage()
        {
            InitializeComponent();
            this.DataContext = this;
            _accommodationService = new AccommodationService();
            _accommodationService.Subscribe(this);
            var app = Application.Current as App;
            AccommodationReservationService = app.AccommodationReservationService;
            Reservations = new ObservableCollection<AccommodationReservation>(AccommodationReservationService.GetAllUngradedReservations());
            Accommodations = new ObservableCollection<Accommodation>(_accommodationService.GetOwnerAccommodations());
            if (Reservations.Count != 0)
            {
                MessageBox.Show("Number of guests to grade: " + Reservations.Count.ToString());
            }
        }

        private void RegisterAccomodation(object sender, RoutedEventArgs e)
        {
            OwnerRegisterAccommodation ownerRegisterAccommodation = new OwnerRegisterAccommodation();
            ownerRegisterAccommodation.Show();
        }
        private void ViewReviews(object sender, RoutedEventArgs e)
        {
            OwnerViewReviews ownerViewReviews = new OwnerViewReviews();
            ownerViewReviews.Show();
        }

        private void GradingGuests(object sender, RoutedEventArgs e)
        {
            OwnerGradingGuests ownerGradingGuests = new OwnerGradingGuests();
            ownerGradingGuests.Show();
        }
        public void Update()
        {
            Accommodations.Clear();

            foreach (Accommodation a in _accommodationService.GetOwnerAccommodations())
            {
                Accommodations.Add(a);
            }
        }
        private void OpenPictures(object sender, RoutedEventArgs e)
        {
        }
    }
}
