using System.Drawing;
using System.Numerics;
using System.Runtime.Versioning;
using ThreeBody;

namespace ThreeBodyFractal;

public enum FractalType
{
    Distance
}

[SupportedOSPlatform("windows")]
public static class Fractal
{
    public static Bitmap GetFractal(FractalType type, PhysicsBody[] startConfig, int width, int height, float time, float timeStep, Vector2 center, float zoom)
    {
        return type switch
        {
            FractalType.Distance => getFractal(startConfig, width, height, time, timeStep, center, zoom, getColourFromDistance),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    // TODO: CancellationToken
    private static Bitmap getFractal(PhysicsBody[] startConfig, int width, int height, float time, float timeStep, Vector2 center, float zoom, Func<PhysicsBody[], PhysicsBody[], float, Color> calculateColor)
    {
        object bmpLocker = new();
        Bitmap bmp = new(width, height);
        float factor = 8 / time;

        Parallel.For(0, width, x =>
        {
            Parallel.For(0, height, y =>
            {
                PhysicsBody[] config = startConfig.ToArray();

                float fractalX = (x - width / 2f) / zoom + center.X;
                float fractalY = (y - height / 2f) / zoom + center.Y;

                // if (x == 400 && y == 400)
                // {
                //     Console.WriteLine($"{fractalX}, {fractalY}");
                // }

                config[0].Position += new Vector2(fractalX, fractalY);
                // config[1].Position += new Vector2(-fractalX, fractalY);

                PhysicsBody[] start = config.ToArray();

                ThreeBodySimulator.Simulate(config, time, timeStep);

                lock (bmpLocker)
                {
                    bmp.SetPixel(x, y, calculateColor(start, config, factor));
                }
            });
        });
        
        // for (int x = 0; x < width; x++)
        // {
        //     
        // }

        return bmp;
    }
    
    private static Color getColourFromDistance(PhysicsBody[] a, PhysicsBody[] b, float factor = 0.3f)
    {
        return Color.FromArgb(
            Math.Clamp((int)(Vector2.Distance(a[0].Position, b[0].Position) * factor), 0, 255),
            Math.Clamp((int)(Vector2.Distance(a[1].Position, b[1].Position) * factor), 0, 255),
            Math.Clamp((int)(Vector2.Distance(a[2].Position, b[2].Position) * factor), 0, 255));
    }
}