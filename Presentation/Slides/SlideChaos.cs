using System.Security.AccessControl;
using Canvas.Components.AnimationUtilities;
using Canvas.Components.Interfaces.Mix;
using CSShaders.Shaders.Vectors;
using SlidePresenter;
using ThreeBody;
using ThreeBodyVisualisation;
using Rectangle = Canvas.Components.Rectangle;

namespace Presentation.Slides;

public sealed class SlideChaos : Slide
{
	private ThreeBodyVisualiser _startConfigVisualiser;
	private ThreeBodyVisualiser _chaosVisualiser;
	private double _runTime = 20;
	private PhysicsBody[] _startConfig = ThreeBodySimulator.GenerateStableConfiguration();
	public SlideChaos()
	{
		_startConfigVisualiser = new ThreeBodyVisualiser(0);
		_chaosVisualiser = new ThreeBodyVisualiser(_runTime)
		{
			RunInstant = true,
			OrbitLength = int.MaxValue,
			TimeStep = 0.001
		};

		#region Slide Content

		Canvas = new Canvas.Canvas(0, 0,
		[
			_startConfigVisualiser.GetRelativeRectangleSizedRelativePositioned(0, 0, 0.5, 1),
			_chaosVisualiser.GetRelativeRectangleSizedRelativePositioned(0.5, 0, 0.5, 1),
			new Rectangle(0, 0, 0, 0, Color.Gray).GetRelativeRectangleSizedRelativePositioned(
				x: 0.5, y: 0.5, width: 0.002, height: 1, centered: true)
		])
		{
			BackgroundColor = Color.Black
		};

		#endregion


		for (int i = 0; i < 4; i++)
		{
			Actions.Add(() =>
			{
				_startConfig[0].Position += new Vec2(-1, 0);
				ReloadStartConfigVisualiser();
				ReloadChaosVisualiser();
			});
		}
		
		Actions.Add(() =>
		{
			_startConfig = ThreeBodySimulator.GenerateStableConfiguration();
			_startConfig[0].Position += new Vec2(-137, -110);
			ReloadStartConfigVisualiser();
			ReloadChaosVisualiser();
		});
		
		for (int i = 0; i < 5; i++)
		{
			Actions.Add(() =>
			{
				_startConfig[0].Position += new Vec2(1, 0);
				ReloadStartConfigVisualiser();
				ReloadChaosVisualiser();
			});
		}
	}

	public override void OnLoad()
	{
		_startConfig = ThreeBodySimulator.GenerateStableConfiguration();
		
		ReloadStartConfigVisualiser();
		ReloadChaosVisualiser();
	}

	public void ReloadStartConfigVisualiser()
	{
		_startConfigVisualiser.Reset(_startConfig.ToArray());
		_startConfigVisualiser.Start();
	}

	public void ReloadChaosVisualiser()
	{
		if (_chaosVisualiser.Running)
		{
			return;
		}
		
		_chaosVisualiser.Reset(_startConfig.ToArray());
		_chaosVisualiser.Start();
	}

	public override void OnUnload()
	{
		_startConfigVisualiser.Stop();
		_chaosVisualiser.Stop();
	}

	public override void Dispose()
	{
		base.Dispose();
		_startConfigVisualiser.Dispose();
		_chaosVisualiser.Dispose();
	}
}