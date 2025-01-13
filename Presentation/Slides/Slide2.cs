using System.Numerics;
using Canvas.Components.Interfaces;
using SlidePresenter;
using ThreeBody;
using ThreeBodyFractalVisualisation;

namespace Presentation.Slides;

public sealed class Slide2 : Slide
{
	public Slide2()
	{
		Canvas = new Canvas.Canvas(0, 0,
		[
			new RelativePositionedComponent<ThreeBodyFractalVisualiser>(new ThreeBodyFractalVisualiser(800, 800)
			{
				SimulationTime = 20f,
				StartConfig =
				[
					new PhysicsBody()
					{
						Mass = 100,
						Position = new Vector2(0, 100),
						Velocity = new Vector2(15, 0),
					},
					new PhysicsBody()
					{
						Mass = 100,
						Position = new Vector2(0, -100),
						Velocity = new Vector2(-7, 7),
					},
					new PhysicsBody()
					{
						Mass = 100,
						Position = new Vector2(0, 0),
						Velocity = new Vector2(-8, -8),
					}
				]
			})
			{
				X = 0.5f,
				Y = 0.5f,
				Centered = true
			}
		]);
	}
}