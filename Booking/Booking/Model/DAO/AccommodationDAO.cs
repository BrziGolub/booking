using Booking.Observer;
using Booking.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Model.DAO
{
    public class AccommodationDAO : ISubject
    {
        //Add observer?

        private readonly List<IObserver> observers;
        private readonly AccommodationRepository _accommodationRepository;
        private List<Accommodation> accommodations;

        private LocationDAO locationDAO;

        public AccommodationDAO()
        {
            _accommodationRepository = new AccommodationRepository();
            observers = new List<IObserver>();
            accommodations = new List<Accommodation>();
            locationDAO = new LocationDAO();
            Load();
        }

        public void Load()
        {
            accommodations = _accommodationRepository.Load();
        }

        public Accommodation GetByID(int id)
        {
            return accommodations.Find(accommodation => accommodation.Id == id);
        }

        public List<Accommodation> GetAll()
        {
            return accommodations;
        }

        public void Subscribe(IObserver observer)
        {
            observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach (var observer in observers)
            {
                observer.Update();
            }
        }

    }
}
