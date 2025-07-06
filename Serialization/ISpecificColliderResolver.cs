namespace Engine.Physics.Serialization;

public interface ISpecificColliderResolver
{
    public Type Type { get; }
    
    public object Resolve(object raw);
}