// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using ThreeBody;
using ThreeBodyFractal;

internal class Program
{
	public static void Main(string[] args)
	{
		Console.Write("Welches Fraktal möchten Sie berechnen? (1: Distanz 2: Radius) ");
		bool distance = Console.ReadKey().Key == ConsoleKey.D1;
		Console.WriteLine();

		readInt("Breite", out int width);
		readInt("Höhe", out int height);

		readFloat("Zoom", out float zoom);
		readFloat("Zeitschrittweite", out float timeStep);
		readFloat("Mitte x", out float centerX);
		readFloat("Mitte y", out float centerY);

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

				readFloat("Position x", out float posX);
				readFloat("Position y", out float posY);
				readFloat("Geschwindigkeit x", out float velX);
				readFloat("Geschwindigkeit y", out float velY);
				readFloat("Masse", out float mass);

				bodies[i] = new PhysicsBody()
				{
					Mass = mass,
					Position = new Vector2(posX, posY),
					Velocity = new Vector2(velX, velY)
				};
			}
		}

		string fileName;

		if (distance)
		{
			readFloat("Zeit", out float time);

			using Bitmap bmp = Fractal.GetFractal(FractalType.Distance, bodies, width, height, time, timeStep,
												  new Vector2(centerX, centerY), zoom, true);
			fileName = "fractal-distance.png";
			bmp.Save(fileName);
		}
		else
		{
			readInt("Maximale Iterationen", out int maxIterations);
			readInt("Radius", out int radius);

			using Bitmap bmp = Fractal.GetFractalIterations(bodies, width, height, maxIterations, radius, timeStep,
															new Vector2(centerX, centerY), zoom, true);
			fileName = "fractal-radius.png";
			bmp.Save(fileName);
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

	private static void readFloat(string name, out float value)
	{
		Console.Write($"{name} (float): ");

		while (!float.TryParse(Console.ReadLine(), out value))
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