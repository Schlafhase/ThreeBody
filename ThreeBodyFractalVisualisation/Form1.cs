using System.Numerics;
using ThreeBody;
using ThreeBodyFractal;

namespace ThreeBodyFractalVisualisation;

public partial class Form1 : Form
{
	private const float _zoom = 1f;
	private Vector2 _center = new(0, 0);
	private List<ThreeBodyForm> _openForms = [];

	public Form1()
	{
		InitializeComponent();
	}

	private void Form1_Load(object sender, EventArgs e)
	{
		pictureBox1.Image = Fractal.GetFractal(ThreeBodySimulator.GenerateStableConfiguration(), 800, 800, 20f,
											   center: _center, zoom: _zoom);
	}

	private void Form1_Click(object sender, MouseEventArgs e)
	{
		float fractalX = (e.X - 800 / 2) / _zoom + _center.X;
		float fractalY = (e.Y - 800 / 2) / _zoom + _center.Y;

		PhysicsBody[] bodies = ThreeBodySimulator.GenerateStableConfiguration();
		bodies[0].Position += new Vector2(fractalX, fractalY);
		bodies[1].Position += new Vector2(-fractalX, fractalY);

		ThreeBodyForm form = new ThreeBodyForm(bodies, 20f);
		form.Show();
		_openForms.Add(form);
	}

	private void Form1_FormClosing(object sender, FormClosingEventArgs e)
	{
		foreach (ThreeBodyForm form in _openForms.Where(form => !form.IsDisposed))
		{
			form.Close();
		}
	}
}