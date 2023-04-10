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
        void Load();
        int NextId();
        List<string> GetAllStates();
        ObservableCollection<string> GetAllCitiesByState(ObservableCollection<string> observe, string state);
        int GetIdByCountryAndCity(string Country, string City);
        Location GetByCountryAndCity(string Name);
        Location AddLocation(Location location);
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
        void NotifyObservers();
    }
}
