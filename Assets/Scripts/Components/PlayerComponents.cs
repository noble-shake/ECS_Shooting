using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;
using Unity.Rendering;

#region Spawner
public struct PlayerSpawnerComp : IComponentData
{
    public Entity PlayerEntity;
}

#endregion

#region Animation
[MaterialProperty("_FacingDirection")]
public struct PlayerDirectionComp : IComponentData
{
    public float Value;
}

[MaterialProperty("_AnimationIndex")]
public struct PlayerAnimationIndexComp : IComponentData
{
    public float Value;
}

public enum PlayerAnimationIndex : byte
{
    Right = 0,
    Left = 1,
    Idle = 2,
    None = byte.MaxValue,
}
#endregion

#region PlayerManager

public struct GameObjectSync : IComponentData
{ }

public struct PlayerManagerComp : IComponentData
{
    public int Life;
}

public class PlayerManaged : IComponentData
{
    public Button SpawnButton;
    public bool isSpawn;
    public int Life;
    public int Bomb;
    public int Power;
    public float Speed;

    public float SCREEN_WIDTH_MIN;
    public float SCREEN_WIDTH_MAX;
    public float SCREEN_HEIGHT_MIN;
    public float SCREEN_HEIGHT_MAX;

    // Every IComponentData class must have a no-arg constructor.
    public PlayerManaged()
    {
    }
}

#endregion

public struct PlayerTag : IComponentData { }

public struct PlayerSpeed : IComponentData
{
    public bool isSlow;
    public float speed;
}

public struct PlayerPower : IComponentData
{
    public float Value;
}

public struct PlayerBomb : IComponentData
{
    public int Value;
}

public struct PlayerNormalBullet : IComponentData
{
    public Entity BulletPrefab;
}

public struct PlayerShootable : IComponentData, IEnableableComponent
{ 
}

public struct PlayerControllable : IComponentData ,IEnableableComponent { }

public struct InputState : IComponentData
{
    public float Horizontal;
    public float Vertical;
    public float MouseX;
    public float MouseY;
    public bool Space;
    public bool Escape;
    public bool Shift;
    public bool Fire;
    public bool Bomb;
    public bool Charge;
    public bool ChargeUp;
}

public struct PlayerCollisionTag : IComponentData
{ }

[MaterialProperty("_SlowFactor")]
public struct PlayerSlowAnimComp : IComponentData
{
    public float Value;
}

public struct PlayerAppearEffectComponent : IEnableableComponent, IComponentData
{ }

public struct PlayerInvulable : IComponentData, IEnableableComponent { }

public struct PlayerDestroyTag : IComponentData, IEnableableComponent { }