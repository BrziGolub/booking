using Booking.Model;
using Booking.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ServiceInterfaces
{
    public interface IUserService : IService<User>
    {
        void Load();
        List<User> GetGuests();
        User GetByUsername(string username);
        void NotifyObservers();
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
    }
}
