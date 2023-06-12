using Booking.Commands;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Observer;
using Booking.Service;
using Booking.Util;
using Booking.WPF.Views.Guest1;
using MahApps.Metro.Controls;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Booking.WPF.ViewModels.Guest1
{
    public class QuickSearchViewModel: INotifyPropertyChanged, IDataErrorInfo
    {
        //, IObserver
        NavigationService navigationService;

        public ObservableCollection<Accommodation> Accommodations { get; set; }

        public List<AccommodationDTO> _accDTO;

        public ObservableCollection<AccommodationDTO> AccommodationsDTO { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public IAccommodationService _accommodationService;

        public RelayCommand Button_Click_Book { get; set; }

        public RelayCommand Button_Click_ShowImages { get; set; }

        public AccommodationDTO SelectedDTO { get; set; }

        public string _numberOfGuests;
        public string NumberOfGuests
        {
            get => _numberOfGuests;
            set
            {
                if (_numberOfGuests != value)
                {
                    _numberOfGuests = value;
                    OnPropertyChanged();
                }
            }

        }

        public string _daysToStay;
        public string DaysToStay
        {
            get => _daysToStay;
            set
            {
                if (_daysToStay != value)
                {
                    _daysToStay = value;
                    OnPropertyChanged();
                }
            }

        }

        public DateTime _arrivalDay;
        public DateTime ArrivalDay
        {
            get => _arrivalDay;
            set
            {
                if (_arrivalDay != value)
                {
                    _arrivalDay = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime _departureDay;
        public DateTime DepartureDay
        {
            get => _departureDay;
            set
            {
                if (_departureDay != value)
                {
                    _departureDay = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Error => null;


        private Regex _searchReservationDaysRegex = new Regex("[1-9]{1,2}");
        private Regex _searchGuestRegex = new Regex("^[1-9][0-9]?$");

        public string this[string columnName]
        {
            get
            {
                if (columnName == "SerachGuests")
                {
                    if (NumberOfGuests != String.Empty)
                    {
                        Match match = _searchGuestRegex.Match(NumberOfGuests);
                        if (!match.Success)
                            return "Number of guests is not in correct format!";
                    }
                    else
                    {
                        return "Required filed!";
                    }
                }

                if (columnName == "SearchReservationDays")
                {
                    if (DaysToStay != String.Empty)
                    {
                        Match match = _searchReservationDaysRegex.Match(DaysToStay);
                        if (!match.Success)
                            return "Staying days is not in correct format!";
                    }
                    else
                    {
                        return "Required filed!";
                    }
                }

                return null;
            }
        }

        private readonly string[] _validatedProperties = { "DaysToStay", "NumberOfGuests" };

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RelayCommand Button_Click_Search { get; set; }

        public QuickSearchViewModel(NavigationService navigate)
        {
            _accommodationService = InjectorService.CreateInstance<IAccommodationService>();
            Accommodations = new ObservableCollection<Accommodation>(_accommodationService.GetAll());
            AccommodationsDTO = new ObservableCollection<AccommodationDTO>(_accommodationService.GetAllDTO());
            _accDTO = new List<AccommodationDTO>();
            //_accommodationService.Subscribe(this);
            //NumberOfGuests = "";
            //DaysToStay = "";
            navigationService = navigate;
            ArrivalDay = DateTime.Now;
            DepartureDay = DateTime.Now;
            Button_Click_Search = new RelayCommand(SearchButton);
            Button_Click_Book = new RelayCommand(BookButton);
            Button_Click_ShowImages = new RelayCommand(Show_Images);
        }

        public void Show_Images(object s)
        {
            var notificationManager = new NotificationManager();
            if (SelectedDTO == null)
            {
                NotificationContent content = new NotificationContent { Title = "Permission denied!", Message = "Select accommodation first!!!!", Type = NotificationType.Error };
                notificationManager.Show(content, areaName: "WindowArea", expirationTime: TimeSpan.FromSeconds(5));
            }
            else
            {
                navigationService.Navigate(new ShowAccommodationImages(SelectedDTO.accommodation));
            }
        }

        public void SearchButton(object sender)
        {
           
            List<AccommodationDTO> accommodationDTOs = new List<AccommodationDTO>();
            if (ArrivalDay.Date == DateTime.Now.Date && DepartureDay.Date == DateTime.Now.Date)
            {
                foreach (Accommodation accommodation in Accommodations)
                {
                    if (_accommodationService.CheckGuestsNumber(accommodation, int.Parse(NumberOfGuests)) && _accommodationService.AccommodationIsAvailable(accommodation, int.Parse(DaysToStay)))
                    {
                        AccommodationDTO dto = new AccommodationDTO();
                        dto.accommodation = accommodation;
                        List<(DateTime, DateTime)> ranges = _accommodationService.FindAvailableDatesQuick(accommodation, int.Parse(DaysToStay));
                        List<DatesDTO> datesList = new List<DatesDTO>();
                        foreach (var range in ranges)
                        {
                            DatesDTO dates = new DatesDTO();
                            dates.InitialDate = range.Item1;
                            dates.EndDate = range.Item2;
                            datesList.Add(dates);
                        }
                        dto.dates = datesList;
                        accommodationDTOs.Add(dto);
                    }
                }

                // AccommodationsDTO = new ObservableCollection<AccommodationDTO>(accommodationDTOs);
                //_accommodationService.NotifyObservers();
                navigationService.Navigate(new QuickSearchSuggestionsView(accommodationDTOs, navigationService));
               // _accDTO = accommodationDTOs;
                //AccommodationsDTO = new ObservableCollection<AccommodationDTO>(accommodationDTOs);

                //_accommodationService.NotifyObservers();

            }
            else
            {
                foreach (Accommodation accommodation in Accommodations)
                {
                    if (_accommodationService.CheckGuestsNumber(accommodation, int.Parse(NumberOfGuests)) && _accommodationService.AccommodationIsAvailableInRange(accommodation, int.Parse(DaysToStay), ArrivalDay, DepartureDay))
                    {
                        AccommodationDTO dto = new AccommodationDTO();
                        dto.accommodation = accommodation;
                        List<(DateTime, DateTime)> ranges = _accommodationService.FindAvailableDatesQuickRanges(accommodation, int.Parse(DaysToStay), ArrivalDay, DepartureDay);
                        List<DatesDTO> datesList = new List<DatesDTO>();
                        foreach (var range in ranges)
                        {
                            DatesDTO dates = new DatesDTO();
                            dates.InitialDate = range.Item1;
                            dates.EndDate = range.Item2;
                            datesList.Add(dates);
                        }
                        dto.dates = datesList;
                        accommodationDTOs.Add(dto);
                    }
                }
               // _accDTO = accommodationDTOs;
               // AccommodationsDTO = new ObservableCollection<AccommodationDTO>(accommodationDTOs);
                
               // _accommodationService.NotifyObservers();
                navigationService.Navigate(new QuickSearchSuggestionsView(accommodationDTOs, navigationService));
            }
        }

        public void BookButton(object s)
        {
            foreach(var v in _accDTO)
            {
                if(SelectedDTO.accommodation.Id == v.accommodation.Id)
                {
                    SelectedDTO.dates = v.dates;
                }
            }
            navigationService.Navigate(new SuggestedDatesQuickSearch(SelectedDTO));
        }
       /* public void Update()
        {
            AccommodationsDTO.Clear();
            ObservableCollection<AccommodationDTO> AccommodationsDTO2 = new ObservableCollection<AccommodationDTO>(_accDTO);
            AccommodationsDTO = AccommodationsDTO2;
        }*/
    }

    public class AccommodationDTO
    {
        public Accommodation accommodation { get; set; }
        public List<DatesDTO> dates { get; set; }
    }

    public class DatesDTO
    {
        public DateTime InitialDate { get; set; }
        public DateTime EndDate { get; set; }
    }

}

