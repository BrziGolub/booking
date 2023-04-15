using Booking.Model;
using Booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Model
{
    public class Notification : ISerializable
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string Text { get; set; }
        public bool IsRead { get; set; }

        public Notification()
        {
            User = new User();
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            User.Id = int.Parse(values[1]);
            Text = values[2];
            IsRead = bool.Parse(values[3]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                User.Id.ToString(),
                Text,
                IsRead.ToString()
            };
            return csvValues;
        }
    }
}
