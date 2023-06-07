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
    public class ForumCommentService : ISubject, IForumCommentService
    {
		private readonly IForumCommentRepository _forumCommentRepository;
		private readonly List<IObserver> _observers;

		public ForumCommentService()
		{
			_forumCommentRepository = InjectorRepository.CreateInstance<IForumCommentRepository>();
			_observers = new List<IObserver>();
		}

		public List<ForumComment> GetAll()
		{
			return _forumCommentRepository.GetAll();
		}

		public ForumComment GetById(int id)
		{
			return _forumCommentRepository.GetById(id);
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
