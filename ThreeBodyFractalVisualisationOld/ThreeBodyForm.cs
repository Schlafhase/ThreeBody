using System.Numerics;
using ThreeBody;
using ThreeBodyVisualisation;

namespace ThreeBodyFractalVisualisation;

public partial class ThreeBodyForm : Form
{
	private Canvas.Canvas? _canvas;
	private Visualiser? _visualiser;
	private PhysicsBody[] _bodies;
	private float _runTime;


	public ThreeBodyForm(PhysicsBody[]? bodies = null, float runTime = -1f)
	{
		_runTime = runTime;
		_bodies = bodies ?? ThreeBodySimulator.GenerateStableConfiguration();;
		InitializeComponent();
	}

	private void Form1_Load(object sender, EventArgs e)
	{
		_canvas = new Canvas.Canvas(800, 800);
		_visualiser = new Visualiser(update, _canvas!, _runTime);
		PhysicsBody[] bodies = _bodies;
		_visualiser.SetConfig(bodies);

		pictureBox1.Image = new Bitmap(800, 800);
		Form1_Resize(sender, e);
		_visualiser.Start();
	}

	private void Form1_Resize(object sender, EventArgs e)
	{
		Size size = new(ClientSize.Width, ClientSize.Height);

		if (size.Height == 0)
		{
			return;
		}

		pictureBox1.Size = size;
		pictureBox1.Image = new Bitmap(pictureBox1.Image, size);
		_canvas!.Width = ClientSize.Width;
		_canvas!.Height = ClientSize.Height;
	}

	private void Form1_FormClosing(object sender, FormClosingEventArgs e)
	{
		_canvas?.Dispose();
		_visualiser?.Dispose();
	}

	private void update()
	{
		if (IsDisposed)
		{
			return;
		}

		Image img = pictureBox1.Image;
		using Graphics g = Graphics.FromImage(img);
		_canvas?.Put(g);
		pictureBox1.Invoke((MethodInvoker)(() => pictureBox1.Image = img));
	}
}