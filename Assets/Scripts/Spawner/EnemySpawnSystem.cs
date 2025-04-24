using System;
using Unity.Entities;
using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial class EnemySpawnSystem : SystemBase
{

    public float TempDelay;

    protected override void OnCreate()
    {
        RequireForUpdate<EnemySpawnerTag>();
        //RequireForUpdate<StageBeginComp>();
        TempDelay = 3f;
    }

    protected override void OnUpdate()
    {
        TempDelay -= SystemAPI.Time.DeltaTime;
        if (TempDelay <= 0f) TempDelay = 0f;
        if (TempDelay > 0f) return;

        // if (!EntityManager.IsComponentEnabled<StageBeginComp>(spawnerEntity)) return;

        
        var ecbSystem = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSystem.CreateCommandBuffer(EntityManager.WorldUnmanaged);

        for (int idx= 0; idx < 10; idx++)
        {
            float3 spawnPos = new float3(UnityEngine.Random.Range(-10, 10), 11f, 0f);
            var prefab = ecb.Instantiate(SystemAPI.GetSingleton<EnemySpawnerPrefabComp>().EnemyAEntity);
            ecb.SetComponent(prefab, LocalTransform.FromPosition(spawnPos));
        }

        TempDelay = 3f;
    }
}


