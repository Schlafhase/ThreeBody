using System.Drawing;
using System.Numerics;
using Microsoft.AspNetCore.Components;
using ThreeBody;
using ThreeBodyFractal;

namespace ThreeBodySandbox.Components.Pages;

public partial class Sandbox : ComponentBase
{
	private Bitmap? _bitmapImage;
	private BitmapImage _imageComponent;
	private bool _imageHidden = false;
	
	private int _width = 800;
	private int _height = 800;
	private float _time = 20f;
	private float _timeStep = 0.1f;
	private PhysicsBody[] _bodies = ThreeBodySimulator.GenerateStableConfiguration();
	
	private (float x, float y)[] _positions = new (float x, float y)[3];
	private (float x, float y)[] _velocities = new (float x, float y)[3];
	
	private Thread _currentThread;

	public Sandbox()
	{
		_positions[0].x = _bodies[0].Position.X;
		_positions[0].y = _bodies[0].Position.Y;
		
		_positions[1].x = _bodies[1].Position.X;
		_positions[1].y = _bodies[1].Position.Y;
		
		_positions[2].x = _bodies[2].Position.X;
		_positions[2].y = _bodies[2].Position.Y;
		
		
		_velocities[0].x = _bodies[0].Velocity.X;
		_velocities[0].y = _bodies[0].Velocity.Y;
		
		_velocities[1].x = _bodies[1].Velocity.X;
		_velocities[1].y = _bodies[1].Velocity.Y;
		
		_velocities[2].x = _bodies[2].Velocity.X;
		_velocities[2].y = _bodies[2].Velocity.Y;
	}
	
	private void updateBodies()
	{
		_bodies[0].Position = new Vector2(_positions[0].x, _positions[0].y);
		_bodies[1].Position = new Vector2(_positions[1].x, _positions[1].y);
		_bodies[2].Position = new Vector2(_positions[2].x, _positions[2].y);
		
		_bodies[0].Velocity = new Vector2(_velocities[0].x, _velocities[0].y);
		_bodies[1].Velocity = new Vector2(_velocities[1].x, _velocities[1].y);
		_bodies[2].Velocity = new Vector2(_velocities[2].x, _velocities[2].y);
	}

	private async Task render()
	{
		_imageHidden = true;
		await InvokeAsync(StateHasChanged);

		_currentThread = new Thread(async () =>
		{
			_bitmapImage = Fractal.GetFractal(FractalType.Distance, _bodies,
											  _width, _height, _time, _timeStep, new Vector2(0, 0), 1f);
			_imageHidden = false;
			await InvokeAsync(StateHasChanged);
		});
		
		_currentThread.Start();
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			await render();
		}
	}
}