using Canvas.Components;
using Canvas.Components.Interfaces.Relative;
using SlidePresenter;

namespace Presentation.Slides;

public sealed class SlideSierpinski : Slide
{
	public SlideSierpinski()
	{
		Canvas = new Canvas.Canvas(0, 0,
		[
			new RelativeRectangleSizedKeepAspectRatioRelativePositionedComponent<BitmapImage>(new BitmapImage("../../../assets/sierpinski.png", 0, 0, 0, 0))
			{
				X = 0.5f,
				Y = 0.5f,
				Centered = true,
				Size = 0.5,
				AspectRatio = 1.1549566891241578d
			}
		])
		{
			BackgroundColor = Color.Black
		};
	}
}