using UnityEngine;
using Unity.Entities;

public class PlayerAutohring : MonoBehaviour
{
    public GameObject bullet;
    public class Baker : Baker<PlayerAutohring>
    {
        public override void Bake(PlayerAutohring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<PlayerTag>(entity);
            AddComponent<PlayerSpeed>(entity);
            AddComponent<PlayerNormalBullet>(entity, new PlayerNormalBullet
            {
                BulletPrefab = GetEntity(authoring.bullet, TransformUsageFlags.Dynamic)
            });
            AddComponent<PlayerDestroyTag>(entity);

            // Control
            AddComponent<InputState>(entity);
            AddComponent<PlayerControllable>(entity);
            AddComponent<PlayerShootable>(entity);
            AddComponent<PlayerInvulable>(entity);

            // Animation
            AddComponent<PlayerDirectionComp>(entity, new PlayerDirectionComp { Value = 1 });
            AddComponent<PlayerAnimationIndexComp>(entity);
            AddComponent<PlayerAppearEffectComponent>(entity);

            // Enabled
            SetComponentEnabled<PlayerControllable>(entity, false);
            SetComponentEnabled<PlayerAppearEffectComponent>(entity, false);
            SetComponentEnabled<PlayerShootable>(entity, false);
            SetComponentEnabled<PlayerDestroyTag>(entity, false);
            SetComponentEnabled<PlayerInvulable>(entity, false);

        }
    }
}
