using Booking.Commands;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Observer;
using Booking.Util;
using Booking.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Booking.WPF.ViewModels.Owner
{
    public class OwnerViewReviewsViewModel : IObserver
    {
        public ObservableCollection<AccommodationAndOwnerGrade> Grades { get; set; }
        public IAccommodationAndOwnerGradeService AccommodationAndOwnerGradeService { get; set; }
        public AccommodationAndOwnerGrade SelectedGrade { get; set; }
        public RelayCommand CloseWindow { get; set; }
        public RelayCommand ShowPictures { get; set; }
        private readonly Window _window;
        public OwnerViewReviewsViewModel(Window window)
        {
            _window = window;
            AccommodationAndOwnerGradeService = InjectorService.CreateInstance<IAccommodationAndOwnerGradeService>();

            AccommodationAndOwnerGradeService.Subscribe(this);
            Grades = new ObservableCollection<AccommodationAndOwnerGrade>(AccommodationAndOwnerGradeService.GetSeeableGrades());
            SetCommands();
        }
        public void SetCommands()
        {
            CloseWindow = new RelayCommand(Close);
            ShowPictures = new RelayCommand(ShowImages);

        }
        public void Update()
        {
            Grades.Clear();
            foreach (AccommodationAndOwnerGrade g in AccommodationAndOwnerGradeService.GetSeeableGrades())
            {
                Grades.Add(g);
            }
        }
        private void Close(object param)
        {
            _window.Close();
        }
        private void ShowImages(object param)
        {
            ShowGradeImages view = new ShowGradeImages(SelectedGrade);
            view.ShowDialog();
        }
    }
}
