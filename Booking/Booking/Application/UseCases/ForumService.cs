using Booking.Domain.Model;
using Booking.Domain.RepositoryInterfaces;
using Booking.Domain.ServiceInterfaces;
using Booking.Observer;
using Booking.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.UseCases
{
    public class ForumService : ISubject, IForumService
	{
		private readonly IForumRepository _forumRepository;
		private readonly List<IObserver> _observers;

		public ForumService()
		{
			_forumRepository = InjectorRepository.CreateInstance<IForumRepository>();
			_observers = new List<IObserver>();
		}

		public List<Forum> GetAll()
		{
			return _forumRepository.GetAll();
		}

		public Forum GetById(int id)
		{
			return _forumRepository.GetById(id);
		}

		public void Subscribe(IObserver observer)
		{
			_observers.Add(observer);
		}
		public void Unsubscribe(IObserver observer)
		{
			_observers.Remove(observer);
		}
		public void NotifyObservers()
		{
			foreach (var observer in _observers)
			{
				observer.Update();
			}
		}
	}
}
