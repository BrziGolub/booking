using Booking.Model;
using Booking.Observer;
using Booking.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Service
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private List<User> _users1;

        private readonly List<IObserver> _observers;


        public UserService() 
        {
            _userRepository = new UserRepository();
            _users1 = new List<User>();
            _observers = new List<IObserver>();

            Load();
        }

        public void Load()
        {
            _users1 = _userRepository.Load();
        }
        public List<User> GetGuests()
        {
          //Load();
            List<User> users = new List<User>();
            foreach (var user in _users1)
            {
                if (user.Role == 4)
                {
                    users.Add(user);
                }
            }
            return users;
        }

        public void Save()
        {
            _userRepository.Save(_users1);
        }

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }

        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _observers.Remove(observer);
        }

    }
}
