using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;


[UpdateInGroup(typeof(LateSimulationSystemGroup))]
public partial struct BulletBorderCheckSystem : ISystem
{
    public float screen_width_min;
    public float screen_width_max;
    public float screen_height_min;
    public float screen_height_max;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BulletTag>();
        
    }

    public void OnUpdate(ref SystemState state)
    {

        var gameManager = SystemAPI.ManagedAPI.GetSingleton<PlayerManaged>();
        screen_width_min = gameManager.SCREEN_WIDTH_MIN - 2f;
        screen_width_max = gameManager.SCREEN_WIDTH_MAX + 2f;
        screen_height_min = gameManager.SCREEN_HEIGHT_MIN - 2f;
        screen_height_max = gameManager.SCREEN_HEIGHT_MAX + 2f;

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);

        foreach (var (trs, _, entity) in
                 SystemAPI.Query<RefRO<LocalTransform>, RefRO<BulletTag>>().WithEntityAccess())
        {
            if (trs.ValueRO.Position.x < screen_width_min || trs.ValueRO.Position.x > screen_width_max || trs.ValueRO.Position.y < screen_height_min || trs.ValueRO.Position.y > screen_height_max)
            {
                ecb.DestroyEntity(entity);
            }
        
        }

        ecb.Playback(state.EntityManager);
        state.Dependency.Complete();
        ecb.Dispose();

    }

}

public partial struct PlayerNormalBulletSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerNormalBulletTag>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var job = new RiseJob
        {
            deltaTime = SystemAPI.Time.DeltaTime,
        };
        
        state.Dependency = job.ScheduleParallel(state.Dependency);

        //foreach (var (trs , speed, _) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<PlayerBulletSpeed>, RefRO<PlayerNormalBulletTag>>())
        //{
        //    trs.ValueRW.Position += new float3(0f, speed.ValueRO.Value, 0f) * SystemAPI.Time.DeltaTime;
        //}
    }

    [BurstCompile]
    partial struct RiseJob : IJobEntity
    {
        public float deltaTime;
        public void Execute(ref LocalTransform _trs, ref PlayerBulletSpeed _speed, PlayerNormalBulletTag _bulletTag)
        {
            _trs.Position += new float3(0f, _speed.Value, 0f) * deltaTime;
        }
    }
}

[UpdateInGroup(typeof(PhysicsSystemGroup))]
[UpdateAfter(typeof(EnemyHitSystem))]
[UpdateBefore(typeof(AfterPhysicsSystemGroup))]
public partial struct PlayerPenetrateSystem: ISystem
{
    public void OnCreate(ref SystemState state) 
    {
        state.RequireForUpdate<PlayerBulletTag>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (_, penetrate, pBuffer, entity) in SystemAPI.Query<RefRO<PlayerBulletTag>, RefRO<BulletPenetratable>, DynamicBuffer<PenetratedBuffer>>().WithPresent<DestroyEntityFlag>().WithEntityAccess())
        {
            if (pBuffer.IsEmpty) continue;

            if (pBuffer.Capacity >= penetrate.ValueRO.Value)
            {
                pBuffer.Clear();
                SystemAPI.SetComponentEnabled<DestroyEntityFlag>(entity, true);
            }
        }
    }
}