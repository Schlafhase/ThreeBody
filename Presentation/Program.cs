using System.Numerics;
using System.Security.Cryptography;
using Canvas.Components.Interfaces;
using Presentation.Slides;
using SlidePresenter;
using ThreeBody;
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
		PhysicsBody[] config =
		[
			new PhysicsBody()
			{
				Mass = 100,
				Position = new Vector2(0, 100),
				Velocity = new Vector2(15, 0),
			},
			new PhysicsBody()
			{
				Mass = 100,
				Position = new Vector2(0, -100),
				Velocity = new Vector2(-7, 7),
			},
			new PhysicsBody()
			{
				Mass = 100,
				Position = new Vector2(0, 0),
				Velocity = new Vector2(-8, -8),
			}
		];
		threeBodyVisualiser.SetConfig(config);

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
			new Slide2(),
			new Slide3(threeBodyVisualiser, threeBodyVisualiserComponent)
		];

		// To customize application configuration such as set high DPI settings or default font,
		// see https://aka.ms/applicationconfiguration.
		ApplicationConfiguration.Initialize();
		Application.Run(new PresentationForm.PresentationForm(slides));
	}
}