using System;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;
using Unity.Burst;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.RuleTile.TilingRuleOutput;
using static UnityEngine.GraphicsBuffer;

public partial struct PlayerSpawnSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        // We need to wait for the scene to load before Updating, so we must RequireForUpdate at
        // least one component type loaded from the scene.

        state.RequireForUpdate<PlayerSpawnerComp>();
    }
    public void OnUpdate(ref SystemState state)
    {
        var directory = SystemAPI.ManagedAPI.GetSingleton<PlayerManaged>();
        if (!directory.isSpawn)
        {
            return;
        }

        SystemAPI.TryGetSingletonEntity<PlayerTag>(out var playerEntity);
        if (state.EntityManager.Exists(playerEntity))
        {
            return;
        }


        float3 spawnPos = new float3(0f, -11f, 0f);
        var ecbSystem = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged);

        var prefab = ecb.Instantiate(SystemAPI.GetSingleton<PlayerSpawnerComp>().PlayerEntity);
        ecb.SetComponent(prefab, LocalTransform.FromPosition(spawnPos));
        ecb.SetComponent<PlayerSpeed>(prefab, new PlayerSpeed { speed = directory.Speed });
        ecb.SetComponentEnabled<PlayerAppearEffectComponent>(prefab, true);

    }
}
