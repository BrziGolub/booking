using Booking.Controller;
using Booking.Model.DAO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Booking
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public TourController TourController { get; set; }
        public LocationController LocationController { get; set; }
        public AccommodationContoller AccommodationController { get; set; }
        public AccommodationGradeController AccommodationGradeController { get; set; }
        public AccommodationReservationController AccommodationReservationController { get; set; }
        public App()
        {
            LocationDAO locationDAO = new LocationDAO();
            LocationController = new LocationController(locationDAO);
            TourDAO tourDAO = new TourDAO();
            TourController = new TourController(tourDAO);
            AccommodationDAO accommodationDAO = new AccommodationDAO();
            AccommodationController = new AccommodationContoller(accommodationDAO);
            AccommodationGradeDAO accommodationGradeDAO = new AccommodationGradeDAO();
            AccommodationGradeController = new AccommodationGradeController(accommodationGradeDAO);
            AccommodationReservationDAO accommodationReservationDAO = new AccommodationReservationDAO();
            AccommodationReservationController = new AccommodationReservationController(accommodationReservationDAO);
        }

        

    }
}
