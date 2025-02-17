using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.Versioning;
using AnimatedGif;
using Canvas.Components;
using Canvas.Components.AnimationUtilities;
using Canvas.Components.Interfaces.Mix;
using Canvas.Components.Interfaces.Positioned;
using CSShaders.Shaders.Vectors;
using ThreeBody;
using ThreeBody.Physics;

namespace ThreeBodyVisualisation;

[SupportedOSPlatform("windows")]
public sealed class ThreeBodyRenderer : IDisposable
{
	private PhysicsBody[] _bodies;
	private readonly Canvas.Canvas _canvas;
	private double _timeSinceStart;
	public double RunTime;

	private readonly object _tickLocker = new();


	public int Width
	{
		get => _canvas.Width;
		set => _canvas.Width = value;
	}

	public int Height
	{
		get => _canvas.Height;
		set => _canvas.Height = value;
	}

	public void Put(Graphics g)
	{
		// Debug.WriteLine($"Put called, canvas exists: {_canvas != null}, width: {_canvas.Width}, height: {_canvas.Height}");
		_canvas.Put(g);
	}

	/// <summary>
	/// Shouldn't affect the speed of the simulation but rather the quality
	/// </summary>
	public double DeltaTime { get; set; }

	/// <summary>
	/// Affects the speed of the simulation as well as the quality
	/// </summary>
	public double StepsPerSecond { get; set; }

	public int OrbitLength { get; set; } = 500;

	public double SimulationWidth = 800;
	public double TransformationRatio => Width / SimulationWidth;

	public bool Running { get; private set; }

	private readonly BezierCurve[] _orbits;
	private PositionedComponent[] _bodyComponents;


	private double _timeSinceLastFrame = double.MaxValue;
	public double FPS { get; set; } = 30;
	public int Repeat { get; set; } = 0;
	private int _frameCounter;

	private void update(AnimatedGifCreator gif)
	{
		if (_timeSinceLastFrame < 1 / FPS)
		{
			return;
		}


		using Image frame = new Bitmap(Width, Height);
		using Graphics g = Graphics.FromImage(frame);
		
		Put(g);
		gif.AddFrame(frame, quality: GifQuality.Bit8);
		// frame.Save("frames/" + _frameCounter + ".png");
		_timeSinceLastFrame = 0;
		_frameCounter++;
	}

	public ThreeBodyRenderer(double runTime = -1f,
		int width = 800,
		int height = 800,
		double deltaTime = 0.01f,
		double stepsPerSecond = 10f)
	{
		_canvas = new Canvas.Canvas(0, 0, width, height)
		{
			BackgroundColor = Color.Black
		};

		RunTime = runTime;
		Width = width;
		Height = height;
		DeltaTime = deltaTime;
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
		if (Running)
		{
			return;
		}

		ResizeComponents();

		Running = true;
		_frameCounter = 0;

		using AnimatedGifCreator? gif = AnimatedGif.AnimatedGif.Create("output.gif", 1, Repeat);

		while (Running)
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
					_bodies[i].Position += _bodies[i].Velocity * DeltaTime;

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
						// ReSharper disable once PossibleLossOfFraction
						_bodyComponents[i].X =
							(int)(_bodies[i].Position.X * TransformationRatio + _canvas.Width / 2);
						// ReSharper disable once PossibleLossOfFraction
						_bodyComponents[i].Y =
							(int)(_bodies[i].Position.Y * TransformationRatio + _canvas.Height / 2);
					}
				}
				catch (ObjectDisposedException)
				{
					break;
				}

				update(gif);
				// Debug.WriteLine($"Parent update called, parent exists: {Parent != null}");

				Gravity.SimulateGravity(_bodies, DeltaTime);
				_timeSinceStart += DeltaTime;
				_timeSinceLastFrame += DeltaTime;

				if (RunTime >= 0 && _timeSinceStart >= RunTime)
				{
					Running = false;
					break;
				}

				// Debug.WriteLine($"Update complete, time: {_timeSinceStart}");
			}
		}

		// Debug.WriteLine("Thread stopped");
	}

	public void Stop()
	{
		Running = false;
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
		Running = false;
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