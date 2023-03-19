using Booking.Model;
using Booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Repository
{
    public class AccommodationGradeRepository
    {
        private const string FilePath = "../../Resources/Data/accommodationGrades.csv";

        private readonly Serializer<AccommodationGrade> _serializer;

        public List<AccommodationGrade> _accommodationsGrades;

        public AccommodationGradeRepository()
        {
            _serializer = new Serializer<AccommodationGrade>();
            _accommodationsGrades = Load();
        }

        public List<AccommodationGrade> Load()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(List<AccommodationGrade> accommodationsGrades)
        {
            _serializer.ToCSV(FilePath, accommodationsGrades);
        }
    }
}
