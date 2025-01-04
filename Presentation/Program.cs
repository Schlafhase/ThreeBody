using Canvas.Components.Interfaces;
using Presentation.Slides;
using SlidePresenter;
using ThreeBodyVisualisation;

namespace Presentation;

internal static class Program
{

	/// <summary>
	///  The main entry point for the application.
	/// </summary>
	[STAThread]
	private static void Main()
	{
		ThreeBodyVisualiser threeBodyVisualiser = new();

		RelativePositionedComponent<ThreeBodyVisualiser> threeBodyVisualiserComponent =
			new RelativePositionedComponent<ThreeBodyVisualiser>(threeBodyVisualiser)
			{
				X = 0.5f,
				Y = 0.5f,
				Centered = true
			};

		List<Slide> slides =
		[
			new Slide1(),
			new Slide2(threeBodyVisualiserComponent),
			new Slide3(threeBodyVisualiser, threeBodyVisualiserComponent)
		];

		// To customize application configuration such as set high DPI settings or default font,
		// see https://aka.ms/applicationconfiguration.
		ApplicationConfiguration.Initialize();
		Application.Run(new PresentationForm.PresentationForm(slides));
	}
}