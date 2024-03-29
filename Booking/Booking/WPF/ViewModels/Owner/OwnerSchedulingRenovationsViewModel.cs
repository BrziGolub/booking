﻿using Booking.Commands;
using Booking.Domain.Model;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Util;
using Booking.WPF.ViewModels.Guest1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Booking.WPF.ViewModels.Owner
{
    public class OwnerSchedulingRenovationsViewModel: INotifyPropertyChanged
    {
        public ObservableCollection<Range> Ranges { get; set; }
        public Range SelectedDate { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public DateTime startDay { get; set; }
        public DateTime endDay { get; set; }
        public int duration { get; set; }
        public IAccommodationReservationService AccommodationReservationService { get; set; }
        public IAccommodationRenovationService AccommodationRenovationService { get; set; }
        public IAccommodationService AccommodationService { get; set; }
        public RelayCommand Schedule { get; set; }
        public RelayCommand Close { get; set; }
        private readonly Window _window;
        public event PropertyChangedEventHandler PropertyChanged;

        public string _ownerDescription;
        public string OwnerDescription
        {
            get => _ownerDescription;
            set
            {
                if (_ownerDescription != value)
                {
                    _ownerDescription = value;
                    OnPropertyChanged();
                }
            }
        }

        public OwnerSchedulingRenovationsViewModel(Accommodation selectedAccommodation,DateTime StartDay,DateTime EndDay,int Duration, Window window)
        {
            _window = window;
            SelectedAccommodation = selectedAccommodation;
            AccommodationReservationService = InjectorService.CreateInstance<IAccommodationReservationService>();
            AccommodationRenovationService = InjectorService.CreateInstance<IAccommodationRenovationService>();
            AccommodationService = InjectorService.CreateInstance<IAccommodationService>();
            startDay = StartDay;
            endDay = EndDay;
            duration = Duration;
            OwnerDescription = "";
            List<(DateTime, DateTime)> suggestedDateRanges = AccommodationReservationService.SuggestDatesForRenovation(startDay, endDay, duration, SelectedAccommodation);
            Ranges = new ObservableCollection<Range>(suggestedDateRanges.Select(s => new Range { StartDate = s.Item1, EndDate = s.Item2 }).ToList());
            Schedule = new RelayCommand(ScheduleRenovation);
            Close = new RelayCommand(CloseWindow);
        }
        private void CloseWindow(object param)
        {
            _window.Close();
        }
        private void ScheduleRenovation(object param)
        {
            if (SelectedDate == null)
            {
                MessageBox.Show("Select a date");
                return;
            }
            else if (OwnerDescription=="")
            {
                MessageBox.Show("Enter description");
                return;
            }
            else 
            {
                AccommodationReservation newReservation = new AccommodationReservation(SelectedAccommodation, SelectedDate.StartDate, SelectedDate.EndDate, AccommodationService.GetSignedInOwner());
                AccommodationReservationService.SaveReservation(newReservation);
                AccommodationRenovation newRenovation = new AccommodationRenovation(SelectedAccommodation, SelectedDate.StartDate, SelectedDate.EndDate, OwnerDescription);
                AccommodationRenovationService.SaveRenovation(newRenovation);
                MessageBox.Show("Renovation succesfully scheduled");
                _window.Close();
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
