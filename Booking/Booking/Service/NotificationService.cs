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
        public NotificationService()
        {
            _repository = InjectorRepository.CreateInstance<INotificationRepository>();
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
