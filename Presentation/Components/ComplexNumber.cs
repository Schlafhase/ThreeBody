namespace Mandelbrot_fractal_2
{
	public struct ComplexNumber
	{

		public double X { get; set; }
		public double Y { get; set; }

		public ComplexNumber(double x, double y)
		{
			this.X = x;
			this.Y = y;
		}
		public override string ToString()
		{
			return $"{this.X} + {this.Y}i";
		}
	}
}