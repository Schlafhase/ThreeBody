namespace ThreeBodySandbox.Languages;

/// <summary>
/// English (UK Edition)
/// </summary>
public class English : Language
{
	public override string LanguageCode => "en";
	
	public override string BitmapImageLoading => "Loading";

	public override string SandBoxGeneral => "General";
	public override string SandBoxWidth => "Width";
	public override string SandBoxHeight => "Height";
	public override string SandBoxTime => "Time";
	public override string SandBoxTimeStep => "Timestep";
	public override string SandBoxZoom => "Zoom";
	public override string SandBoxCenterX => "Centre X";
	public override string SandBoxCenterY => "Centre Y";
	public override string SandBoxRender => "Render";

	public override string SandBoxConfig => "Configuration";
	public override string SandBoxConfigBody => "Body";
	public override string SandBoxConfigMass => "Mass";
	public override string SandBoxConfigPositionX => "X";
	public override string SandBoxConfigPositionY => "Y";
	public override string SandBoxConfigVelocityX => "X Velocity";
	public override string SandBoxConfigVelocityY => "Y Velocity";
	
	public override string ThreeBodyFractal => "Three-Body Fractal";

	public override string ThreeBodyFractalDescription =>
		"<p>Every Pixel of the image is assigned a configuration of three bodies (each with position, velocity and mass). "
		+ "This configuration is calculated as follows: The configuration at pixel 0, 0 is a stable configuration "
		+ "where all bodies are placed at regular intervals on a circle with a radius of 100. The velocities "
		+ "are vectors with a magnitude of 10 pi, pointing tangentially away from the circle. For each pixel, the configuration is changed "
		+ "by shifting the position of the first body by the coordinate of the pixel.</p><p>Then a simulation is run for each pixel, "
		+ "calculating the gravitational forces between the bodies and adjusting the velocities and positions accordingly. "
		+ "How long and how accurately this simulation is simulated can be configured.</p><p>To get from the simulation to the colour of the pixel, "
		+ "I measure the distance of each body to its starting position. Since I then have three distances, "
		+ "I can generate a colour from them using the RGB colour format by simply putting each of the distances into one of the colour channels.</p>"
		+ "<p>To try out the program, click <a href='sandbox?lang=en'>here</a>. You can click on the image to see the simulation at the position of the image.</p>"
		+ "<p>I have tried other methods for calculating the colours as well. However, these are much more expensive to calculate " 
		+ "and can therefore only be executed in a desktop application. You can download this application <a href='https://github.com/Schlafhase/ThreeBody/releases/tag/v1.0.0' target='_blank'>here</a>.</p>";

	public override string ParametersTableHeader => "Parameter";
	public override string ParameterName => "Name";
	public override string ParameterDescription => "Description";

	public override string SandBoxWidthDescription =>
		"Width of the calculated image (The size of the displayed image depends on the size of the browser window)";

	public override string SandBoxHeightDescription => "Height of the calculated image";
	public override string SandBoxTimeDescription => "Number of complete timesteps simulated for each pixel";

	public override string SandBoxTimeStepDescription =>
		"Delta t for each iteration. 1/dt iterations are performed per timestep";

	public override string SandBoxZoomDescription => "Zoom factor";
	public override string SandBoxCenterXDescription => "X-Coordinate of the centre";
	public override string SandBoxCenterYDescription => "Y-Coordinate of the centre";
	public override string SandBoxConfigMassDescription => "Mass of the body";
	public override string SandBoxConfigPositionXDescription => "X-Coordinate of the body";
	public override string SandBoxConfigPositionYDescription => "Y-Coordinate of the body";
	public override string SandBoxConfigVelocityXDescription => "X-Component of the body's velocity";
	public override string SandBoxConfigVelocityYDescription => "Y-Component of the body's velocity";
}