using UnityEngine;
using Unity.Entities;

public class GameManagerAutohring : MonoBehaviour
{
    public float speed;
    public GameObject bullet;
    public class Baker : Baker<GameManagerAutohring>
    {
        public override void Bake(GameManagerAutohring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<PlayerTag>(entity);
            AddComponent<PlayerSpeed>(entity, new PlayerSpeed
            {
                speed = authoring.speed,
            });
            AddComponent<PlayerNormalBullet>(entity, new PlayerNormalBullet
            {
                BulletPrefab = GetEntity(authoring.bullet, TransformUsageFlags.Dynamic)
            });
            AddComponent<InputState>(entity);
            AddComponent<PlayerControllable>(entity);
            SetComponentEnabled<PlayerControllable>(entity, false);
        }
    }
}

