using Booking.Model;
using Booking.Serializer;
using System;
using System.ComponentModel;
using Booking.Observer;
using Booking.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Booking.Domain.ServiceInterfaces;

namespace Booking.Service
{
    public class UserService : IUserService
    {
        private readonly UserRepository _repository;

        private List<User> _users;
        private readonly List<IObserver> _observers;

        public UserService() {
      
            _repository = new UserRepository();
            _observers = new List<IObserver>();
            Load();
        }

        public void Load()
        {
            _users = _repository.Load();
        }
        public List<User> GetGuests()
        {
          //Load();
            List<User> users = new List<User>();
            foreach (var user in _users)
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
            _repository.Save(_users);
        }

        public User GetById(int id)
        {
            return _users.Find(v => v.Id == id);
        }

        public List<User> GetAll()
        {
            return _users;
        }

        public User GetByUsername(string username)
        {
            return _users.Find(u => u.Username == username);
        
    
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
