using Booking.Domain.ServiceInterfaces;
using Booking.Util;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for SuperOwner.xaml
    /// </summary>
    public partial class SuperOwner : Window
    {
        public IAccommodationAndOwnerGradeService _gradeService { get; set; }
        public SuperOwner()
        {
            InitializeComponent();
            this.DataContext = this;
            _gradeService = InjectorService.CreateInstance<IAccommodationAndOwnerGradeService>();
            NumberOfGrades.Text = _gradeService.GetNumberOfGrades().ToString();
            AverageGrade.Text = _gradeService.GetAverageGrade().ToString();
            IsSuper.Text = _gradeService.SuperWindowText();
        }
        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
