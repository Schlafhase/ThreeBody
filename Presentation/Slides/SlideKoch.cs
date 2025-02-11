using System.DirectoryServices.ActiveDirectory;
using Canvas.Components;
using Canvas.Components.Interfaces.Relative;
using SlidePresenter;
using ThreeBodyVisualisation;

namespace Presentation.Slides;

public sealed class SlideKoch : Slide
{
	private RelativeRectangleSizedKeepAspectRatioRelativePositionedComponent<BitmapImage> _image;

	public SlideKoch()
	{
		_image = new RelativeRectangleSizedKeepAspectRatioRelativePositionedComponent<BitmapImage>(
			new BitmapImage("../../../assets/koch.png", 0, 0, 0, 0))
		{
			X = 0.5f,
			Y = 0.5f,
			Centered = true,
			Size = 0.5,
			AspectRatio = 1.15d
		};

		Canvas = new Canvas.Canvas(0, 0,
		[
			_image
		])
		{
			BackgroundColor = Color.Black
		};
	}

}