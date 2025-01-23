namespace ThreeBodySandbox.Languages;

public class German : Language
{
	public override string LanguageCode => "de";

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

	public override string ThreeBodyFractal => "Three-Body Fraktal";

	public override string ThreeBodyFractalDescription =>
		"<p>Jedem Pixel des Bildes wird eine Konfiguration aus drei Körpern (jeweils Position, Geschwindigkeit und Masse) zugeordnet."
		+ " Diese Konfiguration wird wie folgt berechnet: Die Konfiguration am Pixel 0, 0 ist eine stabile Konfiguration, "
		+ "bei der alle Körper in regelmäßigen Abständen auf einem Kreis mit einem Radius von 100 platziert sind. Die Geschwindigkeiten "
		+ "sind Vektoren mit einer Magnitude von 10 pi, die tangential vom Kreis weg zeigen. Für jedes Pixel wird die Konfiguration verändert, "
		+ "indem die Position des ersten Körpers um die Koordinate des Pixels verschoben wird. </p><p>Dann wird für jedes Pixel eine Simulation ausgeführt, "
		+ "die die Anziehungskräfte zwischen den Körpern berechnet und die Geschwindigkeiten und Positionen anhand davon anpassen. "
		+ "Wie lange und genau diese Simulation simuliert wird, können Sie einstellen.</p><p>Um dann von der Simulation auf die Farbe des Pixels zu kommen, "
		+ "messe ich die Distanz jedes Körpers zu seiner Startposition. Da ich dann drei Distanzen habe, "
		+ "kann ich mithilfe des RGB-Farbformates eine Farbe daraus generieren, indem ich einfach jede der Distanzen in einen der Farbkanäle tue.</p>"
		+ "<p>Um das Programm auszuprobieren, klicken sie <a href='sandbox'>hier</a>. Sie können auf das Bild klicken, um die Simulation an der Stelle des Bildes zu sehen.</p>"
		+ "<p>Ich habe noch andere Methoden für die Berechnung der Farben ausprobiert. Diese sind jedoch sehr viel aufwendiger zu berechnen " 
		+ "und können daher nur in einer Desktopapplikation ausgeführt werden. Diese Applikation können sie <a href='https://github.com/Schlafhase/ThreeBody/releases/tag/v1.0.0' target='_blank'>hier</a> herunterladen.</p>";

	public override string ParametersTableHeader => "Parameter";
	public override string ParameterName => "Name";
	public override string ParameterDescription => "Beschreibung";

	public override string SandBoxWidthDescription =>
		"Breite des berechneten Bildes (Die Größe des angezeigten Bildes hängt von der Größe des Browserfensters ab)";

	public override string SandBoxHeightDescription => "Höhe des berechneten Bildes";
	public override string SandBoxTimeDescription => "Anzahl der Zeitschritte, die für jeden Pixel simuliert werden";

	public override string SandBoxTimeStepDescription =>
		"Delta t für jede Iteration. Es werden 1/dt Iterationen pro Zeitschritt durchgeführt";

	public override string SandBoxZoomDescription => "Zoomfaktor";
	public override string SandBoxCenterXDescription => "X-Koordinate der Mitte";
	public override string SandBoxCenterYDescription => "Y-Koordinate der Mitte";
	public override string SandBoxConfigMassDescription => "Masse des Körpers";
	public override string SandBoxConfigPositionXDescription => "X-Koordinate des Körpers";
	public override string SandBoxConfigPositionYDescription => "Y-Koordinate des Körpers";
	public override string SandBoxConfigVelocityXDescription => "X-Komponente der Geschwindigkeit des Körpers";
	public override string SandBoxConfigVelocityYDescription => "Y-Komponente der Geschwindigkeit des Körpers";
}