using Engine.Core.Serialization;
using Engine.Physics.Colliders;

namespace Engine.Physics.Serialization;

public class ColliderTypeResolver: ITypeResolver<ICollider>
{
    private Dictionary<string, ISpecificColliderResolver> _colliderResolvers;
    
    public ColliderTypeResolver(IEnumerable<ISpecificColliderResolver> colliderResolvers)
    {
        _colliderResolvers = colliderResolvers.ToDictionary<ISpecificColliderResolver, string, ISpecificColliderResolver>(resolver => resolver.Type.FullName, resolver => resolver);
    }

    public object? Resolve(Type type, object raw)
    {
        if (raw is not Dictionary<object, object> dict)
            throw new ArgumentException("Raw collider must be a dictionary");

        if (!dict.TryGetValue("type", out var typeNameObj) || typeNameObj is not string typeName)
            throw new InvalidOperationException("Collider definition must contain a 'type' field");

        if (!_colliderResolvers.TryGetValue(typeName, out var resolver))
            throw new InvalidOperationException($"No resolver found for collider type '{typeName}'");

        return resolver.Resolve(raw);
    }
}