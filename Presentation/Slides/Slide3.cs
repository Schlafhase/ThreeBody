using Canvas.Components.Interfaces.Relative;
using SlidePresenter;
using ThreeBodyVisualisation;

namespace Presentation.Slides;

public sealed class Slide3 : Slide
{
	private readonly ThreeBodyVisualiser _threeBodyVisualiser;
	
	public Slide3(ThreeBodyVisualiser threeBodyVisualiser, RelativePositionedComponent<ThreeBodyVisualiser> threeBodyVisualiserComponent)
	{
		_threeBodyVisualiser = threeBodyVisualiser;
		Canvas = new Canvas.Canvas(0, 0, [threeBodyVisualiserComponent]);
	}
	
	public override void OnLoad()
	{
		_threeBodyVisualiser.Start();
	}
	
	public override void OnUnload()
	{
		_threeBodyVisualiser.Stop();
	}
	
	public override void Dispose()
	{
		_threeBodyVisualiser.Dispose();
		base.Dispose();
	}
}