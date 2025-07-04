using System.Numerics;

namespace Engine.Physics;

public interface IPhysicsBody
{
    Vector2 Position { get; set; }
    float Rotation { get; set; }

    void ApplyForce(Vector2 force);
    void ApplyLinearImpulse(Vector2 impulse);

    Vector2 GetLinearVelocity();
    void SetLinearVelocity(Vector2 velocity);

    void SetFixedRotation(bool isFixed);
}