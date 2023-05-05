using Booking.Commands;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Model.Images;
using Booking.Observer;
using Booking.Service;
using Booking.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Booking.WPF.ViewModels.Guide
{
    public class GuideCreateTourViewModel : IObserver, INotifyPropertyChanged
    {
        public ITourService tourService { get; set; }
        public ILocationService locationService { get; set; }
        public ITourImageService tourImageService { get; set; }
        TourKeyPoint SelectedTourKeyPoint { get; set; }

        public Tour tour = new Tour();
        
        public event PropertyChangedEventHandler PropertyChanged;

        List<string> imagePaths = new List<string>();
        int currentImageIndex = -1;

        public RelayCommand Close { get; set; }
        public RelayCommand AddTourKeyPoint { get; set; }
        public RelayCommand RemoveTourKeyPoint { get; set; }
        public RelayCommand AddPicture { get; set; }
        public RelayCommand RemovePicture { get; set; }
        public RelayCommand NextPicture { get; set; }
        public RelayCommand PreviousPicture { get; set; }
        public RelayCommand IncreaseMaxGuests { get; set; }
        public RelayCommand DecreaseMaxGuests { get; set; }
        public RelayCommand IncreaseDuration { get; set; }
        public RelayCommand DecreaseDuration { get; set; }
        
        public ICommand FillCityCommand { get; set; }
        private ICommand _saveTourCommand;

        public string SelectedCountry { get; set; }
        public string SelectedCity { get; set; }
        public string SelectedKeyPoint { get; set; }

        public ObservableCollection<string> CityCollection { get; set; }
        public ObservableCollection<string> CountryCollection { get; set; }
        public ObservableCollection<string> KeyPointsCollection { get; set; }
        public ObservableCollection<TourKeyPoint> tourKeyPoints1 { get; set; }


        private bool _cityComboBoxEnabled;
        public bool CityComboboxEnabled
        {
            get => _cityComboBoxEnabled;
            set
            {
                if (_cityComboBoxEnabled != value)
                {
                    _cityComboBoxEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _selectedIndexKeyPoint;
        public int SelectedIndexKeyPoint
        {
            get { return _selectedIndexKeyPoint; }
            set
            {
                _selectedIndexKeyPoint = value;
                OnPropertyChanged("SelectedIndexKeyPoint");
            }
        }

        public string _name;
        public string TourName
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string _city;
        public string City
        {
            get => _city;
            set
            {
                if (_city != value)
                {
                    _city = value;
                    OnPropertyChanged();
                }
            }
        }

        public string _country;
        public string Country
        {
            get => _country;
            set
            {
                if (_country != value)
                {
                    _country = value;
                    OnPropertyChanged();
                }
            }
        }

        public string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        public string _language;
        public string TourLanguage
        {
            get => _language;
            set
            {
                if (_language != value)
                {
                    _language = value;
                    OnPropertyChanged();
                }
            }
        }

        public int _maxguestnumber;
        public int MaxGuestNumber
        {
            get => _maxguestnumber;
            set
            {
                if (_maxguestnumber != value)
                {
                    _maxguestnumber = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<Location> _destinations;
        public List<Location> Destinations
        {
            get => _destinations;
            set
            {
                if (_destinations != value)
                {
                    _destinations = value;
                    OnPropertyChanged();
                }
            }
        }

        public string _startTime;
        public string StartTime
        {
            get => _startTime;
            set
            {
                if (_startTime != value)
                {
                    _startTime = value;
                    OnPropertyChanged();
                }
            }
        }

        public int _duration;
        public int Duration
        {
            get => _duration;
            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<string> _pictures;
        public List<string> Pictures
        {
            get => _pictures;
            set
            {
                if (_pictures != value)
                {
                    _pictures = value;
                    OnPropertyChanged();
                }
            }
        }

        public string _tbPictures;
        public string TbPictures
        {
            get => _tbPictures;
            set
            {
                if (_tbPictures != value)
                {
                    _tbPictures = value;
                    OnPropertyChanged();
                }
            }
        }

        private BitmapImage _imageSlideshowSource;
        public BitmapImage ImageSlideshowSource
        {
            get { return _imageSlideshowSource; }
            set
            {
                _imageSlideshowSource = value;
                OnPropertyChanged(nameof(ImageSlideshowSource));
            }
        }

        private Visibility _buttonNextVisibility = Visibility.Visible;
        public Visibility ButtonNextVisibility
        {
            get { return _buttonNextVisibility; }
            set { _buttonNextVisibility = value; OnPropertyChanged(); }
        }

        private Visibility _buttonPreviousVisibility = Visibility.Visible;
        public Visibility ButtonPreviousVisibility
        {
            get { return _buttonPreviousVisibility; }
            set { _buttonPreviousVisibility = value; OnPropertyChanged(); }
        }
        
        private readonly Window _window;
        public GuideCreateTourViewModel(Window window)
        {
            _window = window;
            tourService = InjectorService.CreateInstance<ITourService>();
            locationService = InjectorService.CreateInstance<ILocationService>();
            tourImageService = InjectorService.CreateInstance<ITourImageService>();

            CityCollection = new ObservableCollection<string>();
            CountryCollection = new ObservableCollection<string>();
            KeyPointsCollection = new ObservableCollection<string>();

            tourKeyPoints1 = new ObservableCollection<TourKeyPoint>(tourService.GetTourKeyPoints());

            SetCommands();
            FillComboBox();
            NextPreviousPhotoButtonsVisibility();

        }
        public void SetCommands()
        {
            Close = new RelayCommand(ButtonClose);
            AddTourKeyPoint = new RelayCommand(ButtonAddTourKeyPoint);
            RemoveTourKeyPoint = new RelayCommand(ButtonRemoveKeyPoint);
            AddPicture = new RelayCommand(ButtonAddPicture);
            RemovePicture = new RelayCommand(ButtonRemovePicture);

            NextPicture = new RelayCommand(buttonNext_Click);
            PreviousPicture = new RelayCommand(buttonPrevious_Click);
            IncreaseMaxGuests = new RelayCommand(ButtonIncreaseMaxGuests);
            DecreaseMaxGuests = new RelayCommand(ButtonDecreaseMaxGuests);
            IncreaseDuration = new RelayCommand(ButtonIncreaseDuration);
            DecreaseDuration = new RelayCommand(ButtonDecreaseDuration);

            FillCityCommand = new RelayCommand(FillCity);
        }
        public void FillCity(object param)
        {
            CityCollection.Clear();

            var locations = locationService.GetAll().Where(l => l.State.Equals(SelectedCountry));

            foreach (Location c in locations)
            {
                CityCollection.Add(c.City);
            }

            CityComboboxEnabled = true;
        }

        public void FillComboBox()
        {
            List<string> items = new List<string>();

            using (StreamReader reader = new StreamReader("../../Resources/Data/locations.csv"))
            {
                while (!reader.EndOfStream)
                {

                    string[] fields = reader.ReadLine().Split(',');
                    foreach (var field in fields)
                    {
                        string[] Countries = field.Split('|');
                        items.Add(Countries[1]);
                    }
                }
            }
            var distinctItems = items.Distinct().ToList();

            UpdateCountryCollection(distinctItems);

            if (SelectedCountry == null)
            {
                CityComboboxEnabled = false;
            }
            SelectedIndexKeyPoint = -1;
            FillKeyPointsComboBox();
        }

        public void UpdateCountryCollection(List<string> coutries)
        {
            CountryCollection.Clear();
            foreach (string str in coutries)
            {
                CountryCollection.Add(str);
            }
        }

        private void FillKeyPointsComboBox()
        {
            List<string> items = new List<string>();

            using (StreamReader reader = new StreamReader("../../Resources/Data/locations.csv"))
            {
                while (!reader.EndOfStream)
                {
                    string[] fields = reader.ReadLine().Split(',');
                    foreach (var field in fields)
                    {
                        string[] pom = field.Split('|');
                        items.Add(pom[1] + ", " + pom[2]);
                    }
                }
            }

            KeyPointsCollection.Clear();
            foreach(string str in items)
            {
                KeyPointsCollection.Add(str);
            }
        }

        private void ButtonClose(object param)
        {
            CloseWindow();
        }

        private void ButtonAddTourKeyPoint(object param)
        {

            if (SelectedKeyPoint != null)
            { 
                string pom = SelectedKeyPoint.ToString();
                string[] CountryCity = pom.Split(',');
                string Country = CountryCity[0];
                string City = CountryCity[1].Trim();

                int locationId = locationService.GetIdByCountryAndCity(Country, City);
                Location location = locationService.GetById(locationId);

                TourKeyPoint tourKeyPoints = new TourKeyPoint();
                tourKeyPoints1.Add(tourKeyPoints);
                tourKeyPoints.Location = location;
                tour.Destinations.Add(tourKeyPoints);

            }   
            SelectedIndexKeyPoint = -1;

        }
        private void ButtonRemoveKeyPoint(object param)
        {
            tourKeyPoints1.Remove(SelectedTourKeyPoint);
            Update();
        }
        private void CloseWindow()
        {
            _window.Close();
        }

        private void NextPreviousPhotoButtonsVisibility()
         {
             if (imagePaths.Count == 0)
             {
                 ImageSlideshowSource = null;
                 ButtonNextVisibility = Visibility.Collapsed;
                 ButtonPreviousVisibility = Visibility.Collapsed;
             }
             else
             {
                ButtonNextVisibility = Visibility.Visible;
                ButtonPreviousVisibility = Visibility.Visible;
             }
         }
        

       
        
        public ICommand SaveTourCommand
        {
            get
            {
                if (_saveTourCommand == null)
                {
                    _saveTourCommand = new RelayCommand(
                        param => SaveTour()
                    );
                }
                return _saveTourCommand;
            }
        }

        private bool CanSaveTour()
        {         
            return !string.IsNullOrEmpty(TourName) && !string.IsNullOrEmpty(SelectedCountry) &&
             !string.IsNullOrEmpty(SelectedCity) && !string.IsNullOrEmpty(Description) &&
             !string.IsNullOrEmpty(TourLanguage) && MaxGuestNumber > 0 &&
             StartTime != null && Duration > 0; 
        }

        private void SaveTour()
        {
            if (CanSaveTour())
            {
                Location location = new Location
                {
                    State = SelectedCountry,
                    City = SelectedCity
                };
                int LocationID = locationService.GetIdByCountryAndCity(SelectedCountry, SelectedCity);
                tour.Location.Id = LocationID;
                tour.Location = locationService.GetById(LocationID);
                tour.Name = TourName;
                tour.Description = Description;
                tour.Language = TourLanguage;
                tour.MaxVisitors = MaxGuestNumber;
                tour.StartTime = DateTime.Parse(StartTime);
                tour.Duration = Duration;

                if (tour.Destinations.Count < 2)
                {
                    MessageBox.Show("Tour need to have 2 'KEY POINTS' at least");
                    return;
                }
                if (tour.Images.Count == 0)
                {
                    MessageBox.Show("Tour need to have 1 'PICTURE' at least");
                    return;
                }
                if (string.IsNullOrEmpty(SelectedCountry))
                {
                    MessageBox.Show("'COUNTRY' not entered");
                    return;
                }
                if (string.IsNullOrEmpty(tour.Location.City))
                {
                    MessageBox.Show("'CITY' not entered");
                    return;
                }
                if (tour.MaxVisitors < 1)
                {
                    MessageBox.Show("'MAX GUESTS NUMBER' should be greater than 0");
                    return;
                }
                if (tour.Duration < 1)
                {
                    MessageBox.Show("'DURATION' should be greater than 0");
                    return;
                }
                if(TbPictures == "")
                {
                    MessageBox.Show("Tour need to have 1 'IMAGE' at least");
                    return;
                }

                tourService.Create(tour);
                MessageBox.Show("Tour successfully created");
                CloseWindow();
            }
            else
            {
                MessageBox.Show("Please fill in all required fields.");
            }
        }

        private void ButtonAddPicture(object param)
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string imagePath = dialog.FileName;
                imagePaths.Add(imagePath);
                currentImageIndex = imagePaths.Count - 1;
                ImageSlideshowSource = new BitmapImage(new Uri(imagePath));
                TbPictures = imagePath;
            }

            if (TbPictures != "")
            {
                TourImage Images = new TourImage();
                Images.Url = TbPictures;
                tour.Images.Add(Images);
                NextPreviousPhotoButtonsVisibility();
            }
            else
            {
                MessageBox.Show("Photo URL can't be empty!");
            }
        }

        private void ButtonRemovePicture(object param)
        {
            TourImage Images = new TourImage();
            Images.Url = TbPictures;
            ImageSlideshowSource = null;
            imagePaths.Remove(Images.Url);
            tour.Images.Remove(Images);
            TbPictures = "";
            NextPreviousPhotoButtonsVisibility();
        }



        private void buttonPrevious_Click(object param)
        {
            if (currentImageIndex > 0)
            {
                currentImageIndex--;
                string imagePath = imagePaths[currentImageIndex];
                ImageSlideshowSource = new BitmapImage(new Uri(imagePath));
                TbPictures = imagePath;
            }
        }

        private void buttonNext_Click(object param)
        {
            if (currentImageIndex < imagePaths.Count - 1)
            {
                currentImageIndex++;
                string imagePath = imagePaths[currentImageIndex];
                ImageSlideshowSource = new BitmapImage(new Uri(imagePath));
                TbPictures= imagePath;
            }
        }


        private void ButtonIncreaseMaxGuests(object param)
        {
            int currentValue;
            if (int.TryParse(MaxGuestNumber.ToString(), out currentValue))
            {
                MaxGuestNumber = (currentValue + 10);
            }
        }

        private void ButtonDecreaseMaxGuests(object param)
        {
            int currentValue;
            if (int.TryParse(MaxGuestNumber.ToString(), out currentValue) && currentValue > 0)
            {
                MaxGuestNumber = (currentValue - 10);
            }
            if (currentValue < 9)
            {
                MaxGuestNumber = 0;
            }
        }

        private void ButtonIncreaseDuration(object param)
        {
            int currentValue;
            if (int.TryParse(Duration.ToString(), out currentValue))
            {
                Duration = (currentValue + 1);
            }
        }
        private void ButtonDecreaseDuration(object param)
        {
            int currentValue;
            if (int.TryParse(Duration.ToString(), out currentValue) && currentValue > 0)
            {
                Duration = (currentValue - 1);
            }
        }


        public void Update()
        {
            tourKeyPoints1.Clear();

            foreach (TourKeyPoint t in tourService.GetTourKeyPoints())
            {
                tourKeyPoints1.Add(t);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
