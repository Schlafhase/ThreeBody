using System.Numerics;
using ThreeBody.Physics;

namespace ThreeBody;

public class ThreeBodySimulator
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

    public static PhysicsBody[] GenerateStableConfiguration(int version = 0)
    {
        switch (version)
        {
            case 0:
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
                    velocities[i] = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 15;
                }

                return new PhysicsBody[]
                {
                    new PhysicsBody { Position = positions[0], Velocity = velocities[0], Mass = 100 },
                    new PhysicsBody { Position = positions[1], Velocity = velocities[1], Mass = 100 },
                    new PhysicsBody { Position = positions[2], Velocity = velocities[2], Mass = 100 }
                };
            }

            case 1:
            {
                Vector2[] positions = new Vector2[3]
                {
                    new Vector2(0, 0),
                    new Vector2(-100, 0),
                    new Vector2(100, 0)
                };
                
                Vector2[] velocities = new Vector2[3]
                {
                    new Vector2(-10, -10),
                    new Vector2(10, 10),
                    new Vector2(10, 10)
                };
                
                return new PhysicsBody[]
                {
                    new PhysicsBody { Position = positions[0], Velocity = velocities[0], Mass = 500 },
                    new PhysicsBody { Position = positions[1], Velocity = velocities[1], Mass = 500 },
                    new PhysicsBody { Position = positions[2], Velocity = velocities[2], Mass = 500 }
                };
            }
        }
        throw new ArgumentException("Invalid version.", nameof(version));
    }
}
