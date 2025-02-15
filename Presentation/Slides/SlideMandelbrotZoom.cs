using Canvas.Components;
using Canvas.Components.Interfaces.Mix;
using SlidePresenter;

namespace Presentation.Slides;

public sealed class SlideMandelbrotZoom : Slide
{
	public SlideMandelbrotZoom()
	{
		#region Slide Content

		Canvas = new Canvas.Canvas(0, 0,
		[
			new BitmapImage("../../../assets/minibrot.png", 0, 0, 0, 0)
				.GetRelativeRectangleSizedKeepAspectRatioRelativePositioned
				(
					x: 0.5,
					y: 0.5,
					size: 1,
					aspectRatio: 1,
					centered: true
				)
		])
		{
			BackgroundColor = Color.Black
		};

		#endregion
	}
}