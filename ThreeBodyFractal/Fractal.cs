using System.Drawing;
using System.Numerics;
using ThreeBody;

namespace ThreeBodyFractal;

public static class Fractal
{
    public static Bitmap GetFractal(PhysicsBody[] startConfig, int width, int height, float time, Vector2 center, float zoom)
    {
        object bmpLocker = new();
        Bitmap bmp = new(width, height);

        Parallel.For(0, width, x =>
        {
            for (int y = 0; y < height; y++)
            {
                PhysicsBody[] config = startConfig.ToArray();

                float fractalX = (x - width / 2) / zoom + center.X;
                float fractalY = (y - height / 2) / zoom + center.Y;

                if (x == 400 && y == 400)
                {
                    Console.WriteLine($"{fractalX}, {fractalY}");
                }

                config[0].Position += new Vector2(fractalX, fractalY);
                config[1].Position += new Vector2(-fractalX, fractalY);

                PhysicsBody[] start = config.ToArray();

                ThreeBodySimulator.Simulate(config, time, 0.05f);
                lock (bmpLocker)
                {
                    bmp.SetPixel(x, y, GetColorFromDistance(start, config));
                }
            }
        });
        
        // for (int x = 0; x < width; x++)
        // {
        //     
        // }

        return bmp;
    }
    
    private static Color GetColorFromDistance(PhysicsBody[] a, PhysicsBody[] b)
    {
        return Color.FromArgb(
            Math.Clamp((int)(Vector2.Distance(a[0].Position, b[0].Position) * 0.3), 0, 255),
            Math.Clamp((int)(Vector2.Distance(a[1].Position, b[1].Position) * 0.3), 0, 255),
            Math.Clamp((int)(Vector2.Distance(a[2].Position, b[2].Position) * 0.3), 0, 255));
    }
    
    private static Color GetColorFromAngle(PhysicsBody[] a, PhysicsBody[] b)
    {
        return Color.FromArgb(
            Math.Clamp((int)(Math.Atan2(b[0].Position.Y, b[0].Position.X) * 40.58), 0, 255),
            Math.Clamp((int)(Math.Atan2(b[1].Position.Y, b[1].Position.X) * 40.58), 0, 255),
            Math.Clamp((int)(Math.Atan2(b[2].Position.Y, b[2].Position.X) * 40.58), 0, 255));
    }
}