namespace ThreeBodySandbox.Languages;

public class German : Language
{
	public override string BitmapImageLoading => "Wird geladen";

	public override string SandBoxGeneral => "Allgemein";
	public override string SandBoxWidth => "Breite";
	public override string SandBoxHeight => "Höhe";
	public override string SandBoxTime => "Zeitschritte";
	public override string SandBoxTimeStep => "Zeitschrittweite";
	public override string SandBoxZoom => "Zoom";
	public override string SandBoxCenterX => "Mitte X";
	public override string SandBoxCenterY => "Mitte Y";
	public override string SandBoxRender => "Berechnen";

	public override string SandBoxConfig => "Konfiguration";
	public override string SandBoxConfigBody => "Körper";
	public override string SandBoxConfigMass => "Masse";
	public override string SandBoxConfigPositionX => "X";
	public override string SandBoxConfigPositionY => "Y";
	public override string SandBoxConfigVelocityX => "Geschwindigkeit X";
	public override string SandBoxConfigVelocityY => "Geschwindigkeit Y";

	public override string ThreeBodyFractal => "Three-Body-Problem-Fraktal";
	public override string ThreeBodyFractalDescription => "Lorem ipsum"; // TODO:
	public override string ParametersTableHeader => "Parameter";
	public override string ParameterName => "Name";
	public override string ParameterDescription => "Beschreibung";

	public override string SandBoxWidthDescription =>
		"Breite des berechneten Bildes (Die Größe des angezeigten Bildes hängt von der Größe des Browserfensters ab)";
	public override string SandBoxHeightDescription => "Höhe des berechneten Bildes";
	public override string SandBoxTimeDescription => "Anzahl der Zeitschritte, die berechnet werden";
	public override string SandBoxTimeStepDescription => "Delta t für jede Iteration. Es werden 1/dt Iterationen pro Zeitschritt durchgeführt";
	public override string SandBoxZoomDescription => "Zoomfaktor";
	public override string SandBoxCenterXDescription => "X-Koordinate der Mitte";
	public override string SandBoxCenterYDescription => "Y-Koordinate der Mitte";
	public override string SandBoxConfigMassDescription => "Masse des Körpers";
	public override string SandBoxConfigPositionXDescription => "X-Koordinate des Körpers";
	public override string SandBoxConfigPositionYDescription => "Y-Koordinate des Körpers";
	public override string SandBoxConfigVelocityXDescription => "X-Komponente der Geschwindigkeit des Körpers";
	public override string SandBoxConfigVelocityYDescription => "Y-Komponente der Geschwindigkeit des Körpers";
}