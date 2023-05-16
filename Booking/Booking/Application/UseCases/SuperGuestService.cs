using Booking.Domain.Model;
using Booking.Domain.RepositoryInterfaces;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Repository;
using Booking.Service;
using Booking.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.UseCases
{
    public class SuperGuestService: ISuperGuestService
    {
        public readonly ISuperGuestRepository _repository;
        public readonly IUserRepository _userRepository;
        public readonly IAccommodationResevationRepository _accommodationResevationRepository;
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly ILocationRepository _locationRepository;

        public SuperGuestService()
        {
            _repository = InjectorRepository.CreateInstance<ISuperGuestRepository>();
            _userRepository = InjectorRepository.CreateInstance<IUserRepository>();
            _accommodationResevationRepository = InjectorRepository.CreateInstance<IAccommodationResevationRepository>();
            _accommodationRepository = InjectorRepository.CreateInstance<IAccommodationRepository>();
            _locationRepository = InjectorRepository.CreateInstance<ILocationRepository>();
        }

        public List<AccommodationReservation> GetGeustsReservatonst(User SignedGuest)
        {
            List<AccommodationReservation> _guestsReservations = new List<AccommodationReservation>();

            foreach (var reservation in _accommodationResevationRepository.GetAll())
            {
                if (reservation.Guest.Id == SignedGuest.Id)
                {
                    reservation.Accommodation = _accommodationRepository.GetById(reservation.Accommodation.Id);
                    reservation.Accommodation.Location = _locationRepository.GetById(reservation.Accommodation.Location.Id);
                    reservation.Accommodation.Owner = _userRepository.GetById(reservation.Accommodation.Owner.Id);
                    reservation.Guest = _userRepository.GetById(SignedGuest.Id);
                    _guestsReservations.Add(reservation);
                }
            }
            return _guestsReservations;
        }

        public DateTime SetActivationDate(User SignedGuest)
        {
            List<AccommodationReservation> _guestsReservations = GetGeustsReservatonst(SignedGuest);

            _guestsReservations = _guestsReservations.OrderBy(r => r.DepartureDay.AddYears(1).Year > DateTime.Now.Year).ThenBy(r => r.DepartureDay.Date).ToList();

            return _guestsReservations[10].DepartureDay; 
        }

        public void CreateSuperGuest(User signedGuest, int numberOfReservations)
        {
            signedGuest.Super = 1;
            _userRepository.Update(signedGuest);

            SuperGuest superGuest = new SuperGuest();
            superGuest.BonusPoints = 5;
            superGuest.NumberOfReservations = numberOfReservations;
            superGuest.ActivationDate = SetActivationDate(signedGuest);
            superGuest.Guest = signedGuest;

            _repository.Add(superGuest);
        }

        public void CheckNumberOfReservations(User SignedGuest)
        {
            List<AccommodationReservation> _guestsReservations = GetGeustsReservatonst(SignedGuest);

            int countNumberOfReservations = 0;

            foreach(var reservation in _guestsReservations)
            {
                if(reservation.DepartureDay.AddYears(1).Year > DateTime.Now.Year)
                {
                    countNumberOfReservations++;
                }
            }

            if(countNumberOfReservations >= 10)
            {
                CreateSuperGuest(SignedGuest, countNumberOfReservations);
            }
        }

        public void SetOrdinaryGuest(SuperGuest superGuest, User signedGuest)
        {
            _repository.Delete(superGuest);
            signedGuest.Super = 0;
            _userRepository.Update(signedGuest);
        }

        public void CheckActivationDate(User SignedGuest)
        {
            SuperGuest superGuest = _repository.GetById(SignedGuest.Id);

            if(superGuest.ActivationDate.AddYears(1) > DateTime.Now)
            {
                CheckNumberOfReservations(SignedGuest);
            }
            SetOrdinaryGuest(superGuest, SignedGuest);
        }

        public void CheckSuperGuest()
        {
            User SignedGuest = _userRepository.GetById(AccommodationReservationService.SignedFirstGuestId);

            if(SignedGuest.Super != 1)
            {
                CheckNumberOfReservations(SignedGuest);
            }

            CheckActivationDate(SignedGuest);
        }

    }
}
