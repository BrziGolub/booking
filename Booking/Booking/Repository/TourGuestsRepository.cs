using Booking.Model;
using Booking.Model.Images;
using Booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Repository
{
    public class TourGuestsRepository
    {
        private const string FilePath = "../../Resources/Data/tourGuests.csv";

        private readonly Serializer<TourGuests> _serializer;

        public List<TourGuests> _tourGuests;

        public TourGuestsRepository() 
        {
        _serializer = new Serializer<TourGuests>();
        _tourGuests = Load();
        }

        public List<TourGuests> Load()
        {
            return _serializer.FromCSV(FilePath);
        }
         public void Save(List<TourGuests> tourGuests)
        {
            _serializer.ToCSV(FilePath, tourGuests);
        }
    }
}
