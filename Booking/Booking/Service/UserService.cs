using Booking.Domain.RepositoryInterfaces;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Observer;
using Booking.Repository;
using Booking.Util;
using System.Collections.Generic;

namespace Booking.Service
{
	public class UserService : ISubject, IUserService
	{
		private readonly IUserRepository _userRepository;

		private readonly List<IObserver> _observers;

		public UserService()
		{
			_userRepository = InjectorRepository.CreateInstance<IUserRepository>();
			_observers = new List<IObserver>();
		}

		public List<User> GetGuests()
		{
			List<User> users = new List<User>();
			foreach (var user in _userRepository.GetAll())
			{
				if (user.Role == 4)
				{
					users.Add(user);
				}
			}
			return users;
		}

		public User GetByUsername(string username)
		{
			return _userRepository.GetByUsername(username);
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
