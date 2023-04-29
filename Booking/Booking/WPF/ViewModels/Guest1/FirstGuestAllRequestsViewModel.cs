using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Observer;
using Booking.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.WPF.ViewModels.Guest1
{
    public class FirstGuestAllRequestsViewModel: IObserver
    {
        public ObservableCollection<AccommodationReservationRequests> Requests { get; set; }
        public IAccommodationReservationRequestService _requestsService { get; set; }

        public FirstGuestAllRequestsViewModel()
        {
            _requestsService = InjectorService.CreateInstance<IAccommodationReservationRequestService>();

            _requestsService.Subscribe(this);
            Requests = new ObservableCollection<AccommodationReservationRequests>(_requestsService.GetAll());
        }

        public void Update()
        {
            Requests.Clear();
            foreach (var request in _requestsService.GetAll())
            {
                Requests.Add(request);
            }
        }
    }
}
