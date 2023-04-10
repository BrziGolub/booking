using Booking.Model;
using Booking.Observer;
using Booking.Service;
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

namespace Booking.View
{
    /// <summary>
    /// Interaction logic for OwnerViewReviews.xaml
    /// </summary>
    public partial class OwnerViewReviews : Window, IObserver
    {
        public ObservableCollection<AccommodationAndOwnerGrade> Grades { get; set; }
        public AccommodationAndOwnerGradeService AccommodationAndOwnerGradeService { get; set; }
        public OwnerViewReviews()
        {
            InitializeComponent();
            this.DataContext = this;

            var app = Application.Current as App;

            AccommodationAndOwnerGradeService = app.AccommodationAndOwnerGradeService;
            AccommodationAndOwnerGradeService.Subscribe(this);
            Grades = new ObservableCollection<AccommodationAndOwnerGrade>(AccommodationAndOwnerGradeService.GetSeeableGrades());
        }
        public void Update()
        {
            Grades.Clear();
            foreach (AccommodationAndOwnerGrade g in AccommodationAndOwnerGradeService.GetSeeableGrades())
            {
                Grades.Add(g);
            }
        }
        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
