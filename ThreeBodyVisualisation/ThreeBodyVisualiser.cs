using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.Versioning;
using Canvas.Components;
using Canvas.Components.Interfaces;
using ThreeBody;
using ThreeBody.Physics;

namespace ThreeBodyVisualisation;

[SupportedOSPlatform("windows")]
public sealed class ThreeBodyVisualiser : PositionedRectangleSizedComponent, IDisposable
{
	private PhysicsBody[] _bodies;
	private readonly Canvas.Canvas _canvas;
	private Thread _thread;
	private float _timeSinceStart;
	private float _runTime;
	
	public override int X { get => _canvas.X; set => _canvas.X = value; }
	public override int Y { get => _canvas.Y; set => _canvas.Y = value; }
	
	public override int Width { get => _canvas.Width; set => _canvas.Width = value; }
	public override int Height { get => _canvas.Height; set => _canvas.Height = value; }
	
	public override Canvas.Canvas Parent { get => _canvas.Parent; set => _canvas.Parent = value; }

	public override void Put(Graphics g)
	{
		_canvas.Put(g);
	}

	/// <summary>
	/// Shouldn't affect the speed of the simulation but rather the quality
	/// </summary>
	public float TimeStep { get; set; }

	/// <summary>
	/// Affects the speed of the simulation as well as the quality
	/// </summary>
	public float StepsPerSecond { get; set; }

	private bool _running;
	private SynchronizationContext _syncContext;
	
	private readonly BezierCurve[] _orbits;
	private readonly PositionedComponent[] _bodyComponents;

	public ThreeBodyVisualiser(float runTime = -1f,
		int x = 0,
		int y = 0,
		int width = 800,
		int height = 800,
		float timeStep = 0.01f,
		float stepsPerSecond = 10f)
	{
		_canvas = new Canvas.Canvas(0, 0, width, height);
		
		_runTime = runTime;
		X = x;
		Y = y;
		Width = width;
		Height = height;
		TimeStep = timeStep;
		StepsPerSecond = stepsPerSecond;
		
		_bodies = ThreeBodySimulator.GenerateStableConfiguration(1);

		_bodyComponents =
		[
			new GlowDot(0, 0, 10, 80, Color.Red),
			new GlowDot(0, 0, 10, 80, Color.Yellow),
			new GlowDot(0, 0, 10, 80, Color.Blue),
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

		_canvas.BackgroundColor = Color.Black;
	}
	
	public void Start()
	{
		if (_running)
		{
			throw new InvalidOperationException("The simulation is already running.");
		}
		
		_syncContext = SynchronizationContext.Current ??
			throw new NullReferenceException($"{SynchronizationContext.Current} was null");
		_running = true;
		
		_thread = new Thread(() =>
		{
			while (_running)
			{
				Gravity.SimulateGravity(_bodies, TimeStep);
				_timeSinceStart += TimeStep;

				if (_runTime >= 0 && _timeSinceStart >= _runTime)
				{
					break;
				}

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
						_orbits[i].Points.Add(new Point((int)(_bodies[i].Position.X), (int)(_bodies[i].Position.Y)));

						if (_orbits[i].Points.Count > 500)
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
							_ => _bodyComponents[_i].X = (int)(_bodies[_i].Position.X + _canvas.Width / 2),
							null);
						// ReSharper disable once PossibleLossOfFraction
						_syncContext.Post(
							_ => _bodyComponents[_i].Y = (int)(_bodies[_i].Position.Y + _canvas.Height / 2), null);
					}
				}
				catch (ObjectDisposedException)
				{
					break;
				}
				Parent?.Update();
				
				NOP(TimeStep / StepsPerSecond);
			}
		});
		
		_thread.Start();
	}
	
	public void Stop()
	{
		_running = false;
	}

	[SuppressMessage("ReSharper", "InconsistentlySynchronizedField")]
	public void SetConfig(PhysicsBody[] bodies)
	{
		if (_running)
		{
			throw new InvalidOperationException("The configuration must be set before the simulation is started.");
		}
		if (bodies.Length != 3)
		{
			throw new ArgumentException("Three bodies are required for this simulation.");
		}
		
		_bodies = bodies;
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