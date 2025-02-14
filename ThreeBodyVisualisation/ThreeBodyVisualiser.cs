using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.Versioning;
using Canvas.Components;
using Canvas.Components.AnimationUtilities;
using Canvas.Components.Interfaces.Mix;
using Canvas.Components.Interfaces.Positioned;
using CSShaders.Shaders.Vectors;
using ThreeBody;
using ThreeBody.Physics;

namespace ThreeBodyVisualisation;

[SupportedOSPlatform("windows")]
public sealed class ThreeBodyVisualiser : PositionedRectangleSizedComponent, IDisposable
{
	private PhysicsBody[] _bodies;
	private readonly Canvas.Canvas _canvas;
	private Thread _thread;
	private double _timeSinceStart;
	public double RunTime;

	private readonly object _tickLocker = new();

	public override int X
	{
		get => _canvas.X;
		set => _canvas.X = value;
	}

	public override int Y
	{
		get => _canvas.Y;
		set => _canvas.Y = value;
	}

	public override int Width
	{
		get => _canvas.Width;
		set => _canvas.Width = value;
	}

	public override int Height
	{
		get => _canvas.Height;
		set => _canvas.Height = value;
	}

	public override Canvas.Canvas Parent
	{
		get => _canvas.Parent;
		set
		{
			// Debug.WriteLine($"Setting parent canvas: {value != null}");
			_canvas.Parent = value;
		}
	}

	public override void Put(Graphics g)
	{
		// Debug.WriteLine($"Put called, canvas exists: {_canvas != null}, width: {_canvas.Width}, height: {_canvas.Height}");
		_canvas.Put(g);
	}

	/// <summary>
	/// Shouldn't affect the speed of the simulation but rather the quality
	/// </summary>
	public double TimeStep { get; set; }

	/// <summary>
	/// Affects the speed of the simulation as well as the quality
	/// </summary>
	public double StepsPerSecond { get; set; }

	public bool RunInstant { get; set; }
	public int OrbitLength { get; set; } = 500;

	public double SimulationWidth = 800;
	public double TransformationRatio => Width / SimulationWidth;

	public bool Running => _running;

	private bool _running;
	private SynchronizationContext _syncContext;

	private readonly BezierCurve[] _orbits;
	private PositionedComponent[] _bodyComponents;

	public ThreeBodyVisualiser(double runTime = -1f,
		int x = 0,
		int y = 0,
		int width = 800,
		int height = 800,
		double timeStep = 0.01f,
		double stepsPerSecond = 10f)
	{
		_canvas = new Canvas.Canvas(0, 0, width, height);

		RunTime = runTime;
		X = x;
		Y = y;
		Width = width;
		Height = height;
		TimeStep = timeStep;
		StepsPerSecond = stepsPerSecond;

		_bodies = ThreeBodySimulator.GenerateStableConfiguration(1);

		_bodyComponents =
		[
			new GlowDot(0, 0, (int)(10 * TransformationRatio), (int)(80 * TransformationRatio), Color.Red),
			new GlowDot(0, 0, (int)(10 * TransformationRatio), (int)(80 * TransformationRatio), Color.Yellow),
			new GlowDot(0, 0, (int)(10 * TransformationRatio), (int)(80 * TransformationRatio), Color.Blue),
		];

		_orbits =
		[
			new BezierCurve([]),
			new BezierCurve([]),
			new BezierCurve([])
		];

		Array.ForEach(_orbits, orbit => orbit.Pen = Pens.White);

		Array.ForEach(_orbits, _canvas.AddChild);
		Array.ForEach(_bodyComponents, _canvas.AddChild);
	}

	public void Start()
	{
		if (_running)
		{
			return;
		}
		
		ResizeComponents();

		_syncContext = SynchronizationContext.Current ??
			throw new NullReferenceException($"{SynchronizationContext.Current} was null");
		_running = true;

		_thread = new Thread(() =>
		{
			// Debug.WriteLine("Thread started");

			while (_running)
			{
				lock (_tickLocker)
				{
					// Debug.WriteLine($"Body 0 position: {_bodies[0].Position}");

					Array.ForEach(_orbits, orbit =>
					{
						orbit.X = _canvas.Width / 2;
						orbit.Y = _canvas.Height / 2;
					});

					for (int i = 0; i < _bodies.Length; i++)
					{
						_bodies[i].Position += _bodies[i].Velocity * TimeStep;

						lock (_orbits[i].PointsLocker)
						{
							_orbits[i].Points
									  .Add(new Point((int)(_bodies[i].Position.X), (int)(_bodies[i].Position.Y)));

							if (_orbits[i].Points.Count > OrbitLength)
							{
								_orbits[i].Points.RemoveAt(0);
							}
						}
					}

					try
					{
						for (int i = 0; i < _bodies.Length; i++)
						{
							// ReSharper disable once InconsistentNaming
							int _i = i; // Copy i because i might have changed by the time the lambda is executed

							// ReSharper disable once PossibleLossOfFraction
							_syncContext.Post(
								_ => _bodyComponents[_i].X =
									(int)(_bodies[_i].Position.X * TransformationRatio + _canvas.Width / 2),
								null);
							// ReSharper disable once PossibleLossOfFraction
							_syncContext.Post(
								_ => _bodyComponents[_i].Y =
									(int)(_bodies[_i].Position.Y * TransformationRatio + _canvas.Height / 2),
								null);
						}
					}
					catch (ObjectDisposedException)
					{
						break;
					}

					Parent?.Update();
					// Debug.WriteLine($"Parent update called, parent exists: {Parent != null}");

					Gravity.SimulateGravity(_bodies, TimeStep);
					_timeSinceStart += TimeStep;

					if (RunTime >= 0 && _timeSinceStart >= RunTime)
					{
						_running = false;
						break;
					}

					// Debug.WriteLine($"Update complete, time: {_timeSinceStart}");

					if (RunInstant)
					{
						continue;
					}

					NOP(TimeStep / StepsPerSecond);
				}
			}

			// Debug.WriteLine("Thread stopped");
			_syncContext.Send(_ => Parent?.ForceUpdate(), null);
		})
		{
			IsBackground = true
		};

		_thread.Start();
	}

	public void Stop()
	{
		_running = false;
	}

	public void Reset(PhysicsBody[] config)
	{
		ResetTimeSinceStart();
		SetConfig(config);

		// _bodyComponents =
		// [
		// 	new GlowDot(0, 0, (int)(10 * TransformationRatio), (int)(80 * TransformationRatio), Color.Red),
		// 	new GlowDot(0, 0, (int)(10 * TransformationRatio), (int)(80 * TransformationRatio), Color.Yellow),
		// 	new GlowDot(0, 0, (int)(10 * TransformationRatio), (int)(80 * TransformationRatio), Color.Blue),
		// ];

		lock (_tickLocker)
		{
			Array.ForEach(_orbits, orbit => orbit.Points.Clear());
		}
	}

	public void ResizeComponents()
	{
		lock (_tickLocker)
		{
			Array.ForEach(_orbits, orbit => { orbit.Scale = TransformationRatio; });
			
			foreach (GlowDot bodyComponent in _bodyComponents.OfType<GlowDot>())
			{
				bodyComponent.SuppressUpdate = true;
				bodyComponent.SetRadius((int)(10 * TransformationRatio), (int)(80 * TransformationRatio));
				bodyComponent.SuppressUpdate = false;
			}
			
			Parent?.Update();
		}
	}

	public void SetConfig(PhysicsBody[] bodies)
	{
		lock (_tickLocker)
		{
			if (bodies.Length != 3)
			{
				throw new ArgumentException("Three bodies are required for this simulation.");
			}

			_bodies = bodies;
		}
	}

	// public void InterpolatePosition(Vec2 offset, double duration)
	// {
	// 	_bodyComponents[0].X.InterpolateThreading(x =>
	// 	{
	// 		_bodyComponents[0].X = (int)x;
	// 		_bodies[0].Position += new Vec2(x, 0);
	// 	}, _bodyComponents[0].X + offset.X, duration);
	// 	
	// 	_bodyComponents[0].Y.InterpolateThreading(y =>
	// 	{
	// 		_bodyComponents[0].Y = (int)y;
	// 		_bodies[0].Position += new Vec2(0, y);
	// 	}, _bodyComponents[0].Y + offset.Y, duration);
	// }

	public void ResetTimeSinceStart()
	{
		_timeSinceStart = 0;
	}

	public void Dispose()
	{
		_running = false;
		_canvas.Dispose();
	}

	[SuppressMessage("ReSharper", "InconsistentNaming")]
	private static void NOP(double durationSeconds)
	{
		double durationTicks = Math.Round(durationSeconds * Stopwatch.Frequency);
		Stopwatch sw = Stopwatch.StartNew();

		while (sw.ElapsedTicks < durationTicks) { }
	}
}