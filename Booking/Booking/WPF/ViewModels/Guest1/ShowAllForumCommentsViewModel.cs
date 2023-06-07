using Booking.Domain.Model;
using Booking.Domain.ServiceInterfaces;
using Booking.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.WPF.ViewModels.Guest1
{
    public class ShowAllForumCommentsViewModel
    {
        public ObservableCollection<ForumComment> ForumComments { get; set; }

        public IForumCommentService ForumCommentService;
        public ShowAllForumCommentsViewModel()
        {
           ForumCommentService = InjectorService.CreateInstance<IForumCommentService>();
           ForumComments = new ObservableCollection<ForumComment>(ForumCommentService.GetAll());
        }
    }
}
