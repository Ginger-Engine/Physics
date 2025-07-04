using System.Numerics;
using Engine.Physics.Colliders;

namespace Engine.Physics;

public interface IPhysicsWorld
{
    public Vector2 Gravity { get; set; }
    IPhysicsBody CreateBody(BodyDefinition bodyDefinition, ICollider[] colliders);
    public void Step(float dt);
    void DestroyBody(IPhysicsBody body);
}