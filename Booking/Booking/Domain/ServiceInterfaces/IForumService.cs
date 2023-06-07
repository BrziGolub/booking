﻿using Booking.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ServiceInterfaces
{
    public interface IForumService : IService<Forum>
    {
        List<Forum> GetAll();
        Forum GetById(int id);
        void Create(Forum forum);
    }
}
