using Booking.Model;
using Booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Repository
{
    public class AccommodationAndOwnerGradeRepository
    {
        private const string FilePath = "../../Resources/Data/accommodationAndOwnerGrade.csv";

        private readonly Serializer<AccommodationAndOwnerGrade> _serializer;

        public List<AccommodationAndOwnerGrade> _accommodationsAndOwnerGrades; //sta ce mi ovo?

        public AccommodationAndOwnerGradeRepository()
        {
            _serializer = new Serializer<AccommodationAndOwnerGrade>();
            _accommodationsAndOwnerGrades = Load();
        }

        public List<AccommodationAndOwnerGrade> Load()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(List<AccommodationAndOwnerGrade> grades)
        {
            _serializer.ToCSV(FilePath, grades);
        }

    }
}
