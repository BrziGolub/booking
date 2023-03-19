using Booking.Controller;
using Booking.Model.DAO;
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
        public LocationController LocationController { get; set; }
        public AccommodationContoller AccommodationController { get; set; }
        public AccommodationGradeController AccommodationGradeController { get; set; }
        public AccommodationReservationController AccommodationReservationController { get; set; }

        public TourService TourService { get; set; }

        public App()
        {
            AccommodationDAO accommodationDAO = new AccommodationDAO();
            AccommodationController = new AccommodationContoller(accommodationDAO);
            AccommodationGradeDAO accommodationGradeDAO = new AccommodationGradeDAO();
            AccommodationGradeController = new AccommodationGradeController(accommodationGradeDAO);
            AccommodationReservationDAO accommodationReservationDAO = new AccommodationReservationDAO();
            AccommodationReservationController = new AccommodationReservationController(accommodationReservationDAO);

            TourService = new TourService();
        }
    }
}
