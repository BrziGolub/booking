﻿using Booking.Model;
using Booking.Model.Images;
using Booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace Booking.Domain.Model
{
    public class TourRequest : ISerializable
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } // ovo ti je kada je kreiran pri kreiranju samo stavi za njega DateTime.Now;
        public Location Location { get; set; }
        public string Description { get;set; }
        public string Language { get; set; }
        public int GuestsNumber{ get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public TourRequest()
        {
            Location = new Location();
        }

        public string[] ToCSV()
        {
            string[] csvValues = {
                Id.ToString(),                          //0
                CreatedDate.ToString("dd/MM/yyyy"),     //1
                Location.Id.ToString(),                 //2
                Description,                            //3
                Language,                               //4
                GuestsNumber.ToString(),			    //5
                StartTime.ToString("dd/MM/yyyy"),		//6
                EndTime.ToString("dd/MM/yyyy")          //7
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            CreatedDate = DateTime.Parse(values[1]);
            Location.Id = Convert.ToInt32(values[2]);
            Description = values[3];
            Language = values[4];
            GuestsNumber = Convert.ToInt32(values[5]);
            StartTime = DateTime.Parse(values[6]);
            EndTime = DateTime.Parse(values[7]);
        }
    }
}