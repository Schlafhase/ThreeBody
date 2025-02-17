using System.Runtime.Intrinsics.X86;
using Canvas.Components;
using Canvas.Components.Interfaces.Mix;
using SkiaSharp;
using SlidePresenter;

namespace Presentation.Slides;

public sealed class SlideMandelbrotFunction : Slide
{
	public SlideMandelbrotFunction()
	{
		#region Slide Content

		Canvas = new Canvas.Canvas(0, 0,
		[
			new Equation(@"\vec{s_{rot_{t+ \Delta t}}} = \vec{s_{rot_t}} + \vec{v_{rot_{t+ \Delta t}}} \\ \vec{v_{rot_{t+ \Delta t}}} = \vec{v_{rot_t}} + \vec{a_{rot_{t + \Delta t}}}", 0) { Color = SKColors.White }
				.GetRelativeSizedRelativePositioned(x: 0.3, y: 0.5, size: 0.4),
		])
		{
			BackgroundColor = Color.Black
		};

		#endregion
	}
}