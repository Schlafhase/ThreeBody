using CSShaders.Shaders.Vectors;

namespace ThreeBody;

public struct PhysicsBody
{
    public Vec2 Position { get; set; }
    public Vec2 Velocity { get; set; }
    
    public double Mass { get; set; }
}