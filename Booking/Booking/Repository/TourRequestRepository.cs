﻿using Booking.Domain.Model;
using Booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Repository
{
    public class TourRequestRepository : ITourRequestRepository
    {
        private const string FilePath = "../../Resources/Data/tourRequests.csv";
        private readonly Serializer<TourRequest> _serializer;
        private List<TourRequest> _tourRequests;

        public TourRequestRepository()
        {
            _serializer = new Serializer<TourRequest>();
            _tourRequests = _serializer.FromCSV(FilePath);
        }
        public List<TourRequest> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public TourRequest GetById(int id)
        {
            return _tourRequests.Find(tr => tr.Id == id);
        }

        public string GetMostPopularLanguageInLastYear()
        {
            DateTime startDate = DateTime.Now.AddYears(-1);
            DateTime endDate = DateTime.Now;

            var tourRequestsInRange = _tourRequests.Where(tr => tr.CreatedDate >= startDate && tr.CreatedDate <= endDate);

            string mostPopularLanguage = null;
            int maxCount = 0;

            foreach (var tourRequest in tourRequestsInRange)
            {
                string language = tourRequest.Language;

                int languageCount = 0;

                foreach (var otherTourRequest in tourRequestsInRange)
                {
                    if (otherTourRequest.Language == language)
                    {
                        languageCount++;
                    }
                }

                if (languageCount > maxCount)
                {
                    maxCount = languageCount;
                    mostPopularLanguage = language;
                }
            }

            return mostPopularLanguage;
        }

        public int GetMostPopularLocationIdInLastYear()
        {
            DateTime startDate = DateTime.Now.AddYears(-1);
            DateTime endDate = DateTime.Now;

            var tourRequestsInRange = _tourRequests.Where(tr => tr.CreatedDate >= startDate && tr.CreatedDate <= endDate);

            int? mostPopularLocationId = null;
            int maxCount = 0;

            foreach (var tourRequest in tourRequestsInRange)
            {
                int locationId = tourRequest.Location.Id;

                int locationCount = 0;

                foreach (var otherTourRequest in tourRequestsInRange)
                {
                    if (otherTourRequest.Location.Id == locationId)
                    {
                        locationCount++;
                    }
                }

                if (locationCount > maxCount)
                {
                    maxCount = locationCount;
                    mostPopularLocationId = locationId;
                }
            }

            return mostPopularLocationId ?? -1;
        }
    }
}