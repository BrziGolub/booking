using Booking.Domain.Model;
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
    public class TourGradeRepository : ITourGradeRepository
    {
        private const string FilePath = "../../Resources/Data/tourGrades.csv";
        public List<TourGrade> _tourGrades;
        private readonly Serializer<TourGrade> _serializer;

        public TourGradeRepository()
        {
            _serializer = new Serializer<TourGrade>();
            _tourGrades = new List<TourGrade>();
        }
        public List<TourGrade> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
        public TourGrade GetById(int id)
        {
           return _tourGrades.Find(tg => tg.Id == id);
        }

        public int NextId()
        {
            if (_tourGrades.Count == 0)
            {
                return 1;
            }
            else
            {
                return _tourGrades.Max(t => t.Id) + 1;
            }
        }
    }
}
