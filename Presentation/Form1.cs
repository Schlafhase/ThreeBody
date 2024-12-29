using Canvas.Components.Interfaces;
using SlidePresenter;
using Rectangle = Canvas.Components.Rectangle;

namespace Presentation;

public partial class Form1 : Form
{
	private readonly Presenter _presenter = new Presenter();

	private readonly List<List<CanvasComponent>> _slides = 
	[
		[
			new RelativePositionedComponent<Rectangle>(new Rectangle(0, 0, 100, 100, Color.Red))
			{
				X = 0.1f,
				Y = 0.1f,
				Margin = 10
			},
		],
		
		[
			new RelativePositionedComponent<Rectangle>(new Rectangle(0, 0, 100, 100, Color.Blue))
			{
				X = 0.5f,
				Y = 0.5f,
				Margin = 10,
				Centered = true
			},
		]
	];
	
	public Form1()
	{
		InitializeComponent();
	}

	private void Form1_Load(object sender, EventArgs e)
	{
		
	}
}