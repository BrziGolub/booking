﻿using Booking.Commands;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Observer;
using Booking.Util;
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
    public class OwnerDateMoveViewModel : IObserver, INotifyPropertyChanged
    {
        public ObservableCollection<AccommodationReservationRequests> Requests { get; set; }
        public AccommodationReservationRequests SelectedAccommodationReservationRequest { get; set; }
        public IAccommodationReservationRequestService AccommodationReservationRequestService { get; set; }
        public INotificationService NotificationService { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand Accept { get; set; }
        public RelayCommand Reject { get; set; }
        public RelayCommand Close { get; set; }

        private readonly Window _window;
        public OwnerDateMoveViewModel(Window window)
        {
            _window = window;
            AccommodationReservationRequestService = InjectorService.CreateInstance<IAccommodationReservationRequestService>();
            NotificationService = InjectorService.CreateInstance<INotificationService>();
            AccommodationReservationRequestService.Subscribe(this);
            Requests = new ObservableCollection<AccommodationReservationRequests>(AccommodationReservationRequestService.GetSeeableDateChanges());
            SetCommands();
            OwnerComment = "";
        }

        public void SetCommands()
        {
            Accept = new RelayCommand(Button_Click_Accept);
            Reject = new RelayCommand(Button_Click_Reject);
            Close = new RelayCommand(Button_Click_Close);

        }

        public string _ownerComment;
        public string OwnerComment
        {
            get => _ownerComment;
            set
            {
                if (_ownerComment != value)
                {
                    _ownerComment = value;
                    OnPropertyChanged();
                }
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Update()
        {
            Requests.Clear();

            foreach (AccommodationReservationRequests request in AccommodationReservationRequestService.GetSeeableDateChanges())
            {
                Requests.Add(request);
            }
        }
        private void Button_Click_Close(object param)
        {
            CloseWindow();
        }
        private void Button_Click_Reject(object param)
        {
            if (OwnerComment.Equals(""))
            {
                MessageBox.Show("'REASONING' not entered");
                return;
            } 
            else if (SelectedAccommodationReservationRequest==null) 
            {
                MessageBox.Show("Choose a move request");
                return;
            }
            else
            {
                SelectedAccommodationReservationRequest.Comment = OwnerComment;
                AccommodationReservationRequestService.SaveRejected(SelectedAccommodationReservationRequest);
                NotificationService.MakeReject(SelectedAccommodationReservationRequest);
                AccommodationReservationRequestService.NotifyObservers();
            }
            
        }
        private void Button_Click_Accept(object param)
        {
            if (SelectedAccommodationReservationRequest == null)
            {
                MessageBox.Show("Choose a move request");
                return;
            }
            else 
            { 
            AccommodationReservationRequestService.SaveAccepted(SelectedAccommodationReservationRequest);
            NotificationService.MakeAccepted(SelectedAccommodationReservationRequest);
            AccommodationReservationRequestService.NotifyObservers();
            }
        }
        private void CloseWindow()
        {
            _window.Close();
        }
    }
}
