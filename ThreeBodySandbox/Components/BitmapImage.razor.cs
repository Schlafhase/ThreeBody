using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Mime;
using Microsoft.AspNetCore.Components;

namespace ThreeBodySandbox.Components;

public partial class BitmapImage : ComponentBase
{
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
	
	private string _imageBase64 = "";
	
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