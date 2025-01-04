
using System.Numerics;
using ThreeBody;

namespace ThreeBodyVisualisationOl;

public partial class Form1 : Form
{
    // private Thread _thread;
    private Canvas.Canvas? _canvas;
    private Visualiser? _visualiser;
    
    
    public Form1()
    {
        InitializeComponent();
    }
    
    private void Form1_Load(object sender, EventArgs e)
    {
        _canvas = new Canvas.Canvas(800, 800);
        _visualiser = new(update, _canvas!);
        PhysicsBody[] bodies = ThreeBodySimulator.GenerateStableConfiguration();
        bodies[0].Position += new Vector2(20, 20);
        bodies[1].Position += new Vector2(-20, -20);
        bodies[0].Velocity += new Vector2(0, -1);
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