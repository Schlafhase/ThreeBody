using Canvas.Components.Interfaces.Relative;
using CSShaders.Shaders.Vectors;
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
		// TODO: Very pretty start config:
		// [
		// new PhysicsBody()
		// {
		// 	Mass = 100,
		// 	Position = new Vec2(0, 100),
		// 	Velocity = new Vec2(15, 0),
		// },
		// new PhysicsBody()
		// {
		// 	Mass = 100,
		// 	Position = new Vec2(0, -100),
		// 	Velocity = new Vec2(-7, 7),
		// },
		// new PhysicsBody()
		// 	{
		// 		Mass = 100,
		// 		Position = new Vec2(0, 0),
		// 		Velocity = new Vec2(-8, -8),
		// 	}
		// 	]

		List<Slide> slides =
		[
			new SlideSierpinski(),
			new SlideKoch(),
			new SlideFibonacci(),
			new SlideMandelbrot(),
			new SlideSelfSimilarity(),
			new SlideMandelbrotZoom()
		];

		// To customize application configuration such as set high DPI settings or default font,
		// see https://aka.ms/applicationconfiguration.
		ApplicationConfiguration.Initialize();
		Application.Run(new PresentationForm.PresentationForm(slides));
	}
}