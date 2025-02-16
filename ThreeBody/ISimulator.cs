namespace ThreeBody;

public interface ISimulator
{
    void Simulate(PhysicsBody[] bodies, float time, float deltaTime);
}