using Booking.Commands;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Service;
using Booking.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Booking.WPF.ViewModels.Guest1
{
    public class SuggestedDatesQuickSearchViewModel : INotifyPropertyChanged
    {
         public ObservableCollection<DatesDTO> Ranges { get; set; }

         public event PropertyChangedEventHandler PropertyChanged;

        public IAccommodationReservationService _accResService;

         public AccommodationDTO DTO;

         public DatesDTO selectedDates { get; set; }
         public RelayCommand BookCommand { get; set; }

         public String AccommodationLabel { get; set; }

         public SuggestedDatesQuickSearchViewModel(AccommodationDTO dto)
         {
             Ranges = new ObservableCollection<DatesDTO>(dto.dates);
             DTO = dto;
            _accResService = InjectorService.CreateInstance<IAccommodationReservationService>();
            BookCommand = new RelayCommand(Button_Click_Book);
            AccommodationLabel = dto.accommodation.Name + "-" + dto.accommodation.Location.State + "-" + dto.accommodation.Location.City + dto.accommodation.Location.State;
        }

         protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
         {
             PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
         }

         private void Button_Click_Book(object param)
         {
            bool IsReserved = _accResService.BookAccommodation(selectedDates.InitialDate, selectedDates.EndDate, DTO.accommodation);
        }

    }
}
