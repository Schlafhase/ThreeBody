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

                float fractX = (x - width / 2) / zoom + center.X;
                float fractY = (y - height / 2) / zoom + center.Y;

                if (x == 400 && y == 400)
                {
                    Console.WriteLine($"{fractX}, {fractY}");
                }

                config[0].Position += new Vector2(fractX, fractY);
                config[1].Position += new Vector2(-fractX, fractY);

                PhysicsBody[] start = config.ToArray();

                ThreeBodySimulator.Simulate(config, time, 0.01f);
                lock (bmpLocker)
                {
                    bmp.SetPixel(x, y, Color.FromArgb(
                        Math.Clamp((int)(Vector2.Distance(config[0].Position, start[0].Position) * 0.6), 0, 255),
                        Math.Clamp((int)(Vector2.Distance(config[1].Position, start[1].Position) * 0.6), 0, 255),
                        Math.Clamp((int)(Vector2.Distance(config[2].Position, start[2].Position) * 0.6), 0, 255)));
                }
            }
        });
        
        // for (int x = 0; x < width; x++)
        // {
        //     
        // }

        return bmp;
    }
}