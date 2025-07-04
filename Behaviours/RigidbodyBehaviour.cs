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
            Position = transform.Position,
            Rotation = transform.Rotation,
            LinearVelocity = rigidbody.LinearVelocity,
            AngularVelocity = rigidbody.AngularVelocity,
            LinearDamping = rigidbody.LinearDamping,
            AngularDamping = rigidbody.AngularDamping,
            IsSleeping = rigidbody.IsSleeping,
            AllowSleep = rigidbody.AllowSleep,
            IsBullet = rigidbody.CollisionDetectionMode == CollisionDetectionMode.Continuous,
            BodyType = rigidbody.BodyType,
            FixedRotation = rigidbody.FixedRotation
        }, collider.Colliders);
        
        rigidbody.RuntimeBody = body;
        entity.ApplyComponent(rigidbody);

        if (rigidbody.BodyType == PhysicsBodyType.Kinematic)
        {
            var subscription = entity.SubscribeComponentChange<TransformComponent>((newValue, _) =>
            {
                body.Position = newValue.Position;
                body.Rotation = newValue.Rotation;
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
            transformComponent.Position = rigidbody.RuntimeBody.Position;
            transformComponent.Rotation = rigidbody.RuntimeBody.Rotation;
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
