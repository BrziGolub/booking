using Booking.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Model.DAO
{
    public class AccommodationReservationDAO
    {
        private readonly AccommodationResevationRepository _accommodationResevationRepository;

        private List<AccommodationReservation> reservations;

        public AccommodationReservationDAO()
        {
            reservations = new List<AccommodationReservation>();
            _accommodationResevationRepository = new AccommodationResevationRepository();
            Load();
        }

        public void Load()
        {
            reservations = _accommodationResevationRepository.Load();
        }

        public AccommodationReservation GetByID(int id)
        {
            return reservations.Find(reservation => reservation.Id == id);
        }

        public List<AccommodationReservation> GetAll()
        {
            return reservations;
        }


        public int GenerateId()
        {
            if (reservations.Count == 0) return 0;
            return reservations.Max(s => s.Id) + 1;
        }

         
        public void SaveReservation(AccommodationReservation reservation)
        {
            reservation.Id = GenerateId();
            reservations.Add(reservation);
            _accommodationResevationRepository.Save(reservations);
        }

        public Boolean IsAvailableReservation(AccommodationReservation entered, AccommodationReservation existed)
        {
            if(entered.ArrivalDay >= existed.ArrivalDay && entered.ArrivalDay <= existed.DepartureDay)
            {
                return false;
            }
            else if(entered.DepartureDay > existed.ArrivalDay && entered.DepartureDay <= existed.DepartureDay)
            {
                return false;
            }
            return true;
        }


        public Boolean Reserve(DateTime ArrivalDay,DateTime DepartureDay,Accommodation SelectedAccommodation)
        {
            AccommodationReservation reservation = new AccommodationReservation();
            reservation.ArrivalDay = ArrivalDay;
            reservation.DepartureDay = DepartureDay;
            reservation.Accommodation = SelectedAccommodation;

            int difference = (DepartureDay- ArrivalDay).Days;

            if (difference <= SelectedAccommodation.MinNumberOfDays || DepartureDay < ArrivalDay)
            {
                return false;
            }

            int helper = 0;

            foreach(AccommodationReservation r in reservations)
            {
                if(r.Accommodation.Id == SelectedAccommodation.Id)
                {
                    if (IsAvailableReservation(r, reservation) == false)
                    {
                        helper++;
                    }
                }
            }

            if(helper == 0)
            {
                SaveReservation(reservation); //write reservation to file 
                return true;
            }
            else
            {
                //unsuccessful reservation
                return false;
            }
        }

        public List<DateTime> setReservedDates(DateTime ArrivalDay, DateTime DepartureDay,Accommodation selected)
        {
            //this is list od reserved days
            List<DateTime> reservedDates = new List<DateTime>();

            foreach (AccommodationReservation r in reservations)
            {
                if (r.Accommodation.Id == selected.Id)
                {
                       for(DateTime date = r.ArrivalDay; date <= r.DepartureDay; date = date.AddDays(1))
                       {
                            reservedDates.Add(date);
                       }
                }
            }
            //throw out duplicates
            List<DateTime> uniqueReservedDatesList = reservedDates.Distinct().ToList();

            //sort in ascending order
            uniqueReservedDatesList.Sort();
            return uniqueReservedDatesList;
        }

        public List<(DateTime, DateTime)> GetDates(List<DateTime> reservedDates,int difference)
        {
            List<(DateTime, DateTime)> rangeOfDates = new List<(DateTime, DateTime)>();

            //add range after lastDate from list 
            DateTime lastDate = reservedDates[reservedDates.Count - 1];
            rangeOfDates.Add((lastDate, lastDate.AddDays(difference)));

            //add range before firstDate from list
            DateTime firstDate = reservedDates[0];
            DateTime beforeFirst = firstDate.AddDays(-difference);

            if (beforeFirst > DateTime.Now)
            {
                rangeOfDates.Add((firstDate.AddDays(-difference), firstDate));
            }

            //find other ranges
            for(int i = 0; i < reservedDates.Count - 1; i++)
            {
                int find_difference = (reservedDates[i + 1] - reservedDates[i]).Days;

                if (difference <= find_difference)
                {   
                    
                    if (difference == find_difference)
                    {
                        //if there is exactly the number of days we need 
                        rangeOfDates.Add((reservedDates[i], reservedDates[i + 1]));
                    }
                    else
                    {
                        int help = Math.Abs(difference - find_difference);
                        
                        if(help == difference)
                        {

                            //if this is 10 - 5 = 5 equal to the difference, one more range can be made
                            rangeOfDates.Add((reservedDates[i].AddDays(difference), reservedDates[i + 1]));
                        }
                        else
                        {
                            rangeOfDates.Add((reservedDates[i], reservedDates[i + 1].AddDays(-help)));
                        }
                        
                    }
                    
                }
            }
            return rangeOfDates;
        }

        public List<(DateTime, DateTime)> SuggestOtherDates(DateTime ArrivalDay, DateTime DepartureDay,Accommodation SelectedAccommodation)
        {
            int difference = (DepartureDay - ArrivalDay).Days;

            List<DateTime> reservedDates= new List<DateTime>();
            reservedDates = setReservedDates(ArrivalDay,DepartureDay, SelectedAccommodation);

            return GetDates(reservedDates,difference); //return the list of ranges
        }
    }
}
