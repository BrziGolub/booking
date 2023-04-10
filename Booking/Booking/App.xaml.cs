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
<<<<<<< Updated upstream
        public AccommodationAndOwnerGradeService AccommodationAndOwnerGradeService { get; set; }
=======
        public AccommodationReservationRequestService AccommodationReservationRequestService { get; set; }
>>>>>>> Stashed changes

        public App()
        {
            UserService = new UserService();
            LocationService = new LocationService();
            AccommodationService = new AccommodationService();
            AccommodationReservationService = new AccommodationReservationService();
<<<<<<< Updated upstream
            AccommodationGradeService = new AccommodationGradeService();
            TourService = new TourService();
            TourGuestsService = new TourGuestsService();
            AccommodationAndOwnerGradeService = new AccommodationAndOwnerGradeService();
=======
            AccommodationReservationRequestService = new AccommodationReservationRequestService();
            TourService = new TourService();
            TourGuestsService = new TourGuestsService();
            AccommodationReservationService.AccommodationReservationRequestService = AccommodationReservationRequestService;
>>>>>>> Stashed changes
        }
    }
}
