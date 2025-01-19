namespace ThreeBodySandbox.Languages;

public class German : Language
{
	public override string BitmapImageLoading => "Wird geladen";
	
	public override string SandBoxGeneral => "Allgemein";
	public override string SandBoxWidth => "Breite:";
	public override string SandBoxHeight => "Höhe:";
	public override string SandBoxTime => "Zeitschritte:";
	public override string SandBoxTimeStep => "Zeitschrittweite:";
	public override string SandBoxZoom => "Zoom:";
	public override string SandBoxCenterX => "Mitte X:";
	public override string SandBoxCenterY => "Mitte Y:";
	public override string SandBoxRender => "Berechnen";
	
	public override string SandBoxConfig => "Konfiguration";
	public override string SandBoxConfigBody => "Körper";
	public override string SandBoxConfigMass => "Masse:";
	public override string SandBoxConfigPositionX => "X:";
	public override string SandBoxConfigPositionY => "Y:";
	public override string SandBoxConfigVelocityX => "Geschwindigkeit X:";
	public override string SandBoxConfigVelocityY => "Geschwindigkeit Y:";
}