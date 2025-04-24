using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

#region Test

public partial struct TestMoveSystem : ISystem
{

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EnemyTag>();
    }

    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, _) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<EnemyTag>>())
        {
            transform.ValueRW.Position += new float3(0f, -2f * SystemAPI.Time.DeltaTime, 0f);
        }
    }
}

public partial struct TestShotSystem : ISystem
{
    public float CurDelay;
    public float ShotDelay;

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EnemyTag>();
        CurDelay = 2f;
        ShotDelay = 3f;
    }

    public void OnUpdate(ref SystemState state)
    {
        CurDelay -= SystemAPI.Time.DeltaTime;

        if (CurDelay < 0f) CurDelay = 0f;
        if (CurDelay > 0f) return;

        SystemAPI.TryGetSingletonEntity<PlayerTag>(out var Player);
        

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);

        foreach (var (trs, bullet, _, entity) in
         SystemAPI.Query<RefRO<LocalTransform>, RefRO<EnemyTestBullet>, RefRO<EnemyTag>>().WithEntityAccess())
        {
            
            var Rprefab = ecb.Instantiate(bullet.ValueRO.BulletPrefab);

            if (state.EntityManager.Exists(Player))
            {
                var ThisPos = trs.ValueRO.Position;
                var PlayerPos = SystemAPI.GetComponentRO<LocalTransform>(Player).ValueRO.Position;

                
                var gab = PlayerPos - ThisPos;
                var angle = math.atan2(gab.y, gab.x);
                var quat = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg);

                ecb.SetComponent(Rprefab, LocalTransform.FromPositionRotation(trs.ValueRO.Position, quat));
            }
            else
            {

                var defaultGab = -Vector3.up - Vector3.zero;
                var dangle = math.atan2(defaultGab.y, defaultGab.x);
                var dquat = Quaternion.Euler(0f, 0f, dangle * Mathf.Rad2Deg);

                ecb.SetComponent(Rprefab, LocalTransform.FromPositionRotation(trs.ValueRO.Position, dquat));
            }

        }
        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();

        CurDelay = ShotDelay;
    }
}

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct TestEnemyBulletSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EnemyBulletTag>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {

        var job = new RiseJob
        {
            deltaTime = SystemAPI.Time.DeltaTime,
        };

        state.Dependency = job.ScheduleParallel(state.Dependency);
    }

    [BurstCompile]
    partial struct RiseJob : IJobEntity
    {
        public float deltaTime;

        public void Execute(ref LocalTransform _trs, EnemyBulletTag _bulletTag)
        {
            _trs.Position += _trs.Right() * 10f * deltaTime;
        }
    }
}

#endregion