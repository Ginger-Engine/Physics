using Engine.Core;
using Engine.Physics.Colliders;

namespace Engine.Physics.Components;

public struct ColliderComponent : IComponent
{
    public ICollider[] Colliders;
}