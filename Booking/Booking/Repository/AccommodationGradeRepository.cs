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
    public class AccommodationGradeRepository : IAccommodationGradeRepository
    {
        private const string FilePath = "../../Resources/Data/accommodationGrades.csv";

        private readonly Serializer<AccommodationGrade> _serializer;

        public List<AccommodationGrade> _accommodationsGrades;

        public AccommodationGradeRepository()
        {
            _serializer = new Serializer<AccommodationGrade>();
            _accommodationsGrades = _serializer.FromCSV(FilePath);
		}

        public List<AccommodationGrade> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

		public AccommodationGrade GetById(int id)
		{
			return _accommodationsGrades.Find(a => a.Id == id);
		}
	}
}
