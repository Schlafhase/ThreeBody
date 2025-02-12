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
	private double _runTime = 100;
	private PhysicsBody[] _startConfig = ThreeBodySimulator.GenerateStableConfiguration(1);

	private SynchronizationContext? _syncContext;

	public SlideChaos()
	{
		_startConfigVisualiser = new ThreeBodyVisualiser(0);
		_chaosVisualiser = new ThreeBodyVisualiser(_runTime)
		{
			RunInstant = true
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

		Actions.Add(() =>
		{
			_startConfig[0].Position += new Vec2(20, 0);

			if (!_startConfigVisualiser.Running)
			{
				_syncContext?.Post(_ => ReloadStartConfigVisualiser(), null);
			}

			if (!_chaosVisualiser.Running)
			{
				_syncContext?.Post(_ => ReloadChaosVisualiser(), null);
			}
		});
		Actions.Add(() =>
		{
			_startConfig[0].Position += new Vec2(20, 0);

			if (!_startConfigVisualiser.Running)
			{
				_syncContext?.Post(_ => ReloadStartConfigVisualiser(), null);
			}

			if (!_chaosVisualiser.Running)
			{
				_syncContext?.Post(_ => ReloadChaosVisualiser(), null);
			}
		});
		Actions.Add(() =>
		{
			_startConfig[0].Position += new Vec2(20, 0);

			if (!_startConfigVisualiser.Running)
			{
				_syncContext?.Post(_ => ReloadStartConfigVisualiser(), null);
			}

			if (!_chaosVisualiser.Running)
			{
				_syncContext?.Post(_ => ReloadChaosVisualiser(), null);
			}
		});
		Actions.Add(() =>
		{
			_startConfig[0].Position += new Vec2(20, 20);

			if (!_startConfigVisualiser.Running)
			{
				_syncContext?.Post(_ => ReloadStartConfigVisualiser(), null);
			}

			if (!_chaosVisualiser.Running)
			{
				_syncContext?.Post(_ => ReloadChaosVisualiser(), null);
			}
		});
	}

	public override void OnLoad()
	{
		_syncContext = SynchronizationContext.Current;

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