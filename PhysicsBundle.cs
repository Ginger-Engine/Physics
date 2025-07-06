using Engine.Core;
using Engine.Physics.Serialization;
using GignerEngine.DiContainer;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Engine.Physics;

public class PhysicsBundle : IBundle
{
    public void InstallBindings(DiBuilder builder)
    {
        builder.Bind<ColliderTypeResolver>();
    }
    
    public void Configure(string c, IReadonlyDiContainer diContainer)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        var config = deserializer.Deserialize<PhysicsConfig>(c);
        
        diContainer.Resolve<IPhysicsWorld>().Gravity = config.Gravity;
    }
}