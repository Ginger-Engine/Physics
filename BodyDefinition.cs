using System.Numerics;
using Engine.Physics.Components;

namespace Engine.Physics;

public struct MassData
{
    public float Mass;
    public Vector2 Center;
}

public struct BodyDefinition()
{
    public MassData MassData = new();
    public Vector2 Position = Vector2.Zero;
    public float Rotation = 0.0f;
    public Vector2 LinearVelocity = Vector2.Zero;
    public float AngularVelocity = 0.0f;
    public float LinearDamping = 0.0f;
    public float AngularDamping = 0.0f;
    public bool AllowSleep = true;
    public bool IsSleeping = false;
    public bool FixedRotation = false;
    public bool IsBullet = false;
    public PhysicsBodyType BodyType { get; set; }
}