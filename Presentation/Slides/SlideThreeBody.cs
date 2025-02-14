using Canvas.Components;
using Canvas.Components.Interfaces.Mix;
using CSShaders.Shaders.Vectors;
using SkiaSharp;
using SlidePresenter;
using ThreeBody;
using ThreeBodyVisualisation;

namespace Presentation.Slides;

public sealed class SlideThreeBody : Slide
{
	private PhysicsBody[] _startConfig =
	[
		new() { Mass = 100, Position = new Vec2(-100, 0) },
		new() { Mass = 100, Position = new Vec2(0, 0) },
		new() { Mass = 100, Position = new Vec2(100, 0) }
	];

	private readonly ThreeBodyVisualiser _visualiser = new(timeStep: 0.1)
	{
		OrbitLength = 50
	};

	private readonly Equation _gravityFormula;

	public SlideThreeBody()
	{
		_visualiser.SetConfig(_startConfig);
		_gravityFormula = new Equation(@"F = \frac{G \cdot m_1 \cdot m_2}{d^2}", 0);

		#region Slide Content

		Canvas = new Canvas.Canvas(0, 0,
		[
			_visualiser.GetRelativeRectangleSizedRelativePositioned
			(
				width: 1,
				height: 1
			),
			_gravityFormula.GetRelativeSizedRelativePositioned
			(
				size: 0.2,
				x: 0.05,
				y: 0.05
			)
		])
		{
			BackgroundColor = Color.Black,
			FrameRate = 15
		};

		#endregion

		Actions.Add(() => { _gravityFormula.Color = SKColors.White; });

		Actions.Add(() =>
		{
			_visualiser.RunTime = 20;
			_visualiser.Start();
		});

		Actions.Add(() =>
		{
			_startConfig = ThreeBodySimulator.GenerateStableConfiguration();
			_visualiser.RunTime = 0;
			_visualiser.Reset(_startConfig);
			_visualiser.Start();
		});

		Actions.Add(() =>
		{
			_visualiser.RunTime = -1;
			_visualiser.Start();
		});
	}

	public override void OnLoad()
	{
		_startConfig =
		[
			new PhysicsBody { Mass = 100, Position = new Vec2(-100, 0) },
			new PhysicsBody { Mass = 100, Position = new Vec2(0, 0) },
			new PhysicsBody { Mass = 100, Position = new Vec2(100, 0) }
		];
		
		_gravityFormula.Color = SKColors.Black;

		_visualiser.Reset(_startConfig);
		_visualiser.RunTime = 0;
		_visualiser.Start();
	}

	public override void OnUnload()
	{
		_visualiser.Stop();
	}
}