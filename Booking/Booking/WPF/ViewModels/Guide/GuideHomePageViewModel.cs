using Booking.Model;
using Booking.Observer;
using Booking.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Resources;
using Booking.Domain.ServiceInterfaces;
using Booking.Util;
using Booking.View.Guide;
using Booking.View;

namespace Booking.WPF.ViewModels.Guide
{
    public class GuideHomePageViewModel : IObserver // ,Window
    {
        public ObservableCollection<Tour> Tours { get; set; }
        public ITourService _tourService { get; set; }

        public Tour SelectedTour { get; set; }
        public User user { get; set; }

        public static string Username;
        public GuideHomePageViewModel() 
        {

            _tourService = InjectorService.CreateInstance<ITourService>();

            _tourService.Subscribe(this);

            Tours = new ObservableCollection<Tour>(_tourService.GetGuideTours());

            //usernameTextBlock.Text = Username;


        }


        public void Update()
        {
            Tours.Clear();

            foreach (Tour t in _tourService.GetGuideTours())
            {
                Tours.Add(t);
            }
        }





    }
}
