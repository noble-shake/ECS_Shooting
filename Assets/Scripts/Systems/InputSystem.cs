using Unity.Burst;
using Unity.Entities;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine;

public partial struct InputSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerControllable>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var controllable in
         SystemAPI.Query<EnabledRefRW<PlayerControllable>>()
             .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState))
        {
            if (!controllable.ValueRO) return;
        }

        ref var inputState = ref SystemAPI.GetSingletonRW<InputState>().ValueRW;
        inputState.Horizontal = Input.GetAxisRaw("Horizontal");
        inputState.Vertical = Input.GetAxisRaw("Vertical");
        inputState.MouseX = Input.GetAxisRaw("Mouse X");
        inputState.MouseY = Input.GetAxisRaw("Mouse Y");
        inputState.Space = Input.GetKeyDown(KeyCode.Space);
        inputState.Escape = Input.GetKeyDown(KeyCode.Escape);
        inputState.Shift = Input.GetKey(KeyCode.LeftShift);
        inputState.Fire = Input.GetKey(KeyCode.Z);
        inputState.Bomb = Input.GetKeyDown(KeyCode.X);
        inputState.Charge = Input.GetKey(KeyCode.C);
        inputState.ChargeUp = Input.GetKeyUp(KeyCode.C);
    }
}