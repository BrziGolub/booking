using Booking.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Booking.WPF.ViewModels.Owner
{
    public class WizardViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public int GottenPageNumber { get; set; }
        //public String PageLabel { get; set; } = String.Empty;

        private List<string> pageTexts;

        private int currentPageIndex;

        public String _currentPageText;
        public String CurrentPageText
        {
            get => _currentPageText;
            set
            {
                if (_currentPageText != value)
                {
                    _currentPageText = value;
                    OnPropertyChanged();
                }
            }
        }
        public String _pageLabel;
        public String PageLabel
        {
            get => _pageLabel;
            set
            {
                if (_pageLabel != value)
                {
                    _pageLabel = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool _isNextEnabled;

        public bool IsNextEnabled
        {
            get => _isNextEnabled;
            set
            {
                if (_isNextEnabled != value)
                {
                    _isNextEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool _isPreviousEnabled;

        public bool IsPreviousEnabled
        {
            get => _isPreviousEnabled;
            set
            {
                if (_isPreviousEnabled != value)
                {
                    _isPreviousEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public RelayCommand PreviousButton_Click { get; set; }
        public RelayCommand NextButton_Click { get; set; }
        public RelayCommand CloseWindow { get; set; }
        private readonly Window _window;
        public WizardViewModel(int gottenPageNumber,Window window)
        {
            _window = window;
            GottenPageNumber = gottenPageNumber;
            PageLabel = SetPageLabel(GottenPageNumber);
            pageTexts = new List<string>();
            currentPageIndex = GottenPageNumber;

            setTexts();

            PreviousButton_Click = new RelayCommand(PreviousButton);
            NextButton_Click = new RelayCommand(NextButton);
            CloseWindow = new RelayCommand(Close);
            IsPreviousEnabled = false;


            if (currentPageIndex == pageTexts.Count - 1)
            {
                IsNextEnabled = false;
            }
            else
            {
                IsNextEnabled = true;
            }
            if (currentPageIndex > 0)
            {
                IsPreviousEnabled = true;
            }

            if (currentPageIndex == 0)
            {
                IsPreviousEnabled = false;
            }
        }
        private void Close(object param)
        {
            _window.Close();
        }
        public void setTexts()
        {
            string text0 = "Home page";
            pageTexts.Add(text0);
            string text1 = "Register accommodation";
            pageTexts.Add(text1);
            /*
            foreach (var image in selectedAccommodation.Images)
            {
                pictureUrls.Add(image.Url);
            }
            SetCurrentImage();
            */
            SetCurrentPage();
        }
        private String SetPageLabel(int gottenPageNumber)
        {
            if (gottenPageNumber == 0)
            {
                return "Home page";
            }
            else 
            {
                return "Register accommodation";
            }
            //return accommodation.Name + "-" + accommodation.Location.State + "-" + accommodation.Location.City + "-" + accommodation.Type;
        }
        private void PreviousButton(object param)
        {
            if (currentPageIndex > 0)
            {
                currentPageIndex--;
                SetCurrentPage();
                PageLabel=SetPageLabel(currentPageIndex);
                IsNextEnabled = true;
            }

            if (currentPageIndex == 0)
            {
                IsPreviousEnabled = false;
            }
        }
        private void NextButton(object param)
        {
            if (currentPageIndex < pageTexts.Count - 1)
            {
                currentPageIndex++;
                SetCurrentPage();
                PageLabel=SetPageLabel(currentPageIndex);
                IsPreviousEnabled = true;
            }

            if (currentPageIndex == pageTexts.Count - 1)
            {
                IsNextEnabled = false;
            }
        }

        private void SetCurrentPage()
        {
            CurrentPageText = pageTexts[currentPageIndex];
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
