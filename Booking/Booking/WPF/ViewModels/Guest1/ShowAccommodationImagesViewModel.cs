using Booking.Commands;
using Booking.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Booking.WPF.ViewModels.Guest1
{
    public class ShowAccommodationImagesViewModel: INotifyPropertyChanged
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

        public RelayCommand PreviousButton_Click { get; set; }
        public RelayCommand NextButton_Click { get; set; }

        public ShowAccommodationImagesViewModel(Accommodation SelectedAccommodation)
        {
            selectedAccommodation = SelectedAccommodation;
            AccommodationLabel = SetAccommodationLabel(SelectedAccommodation);
            pictureUrls = new List<string>();
            currentPictureIndex = 0;
            setUrls();

            PreviousButton_Click = new RelayCommand(PreviousButton);
            NextButton_Click = new RelayCommand(NextButton);
        }

        public void setUrls()
        {
            foreach (var image in selectedAccommodation.Images)
            {
                pictureUrls.Add(image.Url);
            }
            SetCurrentImage();
        }

        private String SetAccommodationLabel(Accommodation accommodation)
        {
            return accommodation.Name + "-" + accommodation.Location.State + "-" + accommodation.Location.City + "-" + accommodation.Type;
        }

        private void PreviousButton(object param)
        {
            if (currentPictureIndex > 0)
            {
                currentPictureIndex--;
                SetCurrentImage();
                //NextButton.IsEnabled = true;
            }

            if (currentPictureIndex == 0)
            {
               // PreviousButton.IsEnabled = false;
            }
        }

        private void NextButton(object param)
        {
            if (currentPictureIndex < pictureUrls.Count - 1)
            {
                currentPictureIndex++;
                SetCurrentImage();
              //  PreviousButton.IsEnabled = true;
            }

            if (currentPictureIndex == pictureUrls.Count - 1)
            {
              //  NextButton.IsEnabled = false;
            }
        }

        private void SetCurrentImage()
        {
            CurrentImageUrl = pictureUrls[currentPictureIndex];
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
