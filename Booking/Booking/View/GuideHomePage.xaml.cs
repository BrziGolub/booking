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

namespace Booking.View
{
    /// <summary>
    /// Interaction logic for GuideHomePage.xaml
    /// </summary>
    public partial class GuideHomePage : Window, IObserver
    {
        public ObservableCollection<Tour> Tours { get; set; }
        public TourService _tourService { get; set; }
        
        public GuideHomePage()
        {
            InitializeComponent();            
            this.DataContext = this;
            _tourService = new TourService();
            _tourService.Subscribe(this);

            Tours = new ObservableCollection<Tour>(_tourService.GetAll());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        GuideCreateTour guideCreateTour = new GuideCreateTour();
        guideCreateTour.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            GuideFollowTourLive guideFollowTourLive = new GuideFollowTourLive();
            guideFollowTourLive.Show();
        }

        public void Update()
        {
            Tours.Clear();
            foreach (Tour t in _tourService.GetAll())
            {
                Tours.Add(t);
            }
        }
    }
}
