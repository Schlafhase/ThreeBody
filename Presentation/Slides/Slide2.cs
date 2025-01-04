using Canvas.Components.Interfaces;
using SlidePresenter;
using ThreeBodyVisualisation;
using Rectangle = Canvas.Components.Rectangle;

namespace Presentation.Slides;

public sealed class Slide2 : Slide
{
	public Slide2(RelativePositionedComponent<ThreeBodyVisualiser> threeBodyVisualiserComponent)
	{
		Canvas = new Canvas.Canvas(0, 0,
		[
			threeBodyVisualiserComponent,
			new RelativePositionedComponent<Rectangle>(new Rectangle(0, 0, 100, 100, Color.Blue))
			{
				X = 0.5f,
				Y = 0.5f,
				Margin = 10,
				Centered = true
			}
		]);
	}
}