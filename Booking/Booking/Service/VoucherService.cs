using Booking.Model;
using Booking.Observer;
using Booking.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Service
{
    public class VoucherService
    {
        private readonly VoucherRepository _repository;
        private List<Voucher> _vouchers;

        private readonly List<IObserver> _observers;

        public VoucherService()
        {
            _repository = new VoucherRepository();
            _vouchers = new List<Voucher>();
            _observers = new List<IObserver>();

            Load();

        }

        public void Load()
        {
            _vouchers = _repository.Load();
        }

        public void Save()
        {
            _repository.Save(_vouchers);
        }

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }
        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _observers.Remove(observer);
        }

    }   
}
