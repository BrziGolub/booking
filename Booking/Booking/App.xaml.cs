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

        public App()
        {
            AccommodationDAO accommodationDAO = new AccommodationDAO();
            AccommodationController = new AccommodationContoller(accommodationDAO);
        }
    }
}
