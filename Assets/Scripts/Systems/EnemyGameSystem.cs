using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;





public partial struct EnemyDamageProcessSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (CurHP, damage, entity) in SystemAPI.Query<RefRW<EnemyCurrentHP>, DynamicBuffer<DamageBuffer>>().WithPresent<DestroyEntityFlag>().WithEntityAccess())
        {
            if(damage.IsEmpty) continue;

            foreach(var dmg in damage)
            {
                CurHP.ValueRW.Value -= dmg.Value;
            }

            damage.Clear();

            if (CurHP.ValueRO.Value <= 0f)
            {
                SystemAPI.SetComponentEnabled<DestroyEntityFlag>(entity, true);
            }
        }
    }
}

[UpdateInGroup(typeof(PhysicsSystemGroup))]
[UpdateAfter(typeof(PhysicsSimulationGroup))]
[UpdateBefore(typeof(AfterPhysicsSystemGroup))]
public partial struct EnemyHitSystem : ISystem
{

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<SimulationSingleton>();
        state.RequireForUpdate<EnemyTag>();
        state.RequireForUpdate<PlayerBulletTag>();
    }

    public void OnUpdate(ref SystemState state)
    {

        var HitJob = new TriggerEnemyHitJob
        {
            TriggerEnemyGroup = SystemAPI.GetComponentLookup<EnemyTag>(true),
            TriggerBulletGroup = SystemAPI.GetComponentLookup<PlayerBulletTag>(true),
            TriggerBulletDamageGroup = SystemAPI.GetComponentLookup<BulletDamage>(true),
            EnemyDamageBufferGroup = SystemAPI.GetBufferLookup<DamageBuffer>(),
            TriggerBulletPenetrateBufferGroup = SystemAPI.GetBufferLookup<PenetratedBuffer>(),
        };

        var simulationSingleton = SystemAPI.GetSingleton<SimulationSingleton>();
        state.Dependency = HitJob.Schedule(simulationSingleton, state.Dependency);
    }

    [BurstCompile]
    struct TriggerEnemyHitJob : ITriggerEventsJob
    {

        [ReadOnly] public ComponentLookup<EnemyTag> TriggerEnemyGroup;
        [ReadOnly] public ComponentLookup<PlayerBulletTag> TriggerBulletGroup;
        [ReadOnly] public ComponentLookup<BulletDamage> TriggerBulletDamageGroup;
        public BufferLookup<DamageBuffer> EnemyDamageBufferGroup;
        public BufferLookup<PenetratedBuffer> TriggerBulletPenetrateBufferGroup;
        // public EntityCommandBuffer ecb;


        public void Execute(TriggerEvent triggerEvent)
        {

            Entity EnemyEntity;
            Entity PlayerBulletEntity;

            if (TriggerEnemyGroup.HasComponent(triggerEvent.EntityA) && TriggerBulletGroup.HasComponent(triggerEvent.EntityB))
            {
                EnemyEntity = triggerEvent.EntityA;
                PlayerBulletEntity = triggerEvent.EntityB;
            }
            else if (TriggerEnemyGroup.HasComponent(triggerEvent.EntityB) && TriggerBulletGroup.HasComponent(triggerEvent.EntityA))
            {
                EnemyEntity = triggerEvent.EntityB;
                PlayerBulletEntity = triggerEvent.EntityA;
            }
            else
            {
                return;
            }

            var TriggerPenetrateBuffer = TriggerBulletPenetrateBufferGroup[PlayerBulletEntity];

            if (!TriggerPenetrateBuffer.IsEmpty)
            {
                foreach (PenetratedBuffer buffElement in TriggerPenetrateBuffer)
                {
                    if (buffElement.alreadyEntity == EnemyEntity) return;
                }
            }

            var enemyDamageBuffer = EnemyDamageBufferGroup[EnemyEntity];
            float Damage = TriggerBulletDamageGroup[PlayerBulletEntity].Value;
            enemyDamageBuffer.Add(new DamageBuffer { Value = Damage });
            TriggerPenetrateBuffer.Add(new PenetratedBuffer { alreadyEntity = EnemyEntity });


        }
    }
}

[UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
[UpdateBefore(typeof(EndSimulationEntityCommandBufferSystem))]
public partial struct DestroyEnittySystem : ISystem
{

    public void OnUpdate(ref SystemState state)
    {
        var ecbSystem = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (_, entity) in SystemAPI.Query<DestroyEntityFlag>().WithEntityAccess())
        {
            ecb.DestroyEntity(entity);
        }

       
    }
}

