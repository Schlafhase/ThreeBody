using Canvas.Components.Interfaces.Mix;

namespace Presentation.Components;

public class Mandelbrot : PositionedRectangleSizedComponent
{
	private Bitmap _currentImage;
	private Thread? _updateThread;
	private int _iterations = 1000;
	private double _xLeft = -2.5;
	private double _xRight = 1;
	private double _yTop = 1;
	private double _yBottom = -1;

	public bool KeepAspectRatio = true;
	public double MandelBrotWidth = 3.5;
	public double MandelBrotCenterX = -0.75;
	public double MandelBrotCenterY = 0;

	public double Quality = 1;

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
		}
	}

	public Mandelbrot(int iterations = 1000, double xLeft = -2.5, double xRight = 1, double yTop = 1, double yBottom = -1)
	{
		Iterations = iterations;
		XLeft = xLeft;
		XRight = xRight;
		YTop = yTop;
		YBottom = yBottom;
	}

	public void LoadPlaceholder(int width = 1920, int height = 1080)
	{
		adjustScreenSection(width, height);
		_currentImage = Mandelbrot_fractal_2.Mandelbrot.CreateBitmap(width, height, 40, XLeft, XRight, YTop, YBottom); // Prerender minimal placeholder
	}
	
	private void updateImage()
	{
		if (Width <= 0 || Height <= 0)
		{
			return;
		}

		int width = Width;
		int height = Height;

		adjustScreenSection();
		Bitmap bitmap = Mandelbrot_fractal_2.Mandelbrot.CreateBitmap((int)(width * Quality), (int)(height * Quality), Iterations, XLeft, XRight, YTop, YBottom);
		_currentImage?.Dispose();
		_currentImage = new Bitmap(bitmap, width, height);
		bitmap.Dispose();
		
		Parent?.Update();
	}

	private void adjustScreenSection(int? customWidth = null, int? customHeight = null)
	{
		if (!KeepAspectRatio)
		{
			return;
		}

		double aspectRatioInverse = (double)(customHeight ?? Height) / (customWidth ?? Width);
			
		_xLeft = MandelBrotCenterX - MandelBrotWidth / 2;
		_xRight = MandelBrotCenterX + MandelBrotWidth / 2;
		_yTop = MandelBrotCenterY + MandelBrotWidth / 2 * aspectRatioInverse;
		_yBottom = MandelBrotCenterY - MandelBrotWidth / 2 * aspectRatioInverse;
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
		if (_currentImage.Width <= 0 || _currentImage.Height <= 0 || _currentImage is null)
		{
			return;
		}
		
		g.DrawImage(_currentImage, X, Y, Width, Height);
	}
}