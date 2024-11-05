
using System.Numerics;
using ThreeBody;

namespace ThreeBodyVisualisation;

public partial class Form1 : Form
{
    // private Thread _thread;
    private Canvas.Canvas _canvas;
    
    
    public Form1()
    {
        InitializeComponent();
    }
    
    private void Form1_Load(object sender, EventArgs e)
    {
        _canvas = new Canvas.Canvas(800, 800);
        Visualiser visualiser = new(update, _canvas);
        PhysicsBody[] bodies = ThreeBodySimulator.GenerateStableConfiguration(0);
        // bodies[0].Position += new Vector2(100, 100);
        // bodies[1].Position += new Vector2(-100, -100);
        visualiser.SetConfig(bodies);
        
        pictureBox1.Image = new Bitmap(800, 800);
        Form1_Resize(sender, e);
        visualiser.Start();
    }
    
    private void Form1_Resize(object sender, EventArgs e)
    {
        Size size = new(ClientSize.Width, ClientSize.Height);
        if (size.Height == 0) return;
        pictureBox1.Size = size;
        pictureBox1.Image = new Bitmap(pictureBox1.Image, size);
        _canvas.Width = ClientSize.Width;
        _canvas.Height = ClientSize.Height;
    }

    private void update()
    {
        if (IsDisposed)
        {
            return;
        }
        Image img = pictureBox1.Image;
        using Graphics g = Graphics.FromImage(img);
        _canvas.Put(g);
        pictureBox1.Invoke((MethodInvoker)(() => pictureBox1.Image = img));
    }
}