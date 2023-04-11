using Booking.Model;
using Booking.Observer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ServiceInterfaces
{
    public interface ILocationService : IService<Location>
    {
        List<string> GetAllStates();
        ObservableCollection<string> GetAllCitiesByState(ObservableCollection<string> observe, string state);
        int GetIdByCountryAndCity(string Country, string City);
        List<Location> GetAll();
        Location GetById(int id);
    }
}
