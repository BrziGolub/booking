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

        public Voucher AddVoucher(Voucher voucher)
        {
            //Load();
            voucher.Id = NextId();
            _vouchers.Add(voucher);

            NotifyObservers();
            Save();

            return voucher;
        }

        public void Create(Voucher voucher)
        {
            AddVoucher(voucher);
        }

        public int NextId()
        {
            if (_vouchers.Count == 0)
            {
                return 1;
            }
            else
            {
                return _vouchers.Max(t => t.Id) + 1;
            }
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
