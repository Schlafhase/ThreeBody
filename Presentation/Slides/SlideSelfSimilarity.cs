using Canvas.Components;
using Canvas.Components.Interfaces.Mix;
using Canvas.Components.Interfaces.Relative;
using SlidePresenter;

namespace Presentation.Slides;

public class SlideSelfSimilarity : Slide
{
	private BitmapImage _image;

	public SlideSelfSimilarity()
	{
		_image = new BitmapImage("../../../assets/koch0.png", 0, 0, 0, 0);

		// RelativeRectangleSizedKeepAspectRatioRelativePositionedComponent<BitmapImage> imageComponent =
		// 	new(_image)
		// 	{
		// 		X = 0.5f,
		// 		Y = 0.5f,
		// 		Centered = true,
		// 		Size = 0.5,
		// 		AspectRatio = 1.15d
		// 	};

		Actions.Add(() => _image.Bitmap = Image.FromFile("../../../assets/koch1.png") as Bitmap);
		Actions.Add(() => _image.Bitmap = Image.FromFile("../../../assets/koch2.png") as Bitmap);
		Actions.Add(() => _image.Bitmap = Image.FromFile("../../../assets/koch3.png") as Bitmap);
		Actions.Add(() => _image.Bitmap = Image.FromFile("../../../assets/koch4.png") as Bitmap);
		Actions.Add(() => _image.Bitmap = Image.FromFile("../../../assets/koch5.png") as Bitmap);
		Actions.Add(() => _image.Bitmap = Image.FromFile("../../../assets/koch6.png") as Bitmap);

		#region Slide Content

		Canvas = new Canvas.Canvas(0, 0,
		[
			_image.GetRelativeRectangleSizedKeepAspectRatioRelativePositioned(0.5, 0.5, 0.5, 1.15d, centered: true)
		])
		{
			BackgroundColor = Color.Black
		};

		#endregion
	}

	public override void OnLoad()
	{
		_image.Bitmap = Image.FromFile("../../../assets/koch0.png") as Bitmap;
	}
}