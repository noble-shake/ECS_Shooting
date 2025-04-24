using JetBrains.Annotations;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateAfter(typeof(InputSystem))]
public partial struct ControllerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<InputState>();
        state.RequireForUpdate<PlayerControllable>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var input = SystemAPI.GetSingleton<InputState>();

        foreach (var (transform, controller, pdir, entity) in
                 SystemAPI.Query<RefRW<LocalTransform>, RefRW<PlayerSpeed>, RefRW<PlayerDirectionComp>>().WithEntityAccess())
        {
            float speedValue = controller.ValueRO.speed;
            if (input.Shift)
            {
                speedValue /= 2f;
            }

            var move = new float3(input.Horizontal, input.Vertical, 0);
            move = move * speedValue * SystemAPI.Time.DeltaTime;
            move = math.mul(transform.ValueRO.Rotation, move);

            transform.ValueRW.Position += move;

            if (SystemAPI.HasComponent<PlayerTag>(entity))
            {

                var animationComp = SystemAPI.GetComponentRW<PlayerAnimationIndexComp>(entity);
                if (input.Horizontal < 0f)
                {
                    animationComp.ValueRW.Value = (float)PlayerAnimationIndex.Left;
                    // pdir.ValueRW.Value = -1f;
                }
                else if (input.Horizontal > 0f)
                {
                    animationComp.ValueRW.Value = (float)PlayerAnimationIndex.Right;
                    // pdir.ValueRW.Value = 1f;
                }
                else
                {
                    animationComp.ValueRW.Value = (float)PlayerAnimationIndex.Idle;
                }
            }

        }
    }
}

[UpdateAfter(typeof(ControllerSystem))]
public partial struct PlayerHitAppearSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<InputState>();
        state.RequireForUpdate<PlayerControllable>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var input = SystemAPI.GetSingleton<InputState>();

        foreach (var (mat, entity) in
         SystemAPI.Query<RefRW<PlayerSlowAnimComp>>().WithEntityAccess())
        {
            if (input.Shift)
            {
                mat.ValueRW.Value = 1;
            }
            else
            {
                mat.ValueRW.Value = 0;
            }

        }
    }
}

[UpdateAfter(typeof(ControllerSystem))]
public partial struct PlayerBorderCheckSystem : ISystem
{
    public float screen_width_min;
    public float screen_width_max;
    public float screen_height_min;
    public float screen_height_max;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<InputState>();
        state.RequireForUpdate<PlayerControllable>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var gameManager = SystemAPI.ManagedAPI.GetSingleton<PlayerManaged>();
        screen_width_min = gameManager.SCREEN_WIDTH_MIN;
        screen_width_max = gameManager.SCREEN_WIDTH_MAX;
        screen_height_min = gameManager.SCREEN_HEIGHT_MIN;
        screen_height_max = gameManager.SCREEN_HEIGHT_MAX;

        foreach (var (trs, _, entity) in
                 SystemAPI.Query<RefRW<LocalTransform>, RefRO<PlayerTag>>().WithEntityAccess())
        {
            if (trs.ValueRO.Position.x < screen_width_min)
            {
                trs.ValueRW.Position.x = screen_width_min;
            }
            
            if (trs.ValueRO.Position.x > screen_width_max)
            {
                trs.ValueRW.Position.x = screen_width_max;
            }
            
            if (trs.ValueRO.Position.y < screen_height_min)
            {
                trs.ValueRW.Position.y = screen_height_min;
            }
            
            if (trs.ValueRO.Position.y > screen_height_max)
            {
                trs.ValueRW.Position.y = screen_height_max;
            }

        }
    }

}


[UpdateAfter(typeof(InputSystem))]
public partial struct NormalShootSystem : ISystem
{
    public float CurDelay;
    public float ShotDelay;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<InputState>();
        state.RequireForUpdate<PlayerControllable>();

        CurDelay = 0f;
        ShotDelay = 0.12f;
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        CurDelay -= SystemAPI.Time.DeltaTime;

        if (CurDelay < 0f) CurDelay = 0f;
        if (CurDelay > 0f) return;

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
        var input = SystemAPI.GetSingleton<InputState>();

        foreach (var (trs, bullet, shootable, entity) in
         SystemAPI.Query<RefRO<LocalTransform>, RefRO<PlayerNormalBullet>, RefRO<PlayerShootable>>().WithEntityAccess())
        {
            if (state.EntityManager.IsComponentEnabled<PlayerShootable>(entity) == false) return;

            if (input.Fire)
            {
                CurDelay = ShotDelay;
                var Lprefab = ecb.Instantiate(bullet.ValueRO.BulletPrefab);
                var Rprefab = ecb.Instantiate(bullet.ValueRO.BulletPrefab);
                ecb.SetComponent(Lprefab, LocalTransform.FromPosition(trs.ValueRO.Position - new float3(0.35f, 0, 0)));
                ecb.SetComponent(Rprefab, LocalTransform.FromPosition(trs.ValueRO.Position + new float3(0.35f, 0, 0)));
                // ecb.SetComponent<LocalToWorld>(prefab, new LocalToWorld { Value = float4x4.TRS(trs.ValueRO.Position, Quaternion.identity, new float3(1f, 1f, 1f)) });
            }
        }
        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}


public partial struct GlobalTimeUpdateSystem : ISystem
{
    private static int _globalTimeShaderPropertyID;

    public void OnCreate(ref SystemState state)
    {
        _globalTimeShaderPropertyID = Shader.PropertyToID("_GlobalTime");
    }

    public void OnUpdate(ref SystemState state)
    {
        Shader.SetGlobalFloat(_globalTimeShaderPropertyID, (float)SystemAPI.Time.ElapsedTime);
    }
}