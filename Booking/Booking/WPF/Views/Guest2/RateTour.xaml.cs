using Booking.Domain.Model;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Service;
using Booking.Util;
using System.Windows;

namespace Booking.WPF.Views.Guest2
{
	public partial class RateTour : Window
	{
		private ITourGradeService _gradeService;

		public Tour Tour { get; set; }
		public int GuidesKnowledge { get; set; }
		public int GuidesLanguage { get; set; }
		public int Amusement { get; set; }
		public string Comment { get; set; } = string.Empty;

		public RateTour(Tour tour)
		{
			InitializeComponent();
			DataContext = this;

			_gradeService = InjectorService.CreateInstance<ITourGradeService>();

			Tour = tour;
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

		private void RateTourAndGuide(object sender, RoutedEventArgs e)
		{
			if (IsInputValid())
			{
				_gradeService.AddTourGrade(GenerateGrade());
				Close();
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