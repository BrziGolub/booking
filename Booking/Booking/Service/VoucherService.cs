using Booking.Domain.RepositoryInterfaces;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Observer;
using Booking.Util;
using System.Collections.Generic;

namespace Booking.Service
{
	public class VoucherService : ISubject, IVoucherService
	{
		private readonly IVoucherRepository _voucherRepository;

		private readonly List<IObserver> _observers;

		public VoucherService()
		{
			_voucherRepository = InjectorRepository.CreateInstance<IVoucherRepository>();
			_observers = new List<IObserver>();
		}

		public Voucher AddVoucher(Voucher voucher)
		{
			return _voucherRepository.Add(voucher);
		}

		public void Create(Voucher voucher)
		{
			AddVoucher(voucher);
			NotifyObservers();
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

		public List<Voucher> GetAll()
		{
			return _voucherRepository.GetAll();
		}

		public List<Voucher> GetVouchersByUserId(int id)
		{
			return _voucherRepository.GetValidVouchersByUserId(id);
		}

		public void RedeemVoucher(Voucher voucher)
		{
			voucher.IsActive = false;
			_voucherRepository.Update(voucher);
		}
	}
}
