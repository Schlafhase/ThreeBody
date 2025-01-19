namespace ThreeBodySandbox.Languages;

public abstract class Language
{
	public virtual string BitmapImageLoading => "bitmapImage.loading";
	
	public virtual string SandBoxGeneral => "sandbox.general";
	public virtual string SandBoxWidth => "sandbox.width";
	public virtual string SandBoxHeight => "sandbox.height";
	public virtual string SandBoxTime => "sandbox.time";
	public virtual string SandBoxTimeStep => "sandbox.timeStep";
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
}