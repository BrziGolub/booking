using Booking.Model.DAO;
using Booking.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Booking.Observer;

namespace Booking.Controller
{
    public class LocationController
    {
        

        private readonly LocationDAO locationDAO;
        public LocationController(LocationDAO location) 
        {
            locationDAO = location;
        }

        public void Create(Location location)
        {
            locationDAO.addLocation(location);
        }

        public int NextId()
        {
            return locationDAO.NextId();
        }

        public int GetIdByCountryAndCity(string Country, string City) 
        {
            return locationDAO.GetIdByCountryAndCity(Country, City);
        }

        public Location FindById(int id)
        {
            return locationDAO.FindById(id);
        }
        public void Subscribe(IObserver observer)
        {
            locationDAO.Subscribe(observer);
        }

        public List<Location> findAllLocations()
        {
            return locationDAO.FindAllLocations();
        }

        public int FindIdByCountryAndCity(string Country, string City)
        {
            return locationDAO.FindIdByCountryAndCity(Country, City);
        }


      
    }
}
