using Canvas.Components.Interfaces;
using SlidePresenter;
using Rectangle = Canvas.Components.Rectangle;

namespace Presentation.Slides;

public sealed class Slide1 : Slide
{
	public Slide1()
	{
		Canvas = new Canvas.Canvas(0, 0, [new RelativePositionedComponent<Rectangle>(new Rectangle(0, 0, 100, 100, Color.Red))
		{
			X = 0.1f,
			Y = 0.1f,
			Margin = 10
		}]);
	}
}