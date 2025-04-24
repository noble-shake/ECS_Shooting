using System;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;

#if !UNITY_DISABLE_MANAGED_COMPONENTS
public partial struct PlayerManagerSystem : ISystem
{
    public int Life;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        // We need to wait for the scene to load before Updating, so we must RequireForUpdate at
        // least one component type loaded from the scene.

        state.RequireForUpdate<GameObjectSync>();
    }

    public void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;

        var go = PlayerManager.Instance;
        if (go == null)
        {
            throw new Exception("Instance is null");
        }

        var directory = go.GetComponent<PlayerManager>();

        var directoryManaged = new PlayerManaged();
        directoryManaged.SpawnButton = directory.SpawnButton;
        directoryManaged.isSpawn = directory.isSpawn;
        directoryManaged.SpawnButton.onClick.AddListener(() => { directoryManaged.isSpawn = true; });
        directoryManaged.SCREEN_HEIGHT_MAX = directory.SCREEN_HEIGHT_MAX;
        directoryManaged.SCREEN_HEIGHT_MIN = directory.SCREEN_HEIGHT_MIN;
        directoryManaged.SCREEN_WIDTH_MIN = directory.SCREEN_WIDTH_MIN;
        directoryManaged.SCREEN_WIDTH_MAX = directory.SCREEN_WIDTH_MAX;
        directoryManaged.Life = directory.Life;
        directoryManaged.Bomb = directory.Life;
        directoryManaged.Power = directory.Power;
        directoryManaged.Speed = directory.Speed;

        var entity = state.EntityManager.CreateEntity();
        state.EntityManager.AddComponentData(entity, directoryManaged);
        state.EntityManager.AddComponent<GameManagerTag>(entity);
       
    }
}


#endif
