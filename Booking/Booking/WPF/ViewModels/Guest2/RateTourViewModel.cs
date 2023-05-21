using Booking.Commands;
using Booking.Domain.Model;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Service;
using Booking.Util;
using System.Windows;

namespace Booking.WPF.ViewModels.Guest2
{
	public class RateTourViewModel
	{
		private readonly Window _window;
		private ITourGradeService _gradeService;

		public Tour Tour { get; set; }
		public int GuidesKnowledge { get; set; }
		public int GuidesLanguage { get; set; }
		public int Amusement { get; set; }
		public string Comment { get; set; } = string.Empty;

		public RelayCommand Button_Click_Rate { get; set; }

		public RateTourViewModel(Window window, Tour tour)
		{
			_window = window;
			Tour = tour;

			_gradeService = InjectorService.CreateInstance<ITourGradeService>();

			InitializeCommands();
		}

		private void InitializeCommands()
		{
			Button_Click_Rate = new RelayCommand(RateTourAndGuide);
		}

		private TourGrade GenerateGrade()
		{
			TourGrade tourGrade = new TourGrade();
			tourGrade.Tour = Tour;
			tourGrade.Guide.Id = Tour.GuideId;
			tourGrade.Guest.Id = TourService.SignedGuideId;
			tourGrade.KnowledgeGuideGrade = GuidesKnowledge;
			tourGrade.LanguageGuideGrade = GuidesLanguage;
			tourGrade.InterestOfTourGrade = Amusement;
			tourGrade.Comment = Comment;

			return tourGrade;
		}

		private void RateTourAndGuide(object param)
		{
			if (IsInputValid())
			{
				_gradeService.AddTourGrade(GenerateGrade());
				_window.Close();
			}
			else
			{
				MessageBox.Show("Invalid input!");
			}
		}

		private bool IsInputValid()
		{
			bool knowledge = GuidesKnowledge >= 1 && GuidesLanguage <= 5;
			bool language = GuidesLanguage >= 1 && GuidesLanguage <= 5;
			bool amuse = Amusement >= 1 && Amusement <= 5;
			return knowledge && language && amuse;
		}
	}
}
