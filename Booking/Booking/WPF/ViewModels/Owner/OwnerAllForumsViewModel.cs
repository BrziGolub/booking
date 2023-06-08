using Booking.Commands;
using Booking.Domain.Model;
using Booking.Domain.ServiceInterfaces;
using Booking.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Booking.WPF.ViewModels.Owner
{
    public class OwnerAllForumsViewModel
    {
        public ObservableCollection<Forum> Forums { get; set; }
        public IForumService ForumService { get; set; }
        public Forum SelectedForum { get; set; }
        public RelayCommand ShowComments { get; set; }
        public RelayCommand Close { get; set; }
        private readonly Window _window;

        public OwnerAllForumsViewModel(Window window)
        {
            _window = window;
            ForumService = InjectorService.CreateInstance<IForumService>();
            Forums = new ObservableCollection<Forum>(ForumService.GetAll());
            ShowComments = new RelayCommand(ShowForumComments);
            Close = new RelayCommand(CloseWindow);
        }
        private void CloseWindow(object param)
        {
            _window.Close();
        }
        private void ShowForumComments(object param)
        {
            _window.Close();
        }
    }
}
