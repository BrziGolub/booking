using Booking.Commands;
using Booking.Domain.DTO;
using Booking.Domain.Model;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
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
using System.Windows.Media;

namespace Booking.WPF.ViewModels.Guide
{
    public class GuideRequestsStatisticViewModel :  INotifyPropertyChanged
    {

        public ILocationService locationService { get; set; }
        public ITourRequestService tourRequestService { get; set; }

        private bool _isLocationSelected;
        private bool _isLanguageSelected;
        private bool _isLocationEnabled;
        private bool _isLanguageEnabled;

        public bool IsLocationSelected
        {
            get { return _isLocationSelected; }
            set
            {
                _isLocationSelected = value;
                OnPropertyChanged(nameof(IsLocationSelected));
                OnPropertyChanged(nameof(IsLanguageSelected));
                UpdateControlsEnabledState();
            }
        }

        public bool IsLanguageSelected
        {
            get { return _isLanguageSelected; }
            set
            {
                _isLanguageSelected = value;
                OnPropertyChanged(nameof(IsLanguageSelected));
                OnPropertyChanged(nameof(IsLocationSelected));
                UpdateControlsEnabledState();
            }
        }

        public bool IsLocationEnabled
        {
            get { return _isLocationEnabled; }
            set
            {
                _isLocationEnabled = value;
                OnPropertyChanged(nameof(IsLocationEnabled));
            }
        }

        public bool IsLanguageEnabled
        {
            get { return _isLanguageEnabled; }
            set
            {
                _isLanguageEnabled = value;
                OnPropertyChanged(nameof(IsLanguageEnabled));
            }
        }

        private void UpdateControlsEnabledState()
        {
            IsLocationEnabled = IsLocationSelected;
            if(IsLocationEnabled) 
            {
                LanguageTB = null;
                RequestsByYearCollection = null;
                RequestsByMonthCollection = null;
            }
            IsLanguageEnabled = IsLanguageSelected;
            if (IsLanguageEnabled)
            {
                SelectedLocation = null;
                RequestsByYearCollection = null;
                RequestsByMonthCollection = null;
            }
        }

        private bool _isDataGridGenerralyVisible = true;
        public bool IsDataGridGenerallyVisible
        {
            get { return _isDataGridGenerralyVisible; }
            set { _isDataGridGenerralyVisible = value; OnPropertyChanged(); }
        }

        private bool _isDataGridYearlyVisible = true;
        public bool IsDataGridYearlyVisible
        {
            get { return _isDataGridYearlyVisible; }
            set { _isDataGridYearlyVisible = value; OnPropertyChanged(); }
        }

        private bool _isLabelYearVisible = true;
        public bool IsLabelYearVisible
        {
            get { return _isLabelYearVisible; }
            set { _isLabelYearVisible = value; OnPropertyChanged(); }
        }

        private bool _isTBYearVisible = true;
        public bool IsTBYearVisible
        {
            get { return _isTBYearVisible; }
            set { _isTBYearVisible = value; OnPropertyChanged(); }
        }

        private string _selectedLocation;
        public string SelectedLocation
        {
            get { return _selectedLocation; }
            set
            {
                _selectedLocation = value;
                OnPropertyChanged("SelectedLocation");
            }
        }

        public string _languageTB;
        public string LanguageTB
        {
            get => _languageTB;
            set
            {
                if (_languageTB != value)
                {
                    _languageTB = value;
                    OnPropertyChanged(nameof(LanguageTB));
                }
            }
        }

        public string _yearTB;
        public string YearTB
        {
            get => _yearTB;
            set
            {
                if (_yearTB != value)
                {
                    _yearTB = value;
                    OnPropertyChanged(nameof(YearTB));
                }
            }
        }

        private Brush _buttonBackground = new SolidColorBrush(Color.FromRgb(0xAA, 0xCB, 0xF0));
        public Brush Menu_Generally_Color
        {
            get { return _buttonBackground; }
            set
            {
                _buttonBackground = value;
                OnPropertyChanged(nameof(Menu_Generally_Color));
            }
        }

        private Brush _buttonBackground2 = new SolidColorBrush(Color.FromRgb(0x72, 0x87, 0x9E));
        public Brush Menu_Yearly_Color
        {
            get { return _buttonBackground2; }
            set
            {
                _buttonBackground2 = value;
                OnPropertyChanged(nameof(Menu_Yearly_Color));
            }
        }


        private ObservableCollection<YearlyRequests> _requestsByYearCollection;
        public ObservableCollection<YearlyRequests> RequestsByYearCollection
        {
            get { return _requestsByYearCollection; }
            set
            {
                _requestsByYearCollection = value;
                OnPropertyChanged(nameof(RequestsByYearCollection));
            }
        }

        private ObservableCollection<MonthlyRequests> _requestsByMonthCollection;
        public ObservableCollection<MonthlyRequests> RequestsByMonthCollection
        {
            get { return _requestsByMonthCollection; }
            set
            {
                _requestsByMonthCollection = value;
                OnPropertyChanged(nameof(RequestsByMonthCollection));
            }
        }

        public ObservableCollection<string> LocationsCollection { get; set; }

        private void FillLocationsComboBox()
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

            LocationsCollection.Clear();
            foreach (string str in items)
            {
                LocationsCollection.Add(str);
            }
        }


        public RelayCommand Close { get; set; }
        public RelayCommand Show { get; set; }
        public RelayCommand Menu_ThisYear { get; set; }
        public RelayCommand Menu_Generally { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Window _window;
        public GuideRequestsStatisticViewModel(Window window)
        {
            _window = window;

            locationService = InjectorService.CreateInstance<ILocationService>();
            tourRequestService = InjectorService.CreateInstance<ITourRequestService>();

            LocationsCollection = new ObservableCollection<string>();
            
            SetDefaultValues();
            FillLocationsComboBox();
            SetCommands();
        }

        private void SetDefaultValues()
        {
            IsDataGridGenerallyVisible = true;
            IsDataGridYearlyVisible = false;
            IsLabelYearVisible = false;
            IsTBYearVisible = false;

            IsLocationEnabled = true;
            IsLocationSelected = true;
        }

        private void CountByLocation()
        {
            if (SelectedLocation != null)
            {
                string pom = SelectedLocation.ToString();
                string[] CountryCity = pom.Split(',');
                string Country = CountryCity[0];
                string City = CountryCity[1].Trim();

                int locationId = locationService.GetIdByCountryAndCity(Country, City);


                var yearlyRequestsList = tourRequestService.GetRequestsByYearAndLocation(locationId);
                var observableYearlyRequestsList = new ObservableCollection<YearlyRequests>(yearlyRequestsList);

                RequestsByYearCollection = observableYearlyRequestsList;
               
                if (RequestsByYearCollection.Count == 0)
                {
                    MessageBox.Show("On location " + Country + ", " + City + " you don't have tour requests!");
                }

            }
            else 
            {
                MessageBox.Show("First you need to select location");
            }
        }

        private void CountByLanguage()
        {
            if (LanguageTB != null)
            {
                var yearlyRequestsList = tourRequestService.GetRequestsByYearAndLanguage(LanguageTB);
                var observableYearlyRequestsList = new ObservableCollection<YearlyRequests>(yearlyRequestsList);

                RequestsByYearCollection = observableYearlyRequestsList;
                if (RequestsByYearCollection.Count == 0)
                {
                    MessageBox.Show("On language " + LanguageTB + " you don't have tour requests!");
                }
            }
            else
            {
                MessageBox.Show("First you need to input language!");
            }
        }
        public void SetCommands()
        {
            Close = new RelayCommand(ButtonClose);
            Show = new RelayCommand(ButtonShow);
            Menu_ThisYear = new RelayCommand(ButtonMenu_ThisYear);
            Menu_Generally = new RelayCommand(ButtonMenu_Generally);
        }

        private void ButtonMenu_ThisYear(object param)
        {
            IsDataGridGenerallyVisible = false;
            IsDataGridYearlyVisible = true;
            IsLabelYearVisible = true;
            IsTBYearVisible = true;
            YearTB = null;
            LanguageTB = null;
            SelectedLocation = null;
            RequestsByYearCollection = null;
            RequestsByMonthCollection = null;
            Menu_Generally_Color = new SolidColorBrush(Color.FromRgb(0x72, 0x87, 0x9E));
            Menu_Yearly_Color = new SolidColorBrush(Color.FromRgb(0xAA, 0xCB, 0xF0));
        }
        private void ButtonMenu_Generally(object param)
        {
            IsDataGridGenerallyVisible = true; 
            IsDataGridYearlyVisible = false;
            IsLabelYearVisible = false;
            IsTBYearVisible = false;
            YearTB = null;
            LanguageTB= null;
            SelectedLocation = null;
            RequestsByYearCollection = null;
            RequestsByMonthCollection = null;
            Menu_Yearly_Color = new SolidColorBrush(Color.FromRgb(0x72, 0x87, 0x9E));
            Menu_Generally_Color = new SolidColorBrush(Color.FromRgb(0xAA, 0xCB, 0xF0));
        }

        private void ButtonClose(object param)
        {
            CloseWindow();
        }
        private void ButtonShow(object param)
        {

            if(IsDataGridGenerallyVisible)
            {
                if (IsLocationEnabled)
                    CountByLocation();

                if (IsLanguageEnabled)
                    CountByLanguage();
            }

            if(IsDataGridYearlyVisible)
            {
                if (IsLocationEnabled)
                    CountByLocationAndYear();
                if (IsLanguageEnabled)
                    CountByLanguageAndYear();
            }
            
            
        }

        private void CountByLocationAndYear()
        {
            if (SelectedLocation != null && YearTB != null)
            {
                string pom = SelectedLocation.ToString();
                string[] CountryCity = pom.Split(',');
                string Country = CountryCity[0];
                string City = CountryCity[1].Trim();

                int locationId = locationService.GetIdByCountryAndCity(Country, City);


                var monthlyRequestsList = tourRequestService.GetRequestsByMonthAndLocation(locationId, Convert.ToInt32(YearTB));
                var observableMonthlyRequestsList = new ObservableCollection<MonthlyRequests>(monthlyRequestsList);

                RequestsByMonthCollection = observableMonthlyRequestsList;

                if (RequestsByMonthCollection.Count == 0)
                {
                    MessageBox.Show("On location " + Country + ", " + City + " you don't have tour requests!");
                }

            }
            else
            {
                MessageBox.Show("First you need to select location and year");
            }
        }

        private void CountByLanguageAndYear()
        {
            if (LanguageTB != null && YearTB != null)
            {
                var monthlyRequestsList = tourRequestService.GetRequestsByMonthAndLanguage(LanguageTB, Convert.ToInt32(YearTB));
                var observableMonthlyRequestsList = new ObservableCollection<MonthlyRequests>(monthlyRequestsList);

                RequestsByMonthCollection = observableMonthlyRequestsList;
                if (RequestsByMonthCollection.Count == 0)
                {
                    MessageBox.Show("On language " + LanguageTB + " you don't have tour requests!");
                }
            }
            else
            {
                MessageBox.Show("First you need to input language and year!");
            }

        }

        private void CloseWindow()
        {
            _window.Close();
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
