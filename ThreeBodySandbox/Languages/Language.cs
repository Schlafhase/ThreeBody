namespace ThreeBodySandbox.Languages;

public abstract class Language
{
	public abstract string LanguageCode { get; }
	public virtual string BitmapImageLoading => "bitmapImage.loading";
	
	public virtual string SandBoxGeneral => "sandbox.general";
	public virtual string SandBoxWidth => "sandbox.width";
	public virtual string SandBoxHeight => "sandbox.height";
	public virtual string SandBoxTime => "sandbox.time";
	public virtual string SandBoxTimeStep => "sandbox.deltaTime";
	public virtual string SandBoxZoom => "sandbox.zoom";
	public virtual string SandBoxCenterX => "sandbox.centerX";
	public virtual string SandBoxCenterY => "sandbox.centerY";
	public virtual string SandBoxRender => "sandbox.render";
	
	public virtual string SandBoxConfig => "sandbox.config";
	public virtual string SandBoxConfigBody => "sandbox.config.body";
	public virtual string SandBoxConfigMass => "sandbox.config.mass";
	public virtual string SandBoxConfigPositionX => "sandbox.config.positionX";
	public virtual string SandBoxConfigPositionY => "sandbox.config.positionY";
	public virtual string SandBoxConfigVelocityX => "sandbox.config.velocityX";
	public virtual string SandBoxConfigVelocityY => "sandbox.config.velocityY";
	
	public virtual string ThreeBodyFractal => "text.threeBodyFractal";
	public virtual string ThreeBodyFractalDescription => "text.threeBodyFractalDescription";
	public virtual string ParametersTableHeader => "text.parametersTableHeader";
	public virtual string ParameterName => "text.parameterName";
	public virtual string ParameterDescription => "text.parameterDescription";
	public virtual string SandBoxWidthDescription => "text.sandboxWidthDescription";
	public virtual string SandBoxHeightDescription => "text.sandboxHeightDescription";
	public virtual string SandBoxTimeDescription => "text.sandboxTimeDescription";
	public virtual string SandBoxTimeStepDescription => "text.sandboxTimeStepDescription";
	public virtual string SandBoxZoomDescription => "text.sandboxZoomDescription";
	public virtual string SandBoxCenterXDescription => "text.sandboxCenterXDescription";
	public virtual string SandBoxCenterYDescription => "text.sandboxCenterYDescription";
	public virtual string SandBoxConfigMassDescription => "text.sandboxConfigMassDescription";
	public virtual string SandBoxConfigPositionXDescription => "text.sandboxConfigPositionXDescription";
	public virtual string SandBoxConfigPositionYDescription => "text.sandboxConfigPositionYDescription";
	public virtual string SandBoxConfigVelocityXDescription => "text.sandboxConfigVelocityXDescription";
	public virtual string SandBoxConfigVelocityYDescription => "text.sandboxConfigVelocityYDescription";
	
	public static Language GetLanguage(string languageCode)
	{
		return languageCode switch
		{
			"de" => new German(),
			"en" => new English(),
			_ => new German()
		};
	}
}