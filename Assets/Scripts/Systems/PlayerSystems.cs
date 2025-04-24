using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;


[UpdateAfter(typeof(PlayerSpawnSystem))]
public partial struct AppearSystem : ISystem
{
    public float CurTime;
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerAppearEffectComponent>();
        CurTime = 0f;
    }

    public void OnUpdate(ref SystemState state) 
    {
        foreach (var controllable in
 SystemAPI.Query<EnabledRefRW<PlayerAppearEffectComponent>>()
     .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState))
        {
            if (!controllable.ValueRO) return;
        }

        float3 OriginPos = new float3(0f, -11f, 0f);
        float3 DestinationPos = new float3(0f, -8f, 0f);
        foreach (var (transform, _) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<PlayerTag>>())
        {

            transform.ValueRW.Position = math.lerp(OriginPos, DestinationPos, CurTime);

        }

        CurTime += (SystemAPI.Time.DeltaTime);
        if (CurTime > 1f)
        {
            CurTime = 0f;
            SystemAPI.TryGetSingletonEntity<PlayerTag>(out var player);
            SystemAPI.SetComponentEnabled<PlayerAppearEffectComponent>(player, false);
            SystemAPI.SetComponentEnabled<PlayerControllable>(player, true);
            SystemAPI.SetComponentEnabled<PlayerShootable>(player, true);
        }
    }
}

[UpdateInGroup(typeof(PhysicsSystemGroup))]
[UpdateAfter(typeof(PhysicsSimulationGroup))]
[UpdateBefore(typeof(AfterPhysicsSystemGroup))]
public partial struct PlayerHitSystem : ISystem
{
    public PlayerComponentHandles PlayerHandles;
    public EnemyBulletComponentHandles EnemyBulletHandles;

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<SimulationSingleton>();
        state.RequireForUpdate<EnemyBulletTag>();
        state.RequireForUpdate<PlayerTag>();
        PlayerHandles = new PlayerComponentHandles(ref state);
        EnemyBulletHandles = new EnemyBulletComponentHandles(ref state);

    }
    #region m_handles
    public struct PlayerComponentHandles
    {
        public ComponentLookup<PlayerTag> playerTag;
        public ComponentLookup<PlayerInvulable> playerInvullableFlag;
        public ComponentLookup<PlayerDestroyTag> playerDestroyFlag;

        public PlayerComponentHandles(ref SystemState state)
        {
            playerTag = state.GetComponentLookup<PlayerTag>(true);
            playerInvullableFlag = state.GetComponentLookup<PlayerInvulable>();
            playerDestroyFlag = state.GetComponentLookup<PlayerDestroyTag>();
        }

        public void Update(ref SystemState state)
        {
            playerTag.Update(ref state);
            playerInvullableFlag.Update(ref state);
            playerDestroyFlag.Update(ref state);
        }
    }

    public struct EnemyBulletComponentHandles
    {
        public ComponentLookup<EnemyBulletTag> bulletTag;
        public ComponentLookup<DestroyEntityFlag> bulletDestroyFlag;

        public EnemyBulletComponentHandles(ref SystemState state)
        { 
            bulletTag = state.GetComponentLookup<EnemyBulletTag>(true);
            bulletDestroyFlag = state.GetComponentLookup<DestroyEntityFlag>();
        }

        public void Update(ref SystemState state) 
        {
            bulletTag.Update(ref state);
            bulletDestroyFlag.Update(ref state);
        }
    }
    #endregion
    public void OnUpdate(ref SystemState state)
    {
        PlayerHandles.Update(ref state);
        EnemyBulletHandles.Update(ref state);

        var HitJob = new TriggerPlayerHitJob
        {
            Player = PlayerHandles.playerTag,
            PlayerInvisible = PlayerHandles.playerInvullableFlag,
            PlayerDestroyFlag = PlayerHandles.playerDestroyFlag,
            TriggerBulletGroup = EnemyBulletHandles.bulletTag,
            TriggerBulletDestroyFlag = EnemyBulletHandles.bulletDestroyFlag,
        };

        var simulationSingleton = SystemAPI.GetSingleton<SimulationSingleton>();
        
        state.Dependency = HitJob.Schedule(simulationSingleton, state.Dependency);
        state.Dependency.Complete();
    }

    struct TriggerPlayerHitJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentLookup<PlayerTag> Player;
        public ComponentLookup<PlayerInvulable> PlayerInvisible;
        public ComponentLookup<PlayerDestroyTag> PlayerDestroyFlag;

        [ReadOnly] public ComponentLookup<EnemyBulletTag> TriggerBulletGroup;
        public ComponentLookup<DestroyEntityFlag> TriggerBulletDestroyFlag;


        public void Execute(TriggerEvent triggerEvent)
        {

            Entity PlayerEntity;
            Entity EnemyBulletEntity;

            if (Player.HasComponent(triggerEvent.EntityA) && TriggerBulletGroup.HasComponent(triggerEvent.EntityB))
            {
                PlayerEntity = triggerEvent.EntityA;
                EnemyBulletEntity = triggerEvent.EntityB;
            }
            else if (Player.HasComponent(triggerEvent.EntityB) && TriggerBulletGroup.HasComponent(triggerEvent.EntityA))
            {
                
                PlayerEntity = triggerEvent.EntityB;
                EnemyBulletEntity = triggerEvent.EntityA;
            }
            else
            {
                return;
            }

            var TriggeredPlayerInvulable = PlayerInvisible.IsComponentEnabled(PlayerEntity);
            if (TriggeredPlayerInvulable == false)
            {
                PlayerDestroyFlag.SetComponentEnabled(PlayerEntity, true);
            }

            TriggerBulletDestroyFlag.SetComponentEnabled(EnemyBulletEntity, true);
            
        }
    }
}

[UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
[UpdateBefore(typeof(EndSimulationEntityCommandBufferSystem))]
public partial struct DestroyPlayerSystem : ISystem
{

    public void OnUpdate(ref SystemState state)
    {
        //var ecbSystem = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        //var ecb = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged);

        //foreach (var (_, entity) in SystemAPI.Query<PlayerDestroyTag>().WithEntityAccess())
        //{
        //    ecb.DestroyEntity(entity);
        //}


    }
}
