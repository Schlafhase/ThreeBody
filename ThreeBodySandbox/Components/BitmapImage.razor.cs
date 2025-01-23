using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Versioning;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using ThreeBodySandbox.Languages;

namespace ThreeBodySandbox.Components;

[SupportedOSPlatform("windows")]
public partial class BitmapImage : ComponentBase
{
	[Inject]
	private IJSRuntime _jsRuntime { get; set; }
	[Parameter]
	public Image? Image { get; set; }
	[Parameter] 
	public string Alt { get; set; } = "";
	[Parameter]
	public string Width { get; set; }
	[Parameter]
	public string Height { get; set; }
	[Parameter]
	public bool Hidden { get; set; }
	[Parameter]
	public Language Language { get; set; } = new German();
	
	private Guid _id = Guid.NewGuid();
	
	public ValueTask<ImageSize> Size => _jsRuntime.InvokeAsync<ImageSize>("size", _id);

	public struct ImageSize
	{
		public float X { get; init; }
		public float Y { get; init; }
		public float Width { get; init; }
		public float Height { get; set; }
	}
	
	private string _imageBase64 = "";

	public event Func<double, double, Task> OnClick;
	
	private async Task onClickHandler(MouseEventArgs e)
	{
		ImageSize size = await Size;
		await OnClick?.Invoke(e.ClientX - size.X, e.ClientY - size.Y);
	}
	
	protected override async Task OnParametersSetAsync()
	{
		await InvokeAsync(StateHasChanged);
		
		if (Image is null)
		{
			return;
		}
		
		using MemoryStream stream = new MemoryStream();
		Image.Save(stream, ImageFormat.Png);
		_imageBase64 = $"data:image/png;base64,{Convert.ToBase64String(stream.ToArray())}";
		await InvokeAsync(StateHasChanged);
	}

	// public async Task ShowLoadingText()
	// {
	// 	_imageBase64 = "";
	// 	await InvokeAsync(StateHasChanged);
	// }
}