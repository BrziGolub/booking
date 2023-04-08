using Booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Model
{
    public class Voucher : ISerializable
    {

        public int Id { get; set; }
        public User User { get; set; }
        //public Tour Tour { get; set; }
        public DateTime ValidTime { get; set; }
        public Voucher() {
            User = new User();
            //Tour = new Tour();
        }

        public string[] ToCSV()
        {
            string[] csvValues = {
                Id.ToString(),
                User.Id.ToString(),
                //Tour.Id.ToString(),
                ValidTime.ToString("dd/MM/yyyy"),

            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            User.Id = Convert.ToInt32(values[1]);
            //Tour.Id = Convert.ToInt32(values[2]);
            ValidTime = DateTime.Parse(values[2]);
        }
    }
}
