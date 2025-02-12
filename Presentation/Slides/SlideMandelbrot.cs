using Canvas.Components.Interfaces.Relative;
using SlidePresenter;
using Mandelbrot = Presentation.Components.Mandelbrot;

namespace Presentation.Slides;

public sealed class SlideMandelbrot : Slide
{
	private readonly Mandelbrot _mandelbrot;

	private readonly RelativeRectangleSizedRelativePositionedComponent<Mandelbrot> _mandelbrotComponent;

	public SlideMandelbrot()
	{
		_mandelbrot = new Mandelbrot
		{
			Width = 1920,
			Height = 1080,
			MandelBrotWidth = 5
		};

		_mandelbrotComponent = new RelativeRectangleSizedRelativePositionedComponent<Mandelbrot>(_mandelbrot)
		{
			X = 0,
			Y = 0,
			Width = 1,
			Height = 1
		};

		Canvas = new Canvas.Canvas(0, 0,
		[
			_mandelbrotComponent
		]);
		_mandelbrot.LoadPlaceholder();
	}

	public override void OnLoad()
	{
		_mandelbrot.UpdateImageThreading();
	}
}