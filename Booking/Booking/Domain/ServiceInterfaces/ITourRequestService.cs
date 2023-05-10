﻿using Booking.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ServiceInterfaces
{
    public interface ITourRequestService : IService<TourRequest>
    {
        string GetMostPopularLanguageInLastYear();
        int GetMostPopularLocationIdInLastYear();
    }
}