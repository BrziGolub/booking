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
        List<User> GetGuests();
        User GetByUsername(string username);
    }
}
