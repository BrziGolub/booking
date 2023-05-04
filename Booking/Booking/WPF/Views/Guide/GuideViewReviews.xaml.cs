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
    public partial class GuideViewReviews : Window//, IObserver
    {
        public ObservableCollection<TourGrade> TourGrades { get; set; }
        public ITourGradeService _tourGradeService { get; set; }

        public TourGrade SelectedTourGrade { get; set; }
        public GuideViewReviews()
        {
            InitializeComponent();
            this.DataContext = new GuideViewReviewsViewModel(this);
            reviewsDataGrid.Items.Clear();
        }






    }
}
