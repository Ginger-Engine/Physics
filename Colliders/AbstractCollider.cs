namespace Engine.Physics.Colliders;

public abstract class AbstractCollider : ICollider
{
    public bool IsTrigger { get; set; }
    public float Density { get; set; }
    public float Friction { get; set; }
    public float Restitution { get; set; }
}