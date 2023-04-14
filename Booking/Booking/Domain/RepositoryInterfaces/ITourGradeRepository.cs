using Booking.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.RepositoryInterfaces
{
    public interface ITourGradeRepository : IRepository<TourGrade>
    {
        int NextId();
        TourGrade Update(TourGrade tourGrade);
    }
}
