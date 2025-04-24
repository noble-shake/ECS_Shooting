using UnityEngine;
using Unity.Entities;
using Unity.Rendering;

#region Common
public struct BulletTag : IComponentData { }

public struct BulletPenetratable : IComponentData { public int Value;}

public struct BulletDamage : IComponentData
{
    public float Value;
}

public struct PenetratedBuffer : IBufferElementData {public Entity alreadyEntity; }

#endregion

#region Player
public struct PlayerBulletTag : IComponentData { }

public struct PlayerBulletSpeed : IComponentData
{
    public float Value;
}

public struct PlayerNormalBulletTag : IComponentData { }
public struct PlayerSubBulletTag : IComponentData { }

public struct PlayerBulletDamage : IComponentData
{
    public float Value;
}

#endregion

#region Enemy
public struct EnemyBulletTag : IComponentData { }



#endregion

#region Danmak Pattern

public struct BulletAimingTag : IComponentData { }

#endregion