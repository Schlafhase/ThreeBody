using Canvas.Components;
using Canvas.Components.Interfaces.Mix;
using CSShaders.Shaders.Vectors;
using SlidePresenter;
using ThreeBody;

namespace Presentation.Slides;

public sealed class SlideDistance : Slide
{
	private PhysicsBody[] _bodies = ThreeBodySimulator.GenerateStableConfiguration();
	private readonly BitmapImage _bitmap = new BitmapImage(new Bitmap(1, 1), 0, 0, 0, 0);

	public SlideDistance()
	{
		#region Slide Content

		Canvas = new Canvas.Canvas(0, 0,
		[
			_bitmap.GetRelativeRectangleSizedRelativePositioned
			(
				x: 0,
				y: 0,
				width: 1,
				height: 1
			)
		])
		{
			BackgroundColor = Color.Black
		};

		#endregion
	}
	
	public override void OnLoad()
	{
		_bodies[0].Position += new Vec2(-72, 42);
		
		_bitmap.Bitmap = ThreeBodySimulator.GetSimulationImage(_bodies,
															   Canvas.Width/2,
															   Canvas.Height/2,
															   20,
															   0.1,
															   true,
															   true);
	}
}