using Canvas.Components;
using Canvas.Components.Interfaces.Relative;
using SlidePresenter;

namespace Presentation.Slides;

public class SlideFibonacci : Slide
{
	public SlideFibonacci()
	{
		Canvas = new Canvas.Canvas(0, 0,
		[
			new RelativeRectangleSizedKeepAspectRatioRelativePositionedComponent<BitmapImage>(new BitmapImage("../../../assets/fib_tree.jpg", 0, 0, 0, 0))
			{
				X = 0.5f,
				Y = 0.5f,
				Centered = true,
				Size = 0.5,
				AspectRatio = 1.7777777777777777d
			}
		])
		{
			BackgroundColor = Color.Black
		};
	}
}