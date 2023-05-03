using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Model.Images;
using Booking.Observer;
using Booking.Repository;
using Booking.Service;
using Booking.Util;
using Booking.WPF.Views.Guide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Booking.View
{
    public partial class GuideFollowTourLive : Window , IObserver
    {
        public ObservableCollection<Tour> Tours { get; set; }
        public ITourService TourService { get; set; }
        public ITourImageService TourImageService { get; set; }
        public Tour SelectedTour { get; set; }
        int Pomid { get; set; }
        List<string> imagePaths = new List<string>();
        int currentImageIndex = -1;

        public GuideFollowTourLive()
        {
            InitializeComponent();
            this.DataContext = this;

            TourService = InjectorService.CreateInstance<ITourService>();
            TourImageService = InjectorService.CreateInstance<ITourImageService>();

            TourService.Subscribe(this);

            Pomid = -1;
            Tours = new ObservableCollection<Tour>(TourService.GetTodayTours());

            LoadImages();
        }

        private void LoadImages()
        {
            imagePaths.Clear();
            List<TourImage> tourImages = TourImageService.GetImagesFromStartedTourId();

            foreach (TourImage image in tourImages)
            {
                string imagePath = image.Url;
                imagePaths.Add(imagePath);
                imageSlideshow.Source = new BitmapImage(new Uri(imagePath));
            }
            if (tourImages.Count == 0) 
            { 
                imageSlideshow.Source = null;
                buttonNext.Visibility = Visibility.Collapsed;
                buttonPrevious.Visibility = Visibility.Collapsed;
            }
            else 
            {
                buttonNext.Visibility = Visibility.Visible;
                buttonPrevious.Visibility = Visibility.Visible;
            }
        }

        private void StartTour(object sender, RoutedEventArgs e)
        {

            int startedTours = 0;
            startedTours = CheckNumberOfStartedTours(startedTours);

            if (startedTours < 1)
            {
                StartSelectedTour();
            }
            else
            {
                MessageBox.Show("You can start maximum 1 tour in same time!");
            }
        }

        private void StartSelectedTour()
        {
            if (SelectedTour != null)
            {
                GuideKeyPointsCheck guideKeyPointsCheck = new GuideKeyPointsCheck(SelectedTour.Id);
                Pomid = SelectedTour.Id;
                if (SelectedTour.IsEnded)
                {
                    MessageBox.Show("You cant start ended tour!");
                }
                else
                {
                    SelectedTour.IsStarted = true;
                    TourService.UpdateTour(SelectedTour);
                    MessageBox.Show(SelectedTour.Name.ToString() + " is started!");
                    TourService.NotifyObservers();
                   
                    guideKeyPointsCheck.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("In order to start the tour, you first need to select it!");
            }
        }

        private int CheckNumberOfStartedTours(int startedTours)
        {
            foreach (var tour in Tours)
            {
                if (tour.IsStarted == true)
                {
                    startedTours++;
                    Pomid = tour.Id;
                }

            }

            return startedTours;
        }

        public void Update()
        {  
            Tours.Clear();
            foreach(Tour t in TourService.GetTodayTours())
            {
                Tours.Add(t);
            }
            LoadImages();
        }

        private void EndTour(object sender, RoutedEventArgs e)
        {
            if (SelectedTour != null && SelectedTour.IsStarted == true)
            {
                SelectedTour.IsStarted = false;
                SelectedTour.IsEnded = true;
                TourService.UpdateTour(SelectedTour);
                MessageBox.Show(SelectedTour.Name.ToString() + " is ended!");
                TourService.NotifyObservers();
               

            }
            else
            {
                MessageBox.Show("In order to end the tour, you first need to select started tour!");
            }
        }

        private void ShowOnGoingTour(object sender, RoutedEventArgs e)
        {
            int pomid1 = -1;
                foreach (var t in Tours)
                {
                if (t.IsStarted == true)
                    pomid1 = t.Id;
                else Pomid = -1;
                }

            Pomid = pomid1;

            Tour tour = TourService.GetById(Pomid);
                if (tour != null)
                {
                    GuideKeyPointsCheck guideKeyPointsCheck = new GuideKeyPointsCheck(tour.Id);
                    guideKeyPointsCheck.ShowDialog();
                } 
                else
                {
                    MessageBox.Show("You don't have ongoing tour!");
                }         
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ShowDescriptionText(object sender, RoutedEventArgs e)
        {
            ShowDescription showDescriptionText = new ShowDescription(SelectedTour.Id);
            showDescriptionText.Show();
        }

        private void buttonPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (currentImageIndex > 0)
            {
                currentImageIndex--;
                string imagePath = imagePaths[currentImageIndex];
                imageSlideshow.Source = new BitmapImage(new Uri(imagePath));
            }
        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            if (currentImageIndex < imagePaths.Count - 1)
            {
                currentImageIndex++;
                string imagePath = imagePaths[currentImageIndex];
                imageSlideshow.Source = new BitmapImage(new Uri(imagePath));
            }
        }

    }
}
