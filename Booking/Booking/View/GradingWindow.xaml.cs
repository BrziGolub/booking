using Booking.Model;
using Booking.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for GradingWindow.xaml
    /// </summary>
    public partial class GradingWindow : Window
    {
        public event PropertyChangedEventHandler PropertyChanged;
        //public AccommodationReservationController reservationController;
        //public AccommodationGradeController gradeController;
        public AccommodationReservationService AccommodationReservationService { get; set; }
        public AccommodationGradeService AccommodationGradeService { get; set; }
        public AccommodationGrade accommodationGrade = new AccommodationGrade();
        int accommodationReservationId;
        private OwnerGradingGuests OwnerGradingGuests;

        public GradingWindow(int ReservationId, OwnerGradingGuests ownerGradingGuests)
        {
            InitializeComponent();
            accommodationReservationId = ReservationId;
            this.DataContext = this;
            var app = Application.Current as App;
            AccommodationReservationService = app.AccommodationReservationService;
            AccommodationGradeService = app.AccommodationGradeService;
            comboBoxCleanliness.ItemsSource = new List<int>() { 1, 2, 3, 4, 5 };
            comboBoxRuleFollowing.ItemsSource = new List<int>() { 1, 2, 3, 4, 5 };
            comboBoxCommunication.ItemsSource = new List<int>() { 1, 2, 3, 4, 5 };
            comboBoxLateness.ItemsSource = new List<int>() { 1, 2, 3, 4, 5 };
            this.OwnerGradingGuests = ownerGradingGuests;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _cleanliness;

        public int Cleanliness
        {
            get => _cleanliness;
            set
            {
                if (_cleanliness != value)
                {
                    _cleanliness = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _ruleFollowing;

        public int RuleFollowing
        {
            get => _ruleFollowing;
            set
            {
                if (_ruleFollowing != value)
                {
                    _ruleFollowing = value;
                    OnPropertyChanged();
                }
            }
        }

        public string _comment;
        public string Comment
        {
            get => _comment;
            set
            {
                if (_comment != value)
                {
                    _comment = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _communication;
        public int Communication
        {
            get => _communication;
            set
            {
                if (_communication != value)
                {
                    _communication = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _lateness;
        public int Lateness
        {
            get => _lateness;
            set
            {
                if (_lateness != value)
                {
                    _lateness = value;
                    OnPropertyChanged();
                }
            }
        }
        private void Button_Click_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_Grade(object sender, RoutedEventArgs e)
        {
            accommodationGrade.Cleanliness = Cleanliness;
            accommodationGrade.RuleFollowing = RuleFollowing;
            accommodationGrade.Comment = Comment;
            accommodationGrade.Communication = Communication;
            accommodationGrade.Lateness = Lateness;
            accommodationGrade.Accommodation = AccommodationReservationService.GetByID(accommodationReservationId);
            if (accommodationGrade.Cleanliness == -1)
            {
                MessageBox.Show("'CLEANLINESS' not entered");
            }
            else if (accommodationGrade.RuleFollowing == -1)
            {
                MessageBox.Show("'RULEFOLLOWING' not entered");
            }
            else if (accommodationGrade.Communication == -1)
            {
                MessageBox.Show("'Communication' not entered");
            }
            else if (accommodationGrade.Comment == null)
            {
                MessageBox.Show("'Comment' not entered");
            }
            else if (accommodationGrade.Lateness == -1)
            {
                MessageBox.Show("'LATENESS' not entered");
            }
            else
            {
                AccommodationGradeService.Create(accommodationGrade);
                MessageBox.Show("Grade successfully created");
                OwnerGradingGuests.RefreshWindow();
                this.Close();
            }
        }
    }
}
