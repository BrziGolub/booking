﻿using Booking.Model.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ServiceInterfaces
{
    public interface IGuestsAccommodationImagesService : IService<AccommodationImage>
    {
        void Load();
        int GenerateId();
        void Create(AccommodationImage image);
    }
}
