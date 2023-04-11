using Booking.Domain.RepositoryInterfaces;
using Booking.Model;
using Booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Repository
{
    public class AccommodationReservationRequestsRepostiory : IAccommodationReservationRequestsRepostiory
    {

        private const string FilePath = "../../Resources/Data/accommodationsReservationsRequests.csv";

        private readonly Serializer<AccommodationReservationRequests> _serializer;

        public List<AccommodationReservationRequests> _accommodationsReservationsRequests;

        public AccommodationReservationRequestsRepostiory()
        {
            _serializer = new Serializer<AccommodationReservationRequests>();
            _accommodationsReservationsRequests = _serializer.FromCSV(FilePath);
		}

        public List<AccommodationReservationRequests> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

		public AccommodationReservationRequests GetById(int id)
		{
			return _accommodationsReservationsRequests.Find(a => a.Id == id);
		}
	}
}
