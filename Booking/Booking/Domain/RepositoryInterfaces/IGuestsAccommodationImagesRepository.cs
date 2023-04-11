using Booking.Model.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.RepositoryInterfaces
{
	public interface IGuestsAccommodationImagesRepository : IRepository<AccommodationImage>
	{
		int NextId();
		void Add(AccommodationImage image);
	}
}
