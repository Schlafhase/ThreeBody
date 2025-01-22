using CSShaders.Shaders.Vectors;

namespace ThreeBody.Physics;

public static class Gravity
{
    public static void SimulateGravity(PhysicsBody[] bodies, double timeStep)
    {
        for (int i = 0; i < bodies.Length; i++)
        {
            for (int j = 0; j < bodies.Length; j++)
            {
                if (i == j)
                {
                    continue;
                }

                Vec2 direction = bodies[j].Position - bodies[i].Position;
                double distance = direction.Length;
                double force = (0.1f * bodies[i].Mass * bodies[j].Mass) / (distance*distance);
                
                bodies[i].Velocity += direction * force * timeStep;
            }
        }
    }
}