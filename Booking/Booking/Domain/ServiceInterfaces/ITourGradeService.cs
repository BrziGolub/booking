using Booking.Domain.Model;
using Booking.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ServiceInterfaces
{
    public interface ITourGradeService : ISubject,IService<TourGrade>
    {
        List<TourGrade> GetAll();
        TourGrade GetById(int id);
        List<TourGrade> GetGuideGrades();
    }
}
