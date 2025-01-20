using Microsoft.AspNetCore.Components;
using System.Drawing;
using System.Numerics;
using System.Text;
using Newtonsoft.Json;
using ThreeBody;
using ThreeBodySandbox.Languages;

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
	[SupplyParameterFromQuery(Name = "lang")]
	private string languageCode { get; set; }

	private Language _language = new German();
	
	private Bitmap? _image;
	private bool _loaded;

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			_language = languageCode switch
			{
				"en" => new English(),
				_    => new German()
			};
			
			await InvokeAsync(StateHasChanged);
			
			PhysicsBody[] startConfig = JsonConvert.DeserializeObject<PhysicsBody[]>(
				Encoding.UTF8.GetString(
					Convert.FromBase64String(startConfigBase64)));

			startConfig[0].Position += new Vector2(x, y);

			_image = ThreeBodySimulator.GetSimulationImage(startConfig, 1000, 1000, time, timeStep, true, true);
			_loaded = true;
			await InvokeAsync(StateHasChanged);
		}
	}
}