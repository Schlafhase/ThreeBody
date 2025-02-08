using Canvas.Components.Interfaces.Mix;

namespace Presentation.Components;

public class Mandelbrot : PositionedRectangleSizedComponent
{
	private Bitmap _currentImage;
	private Thread _updateThread;
	private int _iterations = 1000;
	private double _xLeft = -2.5;
	private double _xRight = 1;
	private double _yTop = 1;
	private double _yBottom = -1;

	public int Iterations
	{
		get => _iterations;
		set
		{
			_iterations = value;

			if (!SuppressUpdate)
			{
				UpdateImageThreading();
			}
		}
	}

	public double XLeft
	{
		get => _xLeft;
		set
		{
			_xLeft = value;

			if (!SuppressUpdate)
			{
				UpdateImageThreading();
			}
		}

	}

	public double XRight
	{
		get => _xRight;
		set
		{
			_xRight = value;

			if (!SuppressUpdate)
			{
				UpdateImageThreading();
			}
		}

	}


	public double YTop
	{
		get => _yTop;
		set
		{
			_yTop = value;

			if (!SuppressUpdate)
			{
				UpdateImageThreading();
			}
		}

	}


	public double YBottom
	{
		get => _yBottom;
		set
		{
			_yBottom = value;

			if (!SuppressUpdate)
			{
				UpdateImageThreading();
			}
		}
	}
	
	/// <summary>
	///     Width of the component.
	/// </summary>
	public override int Width
	{
		get => _width;
		set
		{
			_width = value;

			if (SuppressUpdate)
			{
				return;
			}
	
			UpdateImageThreading();
			Parent?.Update();
		}
	}

	/// <summary>
	///     Height of the component.
	/// </summary>
	public override int Height
	{
		get => _height;
		set
		{
			_height = value;

			if (SuppressUpdate)
			{
				return;
			}

			UpdateImageThreading();
			Parent?.Update();
		}
	}

	public Mandelbrot(int iterations = 1000, double xLeft = -2.5, double xRight = 1, double yTop = 1, double yBottom = -1)
	{
		Iterations = iterations;
		XLeft = xLeft;
		XRight = xRight;
		YTop = yTop;
		YBottom = yBottom;
		
		_updateThread = new Thread(updateImage);
		UpdateImageThreading();
	}
	
	private void updateImage()
	{
		if (Width <= 0 || Height <= 0)
		{
			return;
		}
		
		_currentImage = Mandelbrot_fractal_2.Mandelbrot.CreateBitmap(Width, Height, Iterations, XLeft, XRight, YTop, YBottom);
		Parent?.Update();
	}
	
	public void UpdateImageThreading()
	{
		if (_updateThread is not null && _updateThread.IsAlive)
		{
			return;
		}
		
		_updateThread = new Thread(updateImage);
		_updateThread.Start();
	}
	
	public override void Put(Graphics g)
	{
		if (Width <= 0 || Height <= 0 || _currentImage is null)
		{
			return;
		}
		
		g.DrawImage(_currentImage, X, Y, Width, Height);
	}
}