using System.Numerics;
using ThreeBody;
using ThreeBodyFractal;

namespace ThreeBodyFractalVisualisation;

public partial class Form1 : Form
{
    private float _zoom = 1f;
    private Vector2 _center = new(0, 0);
    
    public Form1()
    {
        InitializeComponent();
    }
    
    private void Form1_Load(object sender, EventArgs e)
    {
        pictureBox1.Image = Fractal.GetFractal(ThreeBodySimulator.GenerateStableConfiguration(), 800, 800, 20f, center: _center, zoom: _zoom);
    }
    
    private void Form1_Click(object sender, MouseEventArgs e)
    {
        Console.WriteLine($"{e.X}, {e.Y}");
        _center += new Vector2((e.X - 400)/_zoom + _center.X, (e.Y - 400)/_zoom + _center.Y/2);
        pictureBox1.Image = Fractal.GetFractal(ThreeBodySimulator.GenerateStableConfiguration(), 800, 800, 1f, center: _center, zoom: _zoom);
    }
}