using Booking.Commands;
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
    public class GradingWindowViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public IAccommodationReservationService AccommodationReservationService { get; set; }
        public IAccommodationGradeService AccommodationGradeService { get; set; }

        public AccommodationGrade accommodationGrade = new AccommodationGrade();
        int accommodationReservationId;
        public ObservableCollection<int> comboBoxCleanliness { get; set; }
        public ObservableCollection<int> comboBoxRuleFollowing { get; set; }
        public ObservableCollection<int> comboBoxCommunication { get; set; }
        public ObservableCollection<int> comboBoxLateness { get; set; }
        public RelayCommand Grade { get; set; }
        public RelayCommand Close { get; set; }

        private readonly Window _window;

        public GradingWindowViewModel(int ReservationId, Window window)
        {
            _window = window;
            accommodationReservationId = ReservationId;
            AccommodationReservationService = InjectorService.CreateInstance<IAccommodationReservationService>();
            AccommodationGradeService = InjectorService.CreateInstance<IAccommodationGradeService>();
            setComboBoxes();
            SetCommands();
        }
        public void setComboBoxes()
        {
            List<int> grades = new List<int>() { 1, 2, 3, 4, 5 };
            comboBoxCleanliness = new ObservableCollection<int>(grades);
            comboBoxRuleFollowing = new ObservableCollection<int>(grades);
            comboBoxCommunication = new ObservableCollection<int>(grades);
            comboBoxLateness = new ObservableCollection<int>(grades);
        }
        public void SetCommands()
        {
            Grade = new RelayCommand(Button_Click_Grade);
            Close = new RelayCommand(Button_Click_Close);

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
        private void Button_Click_Close(object param)
        {
            _window.Close();
        }

        private void Button_Click_Grade(object param)
        {
            accommodationGrade.Cleanliness = Cleanliness;
            accommodationGrade.RuleFollowing = RuleFollowing;
            accommodationGrade.Comment = Comment;
            accommodationGrade.Communication = Communication;
            accommodationGrade.Lateness = Lateness;
            accommodationGrade.Accommodation = AccommodationReservationService.GetById(accommodationReservationId);
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
                AccommodationReservationService.NotifyObservers();
                _window.Close();
            }
        }
    }
}
