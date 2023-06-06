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
using System.Windows;

namespace Booking.Application.UseCases
{
    public class SuperGuideService : ISuperGuideService
    {
        private readonly ISuperGuideRepository _repository;
        private readonly ITourRepository _tourRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITourGradeRepository _tourGradeRepository;

        public SuperGuideService()
        {
            _repository = InjectorRepository.CreateInstance<ISuperGuideRepository>();
            _tourRepository = InjectorRepository.CreateInstance<ITourRepository>();
            _userRepository = InjectorRepository.CreateInstance<IUserRepository>();
            _tourGradeRepository = InjectorRepository.CreateInstance<ITourGradeRepository>();
        }

        public void UpdateSuperGuideStatus(string language)
        {
                int SignedGuideId = TourService.SignedGuideId;
                // Get the tours of the guide from the tour repository for the last year
                List<Tour> guideTours = _tourRepository.GetToursByGuideAndYearAndLanguage(SignedGuideId, DateTime.Now.Year - 1, language);

                int tourCount = guideTours.Count;

                // Check if the guide meets the criteria for becoming a super-guide
                if (tourCount < 20)
                {
                    MessageBox.Show($"You don't have at least 20 tours in {language} in the last year.", "Insufficient Tours", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return; // maybe return;
                }

                double averageGrade = 0;
                int totalGrades = 0;

                foreach (Tour tour in guideTours)
                {
                    TourGrade grade = _tourGradeRepository.GetByTourId(tour.Id);
                    if (grade != null)
                    {
                        int sumGrades = grade.KnowledgeGuideGrade + grade.LanguageGuideGrade + grade.InterestOfTourGrade;
                        averageGrade += (double)sumGrades / 3;
                        totalGrades++;
                    }
                }

                averageGrade /= totalGrades;
                // Check if the average grade is above 4.0 // ALL GOOD TO THIS

                if (averageGrade > 4.0)
                {
                    // Set the super-guide status for the guide
                    SuperGuide superGuide = _repository.GetSuperBySignedGuideId(SignedGuideId); // signed
                    if (superGuide == null)
                    {
                        superGuide = new SuperGuide
                        {
                            Guide = _userRepository.GetById(SignedGuideId), // signed guide
                            Language = language,
                            AverageGrade = averageGrade,
                            NumberOfTours = tourCount
                        };
                        _repository.Add(superGuide);
                    _userRepository.UpdateSuper(_userRepository.GetById(SignedGuideId));
                    MessageBox.Show("Congratulations, you have become super-guide! ");
                    }
                    else
                    {
                        superGuide.Language = language;
                        superGuide.AverageGrade = averageGrade;
                        superGuide.NumberOfTours = tourCount;
                        _repository.Update(superGuide);
                    MessageBox.Show("You are already super-guide on this language! ");
                }
                }
               
                /*else
                {
                    // Remove the super-guide status for the guide
                    SuperGuide superGuide = _repository.GetSuperBySignedGuideId(SignedGuideId);
                    if (superGuide != null)
                    {
                        _repository.Delete(superGuide);
                    }
                }*/

            
        }
    }
}
