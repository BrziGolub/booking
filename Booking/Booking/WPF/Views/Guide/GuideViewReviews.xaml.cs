using Booking.Domain.Model;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Observer;
using Booking.Service;
using Booking.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Booking.View.Guide
{
    public partial class GuideViewReviews : Window, IObserver
    {
        public ObservableCollection<TourGrade> TourGrades { get; set; }
        public ITourGradeService _tourGradeService { get; set; }

        public TourGrade SelectedTourGrade { get; set; }
        public GuideViewReviews()
        {
            InitializeComponent();
            this.DataContext = this;

            _tourGradeService = InjectorService.CreateInstance<ITourGradeService>();

            _tourGradeService.Subscribe(this);

            reviewsDataGrid.Items.Clear();
            TourGrades = new ObservableCollection<TourGrade>(_tourGradeService.GetGuideGrades());

        }

        private void ShowReviewText(object sender, RoutedEventArgs e)
        {
            ShowReviewComment showReviewComment = new ShowReviewComment(SelectedTourGrade.Id);
            showReviewComment.ShowDialog();
        }

        private MessageBoxResult ConfirmReport()
        {
            string sMessageBoxText = $"Are you sure to report grade?";
            string sCaption = "Report grade";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

            return result;
        }
        private void Report(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = ConfirmReport();
            if (result == MessageBoxResult.Yes)
            {
                SelectedTourGrade.Valid = false;
                _tourGradeService.UpdateTourGrade(SelectedTourGrade);
                _tourGradeService.NotifyObservers();
            }
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void Update()
        {
            TourGrades.Clear();

            foreach (TourGrade t in _tourGradeService.GetGuideGrades())
            {
                TourGrades.Add(t);
            }
        }
    }
}
