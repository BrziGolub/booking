using Booking.Domain.RepositoryInterfaces;
using Booking.Model;
using Booking.Model.Images;
using Booking.Serializer;
using Booking.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Repository
{
    public class TourGuestsRepository : ITourGuestsRepository
    {
        private const string FilePath = "../../Resources/Data/tourGuests.csv";

        private readonly Serializer<TourGuests> _serializer;

        public List<TourGuests> _tourGuests;

        public TourGuestsRepository() 
        {
            _serializer = new Serializer<TourGuests>();
            _tourGuests = _serializer.FromCSV(FilePath);
		}		

		public List<TourGuests> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

		public TourGuests GetById(int id)
		{
			return null;
		}

		public TourGuests Add(TourGuests tourGuest)
		{
            _tourGuests = _serializer.FromCSV(FilePath);
            _tourGuests.Add(tourGuest);
			_serializer.ToCSV(FilePath, _tourGuests);
			return tourGuest;
		}

		public void DeleteByTourId(int id)
		{
			_tourGuests.RemoveAll(TourKeyPoint => TourKeyPoint.Tour.Id == id);
			_serializer.ToCSV(FilePath, _tourGuests);
		}
	}
}
