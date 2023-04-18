using Booking.Domain.Model;
using Booking.Domain.RepositoryInterfaces;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Booking.Service
{
    public class NotificationService: INotificationService
    {
        public INotificationRepository _repository;
        private readonly IAccommodationResevationRepository _reservationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAccommodationRepository _accommodationRepository;
        public NotificationService()
        {
            _repository = InjectorRepository.CreateInstance<INotificationRepository>();
            _reservationRepository = InjectorRepository.CreateInstance<IAccommodationResevationRepository>();
            _userRepository = InjectorRepository.CreateInstance<IUserRepository>();
            _accommodationRepository = InjectorRepository.CreateInstance<IAccommodationRepository>();
        }

        public List<Notification> GetUserNotifications(User user)
        {
            List<Notification> ownerNotifications = new List<Notification>();

            foreach (var notification in _repository.GetAll())
            {
                if (notification.User.Id == user.Id)
                {
                    ownerNotifications.Add(notification);
                }
            }
            return ownerNotifications;
        }

        public void ChangeNotificationState(Notification notification)
        {
            notification.IsRead = true;
            _repository.SaveChanges(notification);
        }
        public void MakeReject(AccommodationReservationRequests SelectedAccommodationReservationRequest) 
        {
            SelectedAccommodationReservationRequest.AccommodationReservation = _reservationRepository.GetById(SelectedAccommodationReservationRequest.AccommodationReservation.Id);
            SelectedAccommodationReservationRequest.AccommodationReservation.Accommodation = _accommodationRepository.GetById(SelectedAccommodationReservationRequest.AccommodationReservation.Accommodation.Id);
            SelectedAccommodationReservationRequest.AccommodationReservation.Accommodation.Owner = _userRepository.GetById(SelectedAccommodationReservationRequest.AccommodationReservation.Accommodation.Owner.Id);
            Notification rejectNotification = new Notification();
            rejectNotification.IsRead = false;
            rejectNotification.User.Id = SelectedAccommodationReservationRequest.AccommodationReservation.Guest.Id;
            rejectNotification.Text = "The date move request for " + SelectedAccommodationReservationRequest.AccommodationReservation.Accommodation.Name + " is rejected Owner:" + SelectedAccommodationReservationRequest.AccommodationReservation.Accommodation.Owner.Username;
            _repository.Add(rejectNotification);
        }
        public void MakeAccepted(AccommodationReservationRequests SelectedAccommodationReservationRequest)
        {
            SelectedAccommodationReservationRequest.AccommodationReservation = _reservationRepository.GetById(SelectedAccommodationReservationRequest.AccommodationReservation.Id);
            SelectedAccommodationReservationRequest.AccommodationReservation.Accommodation = _accommodationRepository.GetById(SelectedAccommodationReservationRequest.AccommodationReservation.Accommodation.Id);
            SelectedAccommodationReservationRequest.AccommodationReservation.Accommodation.Owner = _userRepository.GetById(SelectedAccommodationReservationRequest.AccommodationReservation.Accommodation.Owner.Id);
            Notification rejectNotification = new Notification();
            rejectNotification.IsRead = false;
            rejectNotification.User.Id = SelectedAccommodationReservationRequest.AccommodationReservation.Guest.Id;
            rejectNotification.Text = "The date move request for " + SelectedAccommodationReservationRequest.AccommodationReservation.Accommodation.Name + " was accepted new dates are "+SelectedAccommodationReservationRequest.NewArrivalDay.ToShortDateString()+" - "+SelectedAccommodationReservationRequest.NewDeparuteDay.ToShortDateString()+ " Owner:" + SelectedAccommodationReservationRequest.AccommodationReservation.Accommodation.Owner.Username;
            _repository.Add(rejectNotification);
        }

        public void SendNotification(List<Notification> notifications, User user)
        {
            foreach (Notification notification in notifications)
            {
                if (notification.IsRead == false)
                {
                    MessageBox.Show(notification.Text);
                    ChangeNotificationState(notification);
                }
            }
        }
        public void CreateCancellationNotification(Notification notification)
        {
            _repository.Add(notification);
        }
    }
}
