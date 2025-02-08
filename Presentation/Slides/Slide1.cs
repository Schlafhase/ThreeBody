using Canvas.Components;
using Canvas.Components.Interfaces.Relative;
using SlidePresenter;
using Mandelbrot = Presentation.Components.Mandelbrot;
using Rectangle = Canvas.Components.Rectangle;

namespace Presentation.Slides;

public sealed class Slide1 : Slide
{
	private static readonly Mandelbrot _mandelbrot = new()
	{
		Width = 100,
		Height = 100,
	};

	private static readonly RelativePositionedComponent<Rectangle>
		_mandelbrotComponent = new(new Rectangle(0, 0, 100, 100, Color.Blue))
		{
			X = 0,
			Y = 0,
		};

	public Slide1()
	{
		Canvas = new Canvas.Canvas(0, 0,
		[
			_mandelbrotComponent,
			new RelativeSizedRelativePositionedComponent<Text>(new Text("Toucheing gras totorial", FontFamily.GenericSansSerif, 0), RelativeSizingOptions.Width)
			{
				Size = 0.04,
				Y = 0.1,
				Margin = 10
			},
			new RelativeRectangleSizedKeepAspectRatioRelativePositionedComponent<BitmapImage>(new BitmapImage("C:\\Users\\linus\\OneDrive\\Pictures\\touching grass.jpg", 0, 0, 0, 0))
			{
				Size = 0.5,
				Y = 0.5,
				AspectRatio = 2,
				Margin = 10
			}
		]);
	}

	public override void OnLoad()
	{
		_mandelbrot.UpdateImageThreading();
	}
}