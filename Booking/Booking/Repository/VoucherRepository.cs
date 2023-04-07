using Booking.Model;
using Booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Repository
{
    public class VoucherRepository
    {
        private const string FilePath = "../../Resources/Data/vouchers.csv";

        private readonly Serializer<Voucher> _serializer;

        public VoucherRepository()
        {
            _serializer = new Serializer<Voucher>();
        }

        public List<Voucher> Load()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(List<Voucher> vouchers)
        {
            _serializer.ToCSV(FilePath, vouchers);
        }

    }
}
