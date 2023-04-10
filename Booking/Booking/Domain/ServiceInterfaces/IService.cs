using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ServiceInterfaces
{
    public interface IService<T>
    {
        void Save();
        void Update(T entity);
        List<T> GetAll();
        T GetById(int id);
    }
}
