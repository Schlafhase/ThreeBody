using Microsoft.AspNetCore.Components;
using System.Drawing;
using System.Runtime.Versioning;
using System.Text;
using CSShaders.Shaders.Vectors;
using Newtonsoft.Json;
using ThreeBody;
using ThreeBodySandbox.Languages;

namespace ThreeBodySandbox.Components.Pages;

[SupportedOSPlatform("windows")]
public partial class Simulation : ComponentBase
{
	[Inject] private LanguageState _languageState { get; set; }
	[SupplyParameterFromQuery(Name = "x")] private double x { get; set; }
	[SupplyParameterFromQuery(Name = "y")] private double y { get; set; }

	[SupplyParameterFromQuery(Name = "time")]
	private double time { get; set; }

	[SupplyParameterFromQuery(Name = "deltaTime")]
	private double deltaTime { get; set; }

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
			_language = Language.GetLanguage(languageCode);
			_languageState.Current = _language;

			await InvokeAsync(StateHasChanged);

			PhysicsBody[] startConfig = JsonConvert.DeserializeObject<PhysicsBody[]>(
				Encoding.UTF8.GetString(
					Convert.FromBase64String(startConfigBase64)));

			startConfig[0].Position += new Vec2(x, y);

			_image = ThreeBodySimulator.GetSimulationImage(startConfig, 1000, 1000, time, deltaTime, true, true);
			_loaded = true;
			await InvokeAsync(StateHasChanged);
		}
	}
}