using System.Drawing;
using System.Runtime.Versioning;
using Canvas.Components.Interfaces.Mix;
using CSShaders.Shaders.Vectors;
using ThreeBody;
using ThreeBodyFractal;

namespace ThreeBodyFractalVisualisation;

[SupportedOSPlatform("windows")]
public class ThreeBodyFractalVisualiser : PositionedRectangleSizedComponent
{
	public FractalType Type { get; set; } = FractalType.Distance;
	public double SimulationTime { get; set; } = 20f;
	public double DeltaTime { get; set; } = 0.1f;
	public Vec2 Center { get; set; } = new Vec2(0, 0);
	public double Zoom { get; set; } = 1f;

	public PhysicsBody[] StartConfig
	{
		get => _startConfig;
		init
		{
			if (value.Length != 3)
			{
				throw new ArgumentException("Start config must have 3 bodies.");
			}
			_startConfig = value;
		}
	}

	private Bitmap _currentImage;
	private readonly Thread _updateImageThread;
	
	private readonly PhysicsBody[] _startConfig;

	public ThreeBodyFractalVisualiser(int width = 0, int height = 0, int x = 0, int y = 0)
	{
		X = x;
		Y = y;
		Width = width;
		Height = height;
		
		if (Width > 0 && Height > 0)
		{
			_currentImage = new Bitmap(Width, Height);
		}
		else {
			_currentImage = new Bitmap(1, 1);
		}
		
		_updateImageThread = new Thread(UpdateImage);
		_startConfig = ThreeBodySimulator.GenerateStableConfiguration();
		UpdateImageThreading();
	}

	public void UpdateImage()
	{
		if (Width <= 0 || Height <= 0)
		{
			return;
		}
		
		Bitmap oldBitmap = _currentImage;
		
		_currentImage = Fractal.GetFractal(Type, StartConfig, Width, Height, SimulationTime, DeltaTime, Center, Zoom);
		oldBitmap.Dispose();
		Parent?.Update();
	}

	public void UpdateImageThreading()
	{
		if (_updateImageThread.IsAlive)
		{
			return;
		}

		_updateImageThread.Start();
	}

	public override void Put(Graphics g)
	{
		if (Width <= 0 || Height <= 0)
		{
			return;
		}
		g.DrawImage(_currentImage, X, Y, Width, Height);
	}
}