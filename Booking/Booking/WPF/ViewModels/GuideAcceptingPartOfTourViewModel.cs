using Booking.Application.UseCases;
using Booking.Commands;
using Booking.Domain.Model;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Observer;
using Booking.Service;
using Booking.Util;
using Booking.WPF.ViewModels.Guide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace Booking.WPF.ViewModels
{
    public class GuideAcceptingPartOfTourViewModel : IObserver, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Window _window;

        public ITourRequestService tourRequestService { get; set; }
        public ITourService tourService { get; set; }

        private TourRequest _selectedTourRequest;
        public TourRequest SelectedTourRequest
        {
            get { return _selectedTourRequest; }
            set
            {
                _selectedTourRequest = value;
                OnPropertyChanged("SelectedTourRequest");
            }
        }

        public ObservableCollection<TourRequest> TourRequests { get; set; }

        public RelayCommand Close { get; set; }
        public RelayCommand AcceptPartOfTour { get; set; }
        
        public GuideAcceptingPartOfTourViewModel(Window window)
        {
            _window = window;

            tourRequestService = InjectorService.CreateInstance<ITourRequestService>();
            tourService = InjectorService.CreateInstance<ITourService>();

            TourRequests = new ObservableCollection<TourRequest>(tourRequestService.GetAllOnHoldPartOfComplex());

            SetCommands();
        }

        public void SetCommands()
        {
            Close = new RelayCommand(ButtonClose);
            AcceptPartOfTour = new RelayCommand(ButtonAcceptPartOfTour);
        }

        private void ButtonClose(object param)
        {
            CloseWindow();
        }
        private void CloseWindow()
        {
            _window.Close();
        }


        private void ButtonAcceptPartOfTour(object param)
        {
            if(IsGuideAvailable(TourService.SignedGuideId))
            {
                SelectDateForComplexTourViewModel.GuestStartDate = SelectedTourRequest.StartTime;
                SelectDateForComplexTourViewModel.GuestEndDate = SelectedTourRequest.EndTime;

                SelectDateForComplexTour selectDateForComplexTour = new SelectDateForComplexTour();
                selectDateForComplexTour.ShowDialog();

                //SelectedTourRequest.Status = "AcceptedComplex";
                //SelectedTourRequest.Notify = true;
                //SelectedTourRequest.TourReservedStartTime = DateTime.Parse(StartTime); // treba ga samo izjednaciti sa odabranim start timeom
                //SelectedTourRequest.StartTime = ; // uzeti iz novog prozora iz kog samo biram datum
                //SelectedTourRequest.EndTime =  ;  // uzeti iz novog prozora iz kog samo biram datum

                //tourRequestService.ChangeStatus(SelectedTourRequest);
            }
            else
            {
                System.Windows.MessageBox.Show("You already have a tour planned in that time period!", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
        

        public void Update()
        {
            TourRequests.Clear();
            foreach (TourRequest request in tourRequestService.GetAllOnHoldPartOfComplex())
            {
                TourRequests.Add(request);
            }

        }

        public bool IsGuideAvailable(int guideId)
        {
            List<Tour> tours = tourService.GetGuideTours();

            foreach (Tour tour in tours)
            {
                DateTime tourEndTime = tour.StartTime + TimeSpan.FromDays(tour.Duration * 24);
                bool isOverlapping = SelectedTourRequest.StartTime < tourEndTime && SelectedTourRequest.EndTime > tour.StartTime;

                if (isOverlapping)
                {
                    return false;
                }
            }

            return true;
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
