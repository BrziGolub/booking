using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Booking.WPF.ViewModels.Guest1
{
    public class FirstGuestProfileViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string _numberOfReservations;
        public string NumberOfReservations
        {
            get => _numberOfReservations;
            set
            {
                if (_numberOfReservations != value)
                {
                    _numberOfReservations = value;
                    OnPropertyChanged();
                }

            }   
        }

        public string _typeOfGuest;
        public string TypeOfGuest
        {
            get => _typeOfGuest;
            set
            {
                if (_typeOfGuest != value)
                {
                    _typeOfGuest = value;
                    OnPropertyChanged();
                }

            }
        }

        public string _bonusPoints;
        public string BonusPoints
        {
            get => _bonusPoints;
            set
            {
                if (_bonusPoints != value)
                {
                    _bonusPoints = value;
                    OnPropertyChanged();
                }

            }
        }

        public bool _starImageVisibility;
        public bool StarImageVisibility
        {
            get => _starImageVisibility;
            set
            {
                if (_starImageVisibility != value)
                {
                    _starImageVisibility = value;
                    OnPropertyChanged();
                }

            }
        }

        public FirstGuestProfileViewModel()
        {
            //proba
            NumberOfReservations = "10";
            TypeOfGuest = "SUPER";
            BonusPoints = "5";
            StarImageVisibility = true;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
