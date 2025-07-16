using System.Numerics;
using Engine.Core.Behaviours;
using Engine.Core.Entities;
using Engine.Core.Transform;
using Engine.Physics.Components;

namespace Engine.Physics.Behaviours;

public class RigidbodyBehaviour(IPhysicsWorld physicsWorld) : IEntityBehaviour
{
    private readonly Dictionary<Entity, IDisposable> _subscriptions = new();

    public void OnStart(Entity entity)
    {
        var transform = entity.GetComponent<TransformComponent>();
        var rigidbody = entity.GetComponent<RigidbodyComponent>();
        var collider = entity.GetComponent<ColliderComponent>();

        var body = physicsWorld.CreateBody(new BodyDefinition
        {
            Position = transform.WorldTransform.Position,
            Rotation = transform.WorldTransform.Rotation,
            LinearVelocity = rigidbody.LinearVelocity,
            AngularVelocity = rigidbody.AngularVelocity,
            LinearDamping = rigidbody.LinearDamping,
            AngularDamping = rigidbody.AngularDamping,
            IsSleeping = rigidbody.IsSleeping,
            AllowSleep = rigidbody.AllowSleep,
            IsBullet = rigidbody.CollisionDetectionMode == CollisionDetectionMode.Continuous,
            BodyType = rigidbody.BodyType,
            FixedRotation = rigidbody.FixedRotation,
            MassData = new MassData { Mass = rigidbody.Mass, Center = Vector2.Zero }
        }, collider.Colliders);
        
        rigidbody.RuntimeBody = body;
        entity.ApplyComponent(rigidbody);

        if (rigidbody.BodyType == PhysicsBodyType.Kinematic)
        {
            var subscription = entity.SubscribeComponentChange<TransformComponent>((e) =>
            {
                body.Position = e.newValue.WorldTransform.Position;
                body.Rotation = e.newValue.WorldTransform.Rotation;
            });
            _subscriptions.Add(entity, subscription);
        }
    }

    public void OnUpdate(Entity entity, float dt)
    {
        var rigidbody = entity.GetComponent<RigidbodyComponent>();
        if (rigidbody.BodyType == PhysicsBodyType.Kinematic)
            return;

        entity.Modify((ref TransformComponent transformComponent) =>
        {
            if (rigidbody.RuntimeBody == null) throw new Exception("RigidBody has not been initialized");
            Transform newWorldTransform = new()
            {
                Position = rigidbody.RuntimeBody.Position,
                Rotation = rigidbody.RuntimeBody.Rotation,
                Scale = transformComponent.WorldTransform.Scale,
            };
            transformComponent.WorldTransform = newWorldTransform; 
        });
    }

    public void OnDestroy(Entity entity)
    {
        if (_subscriptions.Remove(entity, out var posSub))
            posSub.Dispose();

        var rigidbody = entity.GetComponent<RigidbodyComponent>();
        
        if (rigidbody.RuntimeBody == null) throw new Exception("RigidBody has not been initialized");
        physicsWorld.DestroyBody(rigidbody.RuntimeBody);
    }
}
