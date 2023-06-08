using Booking.Commands;
using Booking.Domain.Model;
using Booking.Domain.ServiceInterfaces;
using Booking.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;

namespace Booking.WPF.ViewModels.Guest1
{
    public class ShowAllForumCommentsViewModel
    {
        public ObservableCollection<ForumComment> ForumComments { get; set; }

        public IForumCommentService ForumCommentService;

        public Forum SelectedForum;
        public ForumComment SelectedComment { get; set; } //nez hoce trebati ovo neka stoji :)
        public RelayCommand LeaveCommentButton { get; set; }

        public ShowAllForumCommentsViewModel(Forum selectedForum)
        {
           ForumCommentService = InjectorService.CreateInstance<IForumCommentService>();
           SelectedForum = selectedForum;
           ForumComments = new ObservableCollection<ForumComment>(ForumCommentService.GetForumComments(selectedForum));
           LeaveCommentButton = new RelayCommand(LeaveComment);
        }

        public void LeaveComment(object sender)
        {
            ForumComment newForumComment = new ForumComment();
            //ovde napravi komentar za sleecetd forum
        }
    }
}
