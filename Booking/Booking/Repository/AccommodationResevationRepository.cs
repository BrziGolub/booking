using Booking.Domain.RepositoryInterfaces;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Repository
{
    public class AccommodationResevationRepository : IAccommodationResevationRepository
    {
        private const string FilePath = "../../Resources/Data/accommodationsReservations.csv";

        private readonly Serializer<AccommodationReservation> _serializer;

        public List<AccommodationReservation> _accommodations;

        public AccommodationResevationRepository()
        {
            _serializer = new Serializer<AccommodationReservation>();
            _accommodations = _serializer.FromCSV(FilePath);
		}

        public List<AccommodationReservation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

		public AccommodationReservation GetById(int id)
		{
			return _accommodations.Find(a => a.Id == id);
		}
	}
}
