﻿using Booking.Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Controller
{
    public class LocationController
    {

        private readonly LocationDAO locationDAO;
        public LocationController() 
        {
            locationDAO = new LocationDAO();
        }
    }
}
