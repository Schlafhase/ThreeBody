using System.Numerics;

namespace ThreeBody;

public struct PhysicsBody
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    
    public float Mass { get; set; }
}