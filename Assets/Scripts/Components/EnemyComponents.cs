using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Mathematics;
using System.Collections.Generic;

#region Spawner
public struct EnemySpawnerTag : IComponentData { }

public struct StageBeginComp : IComponentData, IEnableableComponent { }
public struct EnemySpawnPositionComp : IComponentData 
{
    public int2 Value;
}

public class ManagedEnemySpawnComponent : IComponentData
{
    public List<StageContainer> stageContainers;
    public ManagedEnemySpawnComponent() { }
}

public struct EnemySpawnerPrefabComp : IComponentData
{
    public Entity EnemyAEntity;
    public Entity EnemyBEntity;
    public Entity EnemyCEntity;
}

#endregion

#region Animation



#endregion


public struct EnemyTag : IComponentData { }

public struct EnemySpeed : IComponentData
{
    public float speed;
}

public struct EnemyTestBullet : IComponentData
{
    public Entity BulletPrefab;
}

public struct EnemyCurrentHP : IComponentData
{
    public float Value;
}

public struct EnemyMaxHP : IComponentData
{
    public float Value;
}

public struct EnemyInvulable : IComponentData, IEnableableComponent { }

