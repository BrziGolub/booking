using Booking.Model;
using Booking.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ServiceInterfaces
{
    public interface IVoucherService : ISubject,IService<Voucher>
    {
        Voucher AddVoucher(Voucher voucher);
        void Create(Voucher voucher);
        List<Voucher> GetAll();
        Voucher GetById(int id);
        Voucher Update(Voucher voucher);
    }
}
