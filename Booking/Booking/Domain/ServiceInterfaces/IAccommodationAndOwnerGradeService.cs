using Booking.Model;
using Booking.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ServiceInterfaces
{
    public interface IAccommodationAndOwnerGradeService : IService<AccommodationAndOwnerGrade>, ISubject
    {
        List<AccommodationAndOwnerGrade> GetSeeableGrades();
        void SaveGrade(AccommodationAndOwnerGrade grade);
    }
}
