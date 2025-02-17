using System.Drawing;
using System.Runtime.Versioning;
using CSShaders.Shaders.Vectors;
using ThreeBody;

namespace ThreeBodyFractal;

public enum FractalType
{
    Distance,
    // Angle
}

[SupportedOSPlatform("windows")]
public static class Fractal
{
    private static Color[] palette =
    [
        Color.DarkBlue, Color.Aqua, Color.White, Color.Yellow, Color.Orange, Color.OrangeRed, Color.Red, Color.Crimson,
        Color.DarkBlue, Color.Aqua
    ];
    
    public static Bitmap GetFractal(FractalType type, PhysicsBody[] startConfig, int width, int height, double time, double deltaTime, Vec2 center, double zoom, bool logProgress = false)
    {
        return type switch
        {
            FractalType.Distance => getFractal(startConfig, width, height, time, deltaTime, center, zoom, getColourFromDistance, logProgress),
            // FractalType.Angle => getFractal(startConfig, width, height, time, deltaTime, center, zoom, getColourFromAngle),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
    
    public static Bitmap GetFractalIterations(PhysicsBody[] startConfig, int width, int height, int maxIterations, int radius, double deltaTime, Vec2 center, double zoom, bool logProgress = false)
    {
        object bmpLocker = new();
        object consoleLocker = new();
        Bitmap bmp = new(width, height);

        DateTime start = DateTime.UtcNow;
        int column = 0;
        
        Parallel.For(0, width, x =>
        {
            Parallel.For(0, height, y =>
            {
                PhysicsBody[] config = startConfig.ToArray();

                double fractalX = (x - width / 2f) / zoom + center.X;
                double fractalY = (y - height / 2f) / zoom + center.Y;

                config[0].Position += new Vec2(fractalX, fractalY);

                int iterations = ThreeBodySimulator.SimulateUntil(config, deltaTime,
                                                 (bodies, i) =>
                                                     bodies.Any(body => body.Position.X * body.Position.X +
                                                                    body.Position.Y * body.Position.Y >
                                                                    radius * radius) || i > maxIterations);

                lock (bmpLocker)
                {
                    Color colour = iterations >= maxIterations ? Color.Black : lerpColours(palette, iterations / (double)maxIterations);
                    bmp.SetPixel(x, y, colour);
                }
            });

            if (!logProgress)
            {
                return;
            }

            lock (consoleLocker)
            {
                if (column == 0)
                {
                    Console.WriteLine();
                    Console.WriteLine();
                }

                column++;
                Console.SetCursorPosition(0, Console.CursorTop - 2);
                
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(string.Join("", Enumerable.Repeat("#", (int)(column / (double)width * 50))));
                
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(string.Join("", Enumerable.Repeat(".", 50 - (int)(column / (double)width * 50))));
                
                Console.ResetColor();
                Console.WriteLine($"Vergangene Zeit: {DateTime.UtcNow - start}");
                Console.Write($"ETA: {(DateTime.UtcNow - start) / (column + 1) * (width - column)}");
            }
        });
        if (logProgress)
        {
            Console.WriteLine();
        }
        
        return bmp;
    }

    public static Bitmap GetFractalChaos(PhysicsBody[] startConfig, int width, int height, double time, double deltaTime, Vec2 center, double zoom, bool logProgress = false)
    {
        object bmpLocker = new();
        object consoleLocker = new();
        Bitmap bmp = new(width, height);
        double factor = 8 / time;

        DateTime start = DateTime.UtcNow;
        int column = 0;
        
        Parallel.For(0, width, x =>
        {
            Parallel.For(0, height, y =>
            {
                PhysicsBody[] config = startConfig.ToArray();

                double fractalX = (x - width / 2f) / zoom + center.X;
                double fractalY = (y - height / 2f) / zoom + center.Y;

                config[0].Position += new Vec2(fractalX, fractalY);

                PhysicsBody[] start = config.ToArray();

                ThreeBodySimulator.Simulate(config, time, deltaTime, out int directionChanges);

                lock (bmpLocker)
                {
                    // Color colour = lerpColours(palette, directionChanges / time);
                    // bmp.SetPixel(x, y, colour);
                    double brightness = Math.Clamp(directionChanges / (time / 600), 0, 255);
                    bmp.SetPixel(x, y, Color.FromArgb((int)brightness, (int)brightness, (int)brightness));
                }
            });
            
            if (!logProgress)
            {
                return;
            }

            lock (consoleLocker)
            {
                if (column == 0)
                {
                    Console.WriteLine();
                    Console.WriteLine();
                }

                column++;
                Console.SetCursorPosition(0, Console.CursorTop - 2);
                
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(string.Join("", Enumerable.Repeat("#", (int)(column / (double)width * 50))));
                
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(string.Join("", Enumerable.Repeat(".", 50 - (int)(column / (double)width * 50))));
                
                Console.ResetColor();
                Console.WriteLine($"Vergangene Zeit: {DateTime.UtcNow - start}");
                Console.Write($"ETA: {(DateTime.UtcNow - start) / (column + 1) * (width - column)}");
            }
        });
        
        // for (int x = 0; x < width; x++)
        // {
        //     
        // }

        return bmp;
    }
    
    private static Color lerpColours(Color[] palette, double t)
    {
        if (palette == null || palette.Length == 0)
        {
            throw new ArgumentException("Palette must contain at least one color.", nameof(palette));
        }

        switch (t)
        {
            case <= 0:
                return palette[0];
            case >= 1:
                return palette[^1];
        }

        double scaledT = t * (palette.Length - 2);
        int index = (int)scaledT;
        double fraction = scaledT - index;

        Color color1 = palette[index];
        Color color2 = palette[index + 1];

        int r = (int)(color1.R + (color2.R - color1.R) * fraction);
        int g = (int)(color1.G + (color2.G - color1.G) * fraction);
        int b = (int)(color1.B + (color2.B - color1.B) * fraction);

        return Color.FromArgb(r, g, b);
    }

    private static Bitmap getFractal(PhysicsBody[] startConfig, int width, int height, double time, double deltaTime, Vec2 center, double zoom, Func<PhysicsBody[], PhysicsBody[], double, Color> calculateColor, bool logProgress = false)
    {
        object bmpLocker = new();
        object consoleLocker = new();
        Bitmap bmp = new(width, height);
        double factor = 8 / time;

        DateTime start = DateTime.UtcNow;
        int column = 0;
        
        Parallel.For(0, width, x =>
        {
            Parallel.For(0, height, y =>
            {
                PhysicsBody[] config = startConfig.ToArray();

                double fractalX = (x - width / 2f) / zoom + center.X;
                double fractalY = (y - height / 2f) / zoom + center.Y;

                // if (x == 400 && y == 400)
                // {
                //     Console.WriteLine($"{fractalX}, {fractalY}");
                // }

                config[0].Position += new Vec2(fractalX, fractalY);
                // config[1].Position += new Vec2(-fractalX, fractalY);

                PhysicsBody[] start = config.ToArray();

                ThreeBodySimulator.Simulate(config, time, deltaTime, out _);

                lock (bmpLocker)
                {
                    bmp.SetPixel(x, y, calculateColor(start, config, factor));
                }
            });
            
            if (!logProgress)
            {
                return;
            }

            lock (consoleLocker)
            {
                if (column == 0)
                {
                    Console.WriteLine();
                    Console.WriteLine();
                }

                column++;
                Console.SetCursorPosition(0, Console.CursorTop - 2);
                
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(string.Join("", Enumerable.Repeat("#", (int)(column / (double)width * 50))));
                
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(string.Join("", Enumerable.Repeat(".", 50 - (int)(column / (double)width * 50))));
                
                Console.ResetColor();
                Console.WriteLine($"Vergangene Zeit: {DateTime.UtcNow - start}");
                Console.Write($"ETA: {(DateTime.UtcNow - start) / (column + 1) * (width - column)}");
            }
        });
        
        // for (int x = 0; x < width; x++)
        // {
        //     
        // }

        return bmp;
    }
    
    private static Color getColourFromDistance(PhysicsBody[] a, PhysicsBody[] b, double factor = 0.3f)
    {
        return Color.FromArgb(
            Math.Clamp((int)((a[0].Position - b[0].Position).Length * factor), 0, 255),
            Math.Clamp((int)((a[1].Position - b[1].Position).Length * factor), 0, 255),
            Math.Clamp((int)((a[2].Position - b[2].Position).Length * factor), 0, 255));
    }
}