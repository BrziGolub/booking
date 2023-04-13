using System;
using System.Collections.Generic;
using System.Linq;
using Booking.Serializer;
using System.Text;
using System.Threading.Tasks;
using Booking.Model;

namespace Booking.Domain.Model
{
    public class TourGrade : ISerializable
    {
        public int Id { get; set; }
        public User Guide { get; set;}
        public User Guest { get; set;}
        public int KnowledgeGuideGrade { get; set; }
        public int LanguageGuideGrade { get; set; }
        public int InterestOfTourGrade { get; set; }
        public string Comment { get; set; }
        public bool Valid { get; set; }

        public TourGrade()
        {
            Guide = new User();
            Guest = new User();
            Valid = true;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Guide.Id = Convert.ToInt32(values[1]);
            Guest.Id = Convert.ToInt32(values[2]);
            KnowledgeGuideGrade = Convert.ToInt32(values[3]);
            LanguageGuideGrade = Convert.ToInt32(values[4]);
            InterestOfTourGrade = Convert.ToInt32(values[5]);
            Comment = Convert.ToString(values[6]);
            Valid = Convert.ToBoolean(values[7]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
             {
                Id.ToString(),                  //0
                Guide.Id.ToString(),            //1
                Guest.Id.ToString(),            //2
                KnowledgeGuideGrade.ToString(), //3
                LanguageGuideGrade.ToString(),  //4
                InterestOfTourGrade.ToString(), //5
                Comment,                        //6
                Valid.ToString()                //7
            };
            return csvValues;
        }
    }
}
