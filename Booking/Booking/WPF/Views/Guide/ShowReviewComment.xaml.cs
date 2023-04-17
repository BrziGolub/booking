using Booking.Domain.Model;
using Booking.Domain.ServiceInterfaces;
using Booking.Observer;
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
    public partial class ShowReviewComment : Window
    {
        public ITourGradeService _tourGradeService { get; set; }

        public ShowReviewComment(int idGrade)
        {
            InitializeComponent();
            _tourGradeService = InjectorService.CreateInstance<ITourGradeService>();
            TourGrade tg = _tourGradeService.GetById(idGrade);
            tbComment.Text = tg.Comment;
        }
    }
}
