using Microsoft.AspNetCore.Components;
using System.Drawing;
using System.Numerics;
using System.Text;
using Newtonsoft.Json;
using ThreeBody;

namespace ThreeBodySandbox.Components.Pages;

public partial class Simulation : ComponentBase
{
	[SupplyParameterFromQuery(Name = "x")] private float x { get; set; }
	[SupplyParameterFromQuery(Name = "y")] private float y { get; set; }

	[SupplyParameterFromQuery(Name = "time")]
	private float time { get; set; }

	[SupplyParameterFromQuery(Name = "timeStep")]
	private float timeStep { get; set; }

	[SupplyParameterFromQuery(Name = "startConfig")]
	private string startConfigBase64 { get; set; }

	
	private Bitmap? _image;
	private bool _loaded;

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			PhysicsBody[] startConfig = JsonConvert.DeserializeObject<PhysicsBody[]>(
				Encoding.UTF8.GetString(
					Convert.FromBase64String(startConfigBase64)));

			startConfig = ThreeBodySimulator.GenerateStableConfiguration();
			startConfig[0].Position += new Vector2(x, y);

			_image = ThreeBodySimulator.GetSimulationImage(startConfig, 1000, 1000, time, timeStep, true);
			_loaded = true;
			await InvokeAsync(StateHasChanged);
		}
	}
}