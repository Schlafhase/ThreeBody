﻿using Canvas.Components.Interfaces.Relative;
using Presentation.Components;
using SlidePresenter;

namespace Presentation.Slides;

public class SlideMandelbrotZoom : Slide
{
	private readonly Mandelbrot _mandelbrot;

	private readonly RelativeRectangleSizedRelativePositionedComponent<Mandelbrot> _mandelbrotComponent;

	public SlideMandelbrotZoom()
	{
		_mandelbrot = new Mandelbrot
		{
			Width = 1920,
			Height = 1080,
			Iterations = 10_000,
			MandelBrotWidth = 1.6059746094705611E-11d,
			MandelBrotCenterX = -0.74326380829271044d,
			MandelBrotCenterY = 0.18079212896068195d
		};

		_mandelbrotComponent = new RelativeRectangleSizedRelativePositionedComponent<Mandelbrot>(_mandelbrot)
		{
			X = 0,
			Y = 0,
			Width = 1,
			Height = 1
		};

		Canvas = new Canvas.Canvas(0, 0,
		[
			_mandelbrotComponent
		]);
		_mandelbrot.LoadPlaceholder();
	}

	public override void OnLoad()
	{
		_mandelbrotComponent.UpdateBoundaries();
		_mandelbrot.UpdateImageThreading();
	}
}