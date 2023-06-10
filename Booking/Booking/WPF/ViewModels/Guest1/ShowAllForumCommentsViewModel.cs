using Booking.Application.UseCases;
using Booking.Commands;
using Booking.Domain.Model;
using Booking.Domain.ServiceInterfaces;
using Booking.Observer;
using Booking.Service;
using Booking.Util;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Booking.WPF.ViewModels.Guest1
{
    public class ShowAllForumCommentsViewModel : IObserver
    {
        public ObservableCollection<ForumComment> ForumComments { get; set; }

        public IForumCommentService ForumCommentService;

        public Forum SelectedForum;
        public ForumComment SelectedComment { get; set; }
        public RelayCommand LeaveCommentButton { get; set; }
        public IAccommodationReservationService _AccommodationReservationService { get; set; }
        public IUserService UserService { get; set; }

        public string NewComment { get; set; }

        public ShowAllForumCommentsViewModel(Forum selectedForum)
        {
            ForumCommentService  = InjectorService.CreateInstance<IForumCommentService>();
            _AccommodationReservationService = InjectorService.CreateInstance<IAccommodationReservationService>();
            UserService = InjectorService.CreateInstance<IUserService>();
            SelectedForum = selectedForum;
            ForumCommentService.Subscribe(this);
            ForumComments = new ObservableCollection<ForumComment>(ForumCommentService.GetForumComments(selectedForum));
            LeaveCommentButton = new RelayCommand(LeaveComment);
        }

        public void LeaveComment(object sender)
        {
            if(SelectedForum.Status.Equals("OPENED"))
            {
                ForumComment newForumComment = new ForumComment();
                newForumComment.Comment = NewComment;
                newForumComment.Forum = SelectedForum;
                newForumComment.User = UserService.GetById(AccommodationReservationService.SignedFirstGuestId);
                newForumComment.Reports = 0;
                newForumComment.Visited = _AccommodationReservationService.IsLocationVisited(SelectedForum.Location);
                ForumCommentService.Create(newForumComment);
            }
            else
            {
                var notificationManager = new NotificationManager();
                NotificationContent content = new NotificationContent { Title = "Permission denied!", Message = "This forum is CLOSED, try comment on other forums!", Type = NotificationType.Error };
                notificationManager.Show(content, areaName: "WindowArea", expirationTime: TimeSpan.FromSeconds(5));
            }
           
        }

        public void Update()
        {
            ForumComments.Clear();
            foreach (ForumComment f in ForumCommentService.GetForumComments(SelectedForum))
            {
                ForumComments.Add(f);
            }
        }
    }
}
