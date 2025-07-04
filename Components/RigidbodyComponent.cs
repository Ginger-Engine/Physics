using System.Numerics;
using Engine.Core;

namespace Engine.Physics.Components;

public enum PhysicsBodyType
{
    Static,
    Dynamic,
    Kinematic,
}

public enum CollisionDetectionMode
{
    Discrete,
    Continuous,
} 

public struct RigidbodyComponent : IComponent
{
    public PhysicsBodyType BodyType;
    public float Mass;
    public Vector2 LinearVelocity;
    public float AngularVelocity;
    public float LinearDamping;
    public float AngularDamping;
    public bool AllowSleep;
    public bool IsSleeping;
    public CollisionDetectionMode CollisionDetectionMode;
    public bool FixedRotation;
    
    public IPhysicsBody? RuntimeBody;
}