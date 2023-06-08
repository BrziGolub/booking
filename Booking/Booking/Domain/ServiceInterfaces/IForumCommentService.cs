using Booking.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ServiceInterfaces
{
    public interface IForumCommentService
    {
        List<ForumComment> GetAll();
        ForumComment GetById(int id);
    }
}
