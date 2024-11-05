using System.Numerics;

namespace ThreeBody.Physics;

public static class Gravity
{
    public static void SimulateGravity(PhysicsBody[] bodies, float timeStep)
    {
        for (int i = 0; i < bodies.Length; i++)
        {
            for (int j = 0; j < bodies.Length; j++)
            {
                if (i == j)
                {
                    continue;
                }

                Vector2 direction = bodies[j].Position - bodies[i].Position;
                float distance = direction.Length();
                float force = (0.1f * bodies[i].Mass * bodies[j].Mass) / (distance*distance);
                
                bodies[i].Velocity += direction * force * timeStep;
            }
        }
    }
}