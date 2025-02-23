﻿using System.Drawing;
using System.Runtime.Versioning;
using CSShaders.Shaders.Vectors;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using ThreeBody;
using ThreeBodyFractal;
using ThreeBodySandbox.Languages;

namespace ThreeBodySandbox.Components.Pages;

[SupportedOSPlatform("windows")]
public partial class Sandbox : ComponentBase
{
	[Inject]
	private LanguageState _languageState { get; set; }
	[SupplyParameterFromQuery(Name = "lang")]
	private string _languageCode { get; set; }

	private Language _language = new German();

	[Inject] private IJSRuntime _jsRuntime { get; set; }

	private Bitmap? _bitmapImage;
	private BitmapImage _imageComponent;
	private bool _imageHidden = false;

	private int _width = 800;
	private int _height = 800;
	private double _time = 20f;
	private double _timeStep = 0.1f;
	private double _zoom = 1f;
	private double _centerX;
	private double _centerY;
	private readonly PhysicsBody[] _bodies = ThreeBodySimulator.GenerateStableConfiguration();

	private (double x, double y)[] _positions = new (double x, double y)[3];
	private (double x, double y)[] _velocities = new (double x, double y)[3];

	private Thread _currentThread;

	public Sandbox()
	{
		_centerY = 0;
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
		_bodies[0].Position = new Vec2(_positions[0].x, _positions[0].y);
		_bodies[1].Position = new Vec2(_positions[1].x, _positions[1].y);
		_bodies[2].Position = new Vec2(_positions[2].x, _positions[2].y);

		_bodies[0].Velocity = new Vec2(_velocities[0].x, _velocities[0].y);
		_bodies[1].Velocity = new Vec2(_velocities[1].x, _velocities[1].y);
		_bodies[2].Velocity = new Vec2(_velocities[2].x, _velocities[2].y);
	}

	private async Task render()
	{
		_imageHidden = true;
		await InvokeAsync(StateHasChanged);

		_currentThread = new Thread(async () =>
		{
			_bitmapImage = Fractal.GetFractal(FractalType.Distance, _bodies,
											  _width, _height, _time, _timeStep, new Vec2(_centerX, _centerY), _zoom);

			// _bitmapImage = Fractal.GetFractalIterations(_bodies, _width, _height, 100, 101, _timeStep, new Vec2(0, 0), _zoom);

			_imageHidden = false;
			await InvokeAsync(StateHasChanged);
		});

		_currentThread.Start();
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			_language = Language.GetLanguage(_languageCode);
			_languageState.Current = _language;
			
			_imageComponent.OnClick += async (x, y) =>
			{
				BitmapImage.ImageSize imageSize = await _imageComponent.Size;
				int imageX = (int)(x / imageSize.Width * _width);
				int imageY = (int)(y / imageSize.Height * _height);

				double fractalX = (imageX - _width / 2f) / _zoom + _centerX;
				double fractalY = (imageY - _height / 2f) / _zoom + _centerY;

				string configBase64 =
					Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_bodies)));
				
				await _jsRuntime.InvokeVoidAsync(
					"open",
					$"simulation?x={fractalX}&y={fractalY}&time={_time}&deltaTime={_timeStep}&lang={_languageState.Current.LanguageCode}&startConfig={configBase64}", "_blank");
			};

			await render();
		}
	}
}