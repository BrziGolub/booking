using Booking.Commands;
using Booking.Domain.Model;
using Booking.Domain.ServiceInterfaces;
using Booking.Util;
using Booking.WPF.Views.Guest1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Booking.WPF.ViewModels.Guest1
{
    public class ShowAllForumsViewModel
    {
        public ObservableCollection<Forum> Forums { get; set; }
        public IForumService ForumService { get; set; }
        public Forum SelectedForum { get; set; }
        public RelayCommand Show_Comments_Button { get; set; }
        public RelayCommand Create_Forum_Button { get; set; }

        public NavigationService navigationService;

        public ShowAllForumsViewModel(NavigationService navigation)
        {
            ForumService = InjectorService.CreateInstance<IForumService>();
            Forums = new ObservableCollection<Forum>(ForumService.GetAll());
            Show_Comments_Button = new RelayCommand(ShowForumComments);
            Create_Forum_Button = new RelayCommand(CreateForumButton);
            navigationService = navigation;
        }
        public void CreateForumButton(object sender)
        {
            navigationService.Navigate(new CreateForum(navigationService));
        }

        public void ShowForumComments(object param)
        {
            
        }
    }
}
