// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Drawing;
using System.Runtime.Versioning;
using CSShaders.Shaders.Vectors;
using ThreeBody;
using ThreeBodyFractal;

[SupportedOSPlatform("windows")]
internal class Program
{
	public static void Main(string[] args)
	{
		int fractalType = int.MinValue;

		while (fractalType is < 1 or > 3)
		{
			if (fractalType != int.MinValue)
			{
				Console.BackgroundColor = ConsoleColor.Red;
				Console.ForegroundColor = ConsoleColor.Black;
				Console.WriteLine("Ungültige Eingabe.");
				Console.ResetColor();
			}

			readInt("Fraktaltyp (1: Distanz, 2: Radius, 3: Chaos)", out fractalType);
		}

		readInt("Breite", out int width);
		readInt("Höhe", out int height);

		readDouble("Zoom", out double zoom);
		readDouble("Zeitschrittweite", out double timeStep);
		readDouble("Mitte x", out double centerX);
		readDouble("Mitte y", out double centerY);

		Console.Write("Konfiguration selbst eingeben? (y/n) ");
		bool customConfig = Console.ReadKey().Key == ConsoleKey.Y;
		Console.WriteLine();

		PhysicsBody[] bodies = ThreeBodySimulator.GenerateStableConfiguration();

		if (customConfig)
		{
			Console.WriteLine("Bitte geben Sie die Positionen, Geschwindigkeiten und Massen der Körper ein.");

			for (int i = 0; i < 3; i++)
			{
				Console.WriteLine($"Körper {i}");

				readDouble("Position x", out double posX);
				readDouble("Position y", out double posY);
				readDouble("Geschwindigkeit x", out double velX);
				readDouble("Geschwindigkeit y", out double velY);
				readDouble("Masse", out double mass);

				bodies[i] = new PhysicsBody()
				{
					Mass = mass,
					Position = new Vec2(posX, posY),
					Velocity = new Vec2(velX, velY)
				};
			}
		}

		string fileName;

		switch (fractalType)
		{
			case 1:
			{
				readDouble("Zeit", out double time);

				using Bitmap bmp = Fractal.GetFractal(FractalType.Distance, bodies, width, height, time, timeStep,
													  new Vec2(centerX, centerY), zoom, true);
				fileName = "fractal-distance.png";
				bmp.Save(fileName);
				break;
			}
			case 3:
			{
				readDouble("Zeit", out double time);

				using Bitmap bmp = Fractal.GetFractalChaos(bodies, width, height, time, timeStep,
														   new Vec2(centerX, centerY), zoom, true);
				fileName = "fractal-chaos.png";
				bmp.Save(fileName);
				break;
			}
			default:
			{
				readInt("Maximale Iterationen", out int maxIterations);
				readInt("Radius", out int radius);

				using Bitmap bmp = Fractal.GetFractalIterations(bodies, width, height, maxIterations, radius, timeStep,
																new Vec2(centerX, centerY), zoom, true);
				fileName = "fractal-radius.png";
				bmp.Save(fileName);
				break;
			}
		}

		Console.WriteLine(
			$"Bild gespeichert unter {Path.Combine(Directory.GetCurrentDirectory(), fileName)}");
		Console.Write("Wollen Sie das Bild anzeigen? (y/n)");

		if (Console.ReadKey().Key == ConsoleKey.Y)
		{
			new Process
			{
				StartInfo = new ProcessStartInfo(fileName)
				{
					UseShellExecute = true
				}
			}.Start();
		}

		Console.WriteLine();

		Console.WriteLine("Drücken Sie eine beliebige Taste, um das Programm zu beenden.");
		Console.ReadKey();
	}

	private static void readDouble(string name, out double value)
	{
		Console.Write($"{name} (double): ");

		while (!double.TryParse(Console.ReadLine(), out value))
		{
			Console.BackgroundColor = ConsoleColor.Red;
			Console.ForegroundColor = ConsoleColor.Black;
			Console.WriteLine(
				"Ungültige Eingabe. (Hinweis: Nachkommastellen müssen durch einen Punkt abgetrennt werden)");
			Console.ResetColor();
			Console.Write($"{name}: ");
		}
	}

	private static void readInt(string name, out int value)
	{
		Console.Write($"{name} (Integer): ");

		while (!int.TryParse(Console.ReadLine(), out value))
		{
			Console.BackgroundColor = ConsoleColor.Red;
			Console.ForegroundColor = ConsoleColor.Black;
			Console.WriteLine("Ungültige Eingabe.");
			Console.ResetColor();
			Console.Write($"{name}: ");
		}
	}
}