using Booking.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Booking.WPF.Views.Guest1
{

    public partial class ShowAccommodationImages : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Accommodation selectedAccommodation { get; set; }
        public String AccommodationLabel { get; set; } = String.Empty;

        private List<string> pictureUrls;
        private int currentPictureIndex;
        
        public String _currentImageUrl;
        public String CurrentImageUrl
        {
            get => _currentImageUrl;
            set
            {
                if (_currentImageUrl != value)
                {
                    _currentImageUrl = value;
                    OnPropertyChanged();
                }
            }
        }

        public ShowAccommodationImages(Accommodation SelectedAccommodation)
        {
            InitializeComponent();
            this.DataContext = this;
            selectedAccommodation = SelectedAccommodation;
            AccommodationLabel = SetAccommodationLabel(SelectedAccommodation) ;
            pictureUrls = new List<string>();
            currentPictureIndex = 0;
            setUrls();
        }

        public void setUrls()
        {
            foreach(var image in selectedAccommodation.Images)
            {
                pictureUrls.Add(image.Url);
            }
            SetCurrentImage();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private String SetAccommodationLabel(Accommodation accommodation)
        {
            return accommodation.Name + "-" + accommodation.Location.State + "-" + accommodation.Location.City + "-" + accommodation.Type;
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPictureIndex > 0)
            {
                currentPictureIndex--;
                SetCurrentImage();
                NextButton.IsEnabled = true;
            }

            if (currentPictureIndex == 0)
            {
                PreviousButton.IsEnabled = false;
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPictureIndex < pictureUrls.Count - 1)
            {
                currentPictureIndex++;
                SetCurrentImage();
                PreviousButton.IsEnabled = true;
            }

            if (currentPictureIndex == pictureUrls.Count - 1)
            {
                NextButton.IsEnabled = false;
            }
        }

        private void SetCurrentImage()
        {
            CurrentImageUrl = pictureUrls[currentPictureIndex];
        }
    }
}
