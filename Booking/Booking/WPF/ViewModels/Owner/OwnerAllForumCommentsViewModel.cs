using Booking.Commands;
using Booking.Domain.Model;
using Booking.Domain.ServiceInterfaces;
using Booking.Observer;
using Booking.Service;
using Booking.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Booking.WPF.ViewModels.Owner
{
    public class OwnerAllForumCommentsViewModel : IObserver, INotifyPropertyChanged
    {
        public ObservableCollection<ForumComment> ForumComments { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public IForumCommentService ForumCommentService;
        public ILocationService LocationService;
        public String ForumLabel { get; set; } = String.Empty;

        public Forum SelectedForum;
        public ForumComment SelectedComment { get; set; } //nez hoce trebati ovo neka stoji :)
        public RelayCommand LeaveComment { get; set; }
        public RelayCommand Close { get; set; }
        private readonly Window _window;

        public OwnerAllForumCommentsViewModel(Forum selectedForum, Window window)
        {
            _window = window;
            ForumCommentService = InjectorService.CreateInstance<IForumCommentService>();
            LocationService = InjectorService.CreateInstance<ILocationService>();
            SelectedForum = selectedForum;
            ForumLabel = SetForumLabel(SelectedForum);
            ForumCommentService.Subscribe(this);
            ForumComments = new ObservableCollection<ForumComment>(ForumCommentService.GetForumComments(selectedForum));
            LeaveComment = new RelayCommand(LeaveCommentButton);
            Close = new RelayCommand(CloseWindow);
            OwnerComment = "";
        }
        public string _ownerComment;
        public string OwnerComment
        {
            get => _ownerComment;
            set
            {
                if (_ownerComment != value)
                {
                    _ownerComment = value;
                    OnPropertyChanged();
                }
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void Update()
        {
            ForumComments.Clear();

            foreach (ForumComment forumComment in ForumCommentService.GetForumComments(SelectedForum))
            {
                ForumComments.Add(forumComment);
            }
        }

        public void LeaveCommentButton(object sender)
        {
            if (SelectedForum.Status.Equals("CLOSED"))
            {
                MessageBox.Show("The forum is closed");
                return;
            }
            if (!LocationService.DoesOwnerHaveLocation(SelectedForum.Location.Id))
            {
                MessageBox.Show("You can not comment on this forum");
                return;
            }
            ForumComment newForumComment = new ForumComment();
            newForumComment.Forum = SelectedForum;
            newForumComment.User.Id = AccommodationService.SignedOwnerId;
            newForumComment.Reports = 0;
            newForumComment.Comment = OwnerComment;
            newForumComment.Visited = "OWNER";
            ForumCommentService.Create(newForumComment);
            ForumCommentService.NotifyObservers();
            MessageBox.Show("Comment succesfully added");
        }
        private String SetForumLabel(Forum forum)
        {
            return forum.User.Username + " for location:" + forum.Location.State + "-" + forum.Location.City;
        }
        private void CloseWindow(object param)
        {
            _window.Close();
        }
    }
}
