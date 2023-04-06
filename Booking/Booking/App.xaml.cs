using Booking.Model;
using Booking.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Booking
{
    public partial class App : Application
    {
        public UserService UserService { get; set; }
        public LocationService LocationService { get; set; }
        public AccommodationService AccommodationService { get; set; }
        public AccommodationGradeService AccommodationGradeService { get; set; }
        public AccommodationReservationService AccommodationReservationService { get; set; }
        public TourService TourService { get; set; }
        public TourGuestsService TourGuestsService { get; set; }

        public App()
        {
            UserService = new UserService();
            LocationService = new LocationService();
            AccommodationService = new AccommodationService();
            AccommodationGradeService = new AccommodationGradeService();
            AccommodationReservationService = new AccommodationReservationService();
            TourService = new TourService();
            TourGuestsService = new TourGuestsService();
        }
    }
}
