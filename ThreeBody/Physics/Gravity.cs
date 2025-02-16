using CSShaders.Shaders.Vectors;

namespace ThreeBody.Physics;

public static class Gravity
{
    private const double gravitationalConstant = 0.1;
    
    public static void SimulateGravity(PhysicsBody[] bodies, double deltaTime)
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
                double force = (gravitationalConstant * bodies[i].Mass * bodies[j].Mass) / (distance*distance);
                
                bodies[i].Velocity += direction * force * deltaTime;
            }
        }
    }
}