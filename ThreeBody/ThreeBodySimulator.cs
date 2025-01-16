using System.Drawing;
using System.Numerics;
using Canvas.Components;
using ThreeBody.Physics;

namespace ThreeBody;

public static class ThreeBodySimulator
{
    public static void Simulate(PhysicsBody[] bodies, float time, float timeStep)
    {
        #region Validation

        if (timeStep <= 0)
        {
            throw new ArgumentException("Time step must be greater than zero.", nameof(timeStep));
        }

        if (bodies.Length != 3)
        {
            throw new ArgumentException("Three bodies are required.", nameof(bodies));
        }

        switch (time)
        {
            case < 0:
                throw new ArgumentException("Time must be greater than or equal to zero.", nameof(time));
            case 0:
                return;
        }

        #endregion

        int steps = (int)(time / timeStep);

        for (int i = 0; i < steps; i++)
        {
            Gravity.SimulateGravity(bodies, timeStep);
            for (int j = 0; j < bodies.Length; j++)
            {
                bodies[j].Position += bodies[j].Velocity * timeStep;
            }
        }
    }
    
    public static int SimulateUntil(PhysicsBody[] bodies, float timeStep, Func<PhysicsBody[], int, bool> predicate)
    {
        #region Validation

        if (timeStep <= 0)
        {
            throw new ArgumentException("Time step must be greater than zero.", nameof(timeStep));
        }

        if (bodies.Length != 3)
        {
            throw new ArgumentException("Three bodies are required.", nameof(bodies));
        }

        #endregion
        
        int iterations = 0;

        while (!predicate(bodies, iterations))
        {
            Gravity.SimulateGravity(bodies, timeStep);
            for (int i = 0; i < bodies.Length; i++)
            {
                bodies[i].Position += bodies[i].Velocity * timeStep;
            }
            iterations++;
        }
        
        return iterations;
    }

    public static PhysicsBody[] GenerateStableConfiguration(int version = 0)
    {
        switch (version)
        {
            case 0 or 1:
            {
                Vector2[] positions = new Vector2[3];
                Vector2[] velocities = new Vector2[3];

                for (int i = 0; i < positions.Length; i++)
                {
                    positions[i] = new Vector2(100 * (float)Math.Cos(i * 2 * Math.PI / 3),
                        100 * (float)Math.Sin(i * 2 * Math.PI / 3));
                }

                for (int i = 0; i < velocities.Length; i++)
                {
                    float angle = (float)Math.Atan2(positions[i].Y, positions[i].X) + (float)Math.PI / 2f;
                    velocities[i] = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * (version == 0 ? MathF.PI * 10 : 15);
                }

                return
                [
                    new PhysicsBody { Position = positions[0], Velocity = velocities[0], Mass = 100 },
                    new PhysicsBody { Position = positions[1], Velocity = velocities[1], Mass = 100 },
                    new PhysicsBody { Position = positions[2], Velocity = velocities[2], Mass = 100 }
                ];
            }
        }
        throw new ArgumentException("Invalid version.", nameof(version));
    }

    public static Bitmap GetSimulationImage(PhysicsBody[] startConfig,
        int width,
        int height,
        float time,
        float timeStep,
        bool showDistance = false)
    {
        Bitmap bmp = new(width, height);
        using Graphics g = Graphics.FromImage(bmp);
        g.Clear(Color.Black);
        
        GlowDot body1Start = new((int)startConfig[0].Position.X + width/2, (int)startConfig[0].Position.Y + height/2, 5, 80, Color.FromArgb(255, 1, 0, 0));
        GlowDot body2Start = new((int)startConfig[1].Position.X + width/2, (int)startConfig[1].Position.Y + height/2, 5, 80, Color.FromArgb(255, 0, 1, 0));
        GlowDot body3Start = new((int)startConfig[2].Position.X + width/2, (int)startConfig[2].Position.Y + height/2, 5, 80, Color.FromArgb(255, 0, 0, 1));
        
        Simulate(startConfig, time, timeStep);
        
        GlowDot body1End = new((int)startConfig[0].Position.X + width/2, (int)startConfig[0].Position.Y + height/2, 10, 80, Color.Red);
        GlowDot body2End = new((int)startConfig[1].Position.X + width/2, (int)startConfig[1].Position.Y + height/2, 10, 80, Color.Lime);
        GlowDot body3End = new((int)startConfig[2].Position.X + width/2, (int)startConfig[2].Position.Y + height/2, 10, 80, Color.Blue);
        
        body1Start.Put(g);
        body2Start.Put(g);
        body3Start.Put(g);
        
        body1End.Put(g);
        body2End.Put(g);
        body3End.Put(g);
        
        if (showDistance)
        {
            float distance = Vector2.Distance(new Vector2(body1Start.X, body1Start.Y), new Vector2(body1End.X, body1End.Y));
            float centerX = (body1Start.X + body1End.X) / 2;
            float centerY = (body1Start.Y + body1End.Y) / 2;
            g.DrawLine(Pens.Gray, body1Start.X, body1Start.Y, body1End.X, body1End.Y);
            g.DrawString(distance.ToString("0.##"), new Font("Arial", 12), Brushes.White, centerX, centerY);
            
            distance = Vector2.Distance(new Vector2(body2Start.X, body2Start.Y), new Vector2(body2End.X, body2End.Y));
            centerX = (body2Start.X + body2End.X) / 2;
            centerY = (body2Start.Y + body2End.Y) / 2;
            g.DrawLine(Pens.Gray, body2Start.X, body2Start.Y, body2End.X, body2End.Y);
            g.DrawString(distance.ToString("0.##"), new Font("Arial", 12), Brushes.White, centerX, centerY);
            
            distance = Vector2.Distance(new Vector2(body3Start.X, body3Start.Y), new Vector2(body3End.X, body3End.Y));
            centerX = (body3Start.X + body3End.X) / 2;
            centerY = (body3Start.Y + body3End.Y) / 2;
            g.DrawLine(Pens.Gray, body3Start.X, body3Start.Y, body3End.X, body3End.Y);
            g.DrawString(distance.ToString("0.##"), new Font("Arial", 12), Brushes.White, centerX, centerY);
        }
        
        return bmp;
    }
}
