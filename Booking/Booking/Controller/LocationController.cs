using Booking.Model.DAO;
using Booking.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Booking.Controller
{
    public class LocationController
    {
        

        private readonly LocationDAO locationDAO;
        public LocationController() 
        {
            locationDAO = new LocationDAO();
        }

        public void Create(Location location)
        {
            locationDAO.addLocation(location);
        }

    }
}
