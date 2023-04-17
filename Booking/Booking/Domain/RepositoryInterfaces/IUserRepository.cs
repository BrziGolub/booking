using Booking.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.RepositoryInterfaces
{
	public interface IUserRepository : IRepository<User>
	{
		User GetByUsername(string username);
		void Save(List<User> users); // mozda malo drugaciji save
	}
}
