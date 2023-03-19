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
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public TourController TourController { get; set; }
        public LocationController LocationController { get; set; }
        public AccommodationContoller AccommodationController { get; set; }

        public TourService TourService { get; set; }
        public LocationService LocationService { get; set; }

        public App()
        {
            LocationDAO locationDAO = new LocationDAO();
            LocationController = new LocationController(locationDAO);
            TourDAO tourDAO = new TourDAO();
            TourController = new TourController(tourDAO);
            AccommodationDAO accommodationDAO = new AccommodationDAO();
            AccommodationController = new AccommodationContoller(accommodationDAO);

            TourService =  new TourService();
            LocationService = new LocationService();
        }

        

    }
}
