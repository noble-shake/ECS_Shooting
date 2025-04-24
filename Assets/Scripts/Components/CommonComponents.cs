using Unity.Entities;

public struct DamageBuffer : IBufferElementData
{
    public float Value;
}

public struct DestroyEntityFlag : IComponentData, IEnableableComponent { }