using UnityEngine;
using Unity.Entities;

public class PlayerNormalBulletAuthoring : MonoBehaviour
{
    public float speed;
    public class Baker : Baker<PlayerNormalBulletAuthoring>
    {
        public override void Bake(PlayerNormalBulletAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<PlayerBulletTag>(entity);
            AddComponent<BulletTag>(entity);
            AddComponent<PlayerNormalBulletTag>(entity);
            AddComponent<DestroyEntityFlag>(entity);
            AddComponent<PlayerBulletSpeed>(entity, new PlayerBulletSpeed { Value = authoring.speed });
            AddComponent<BulletDamage>(entity, new BulletDamage { Value = 1.0f });
            AddComponent<BulletPenetratable>(entity, new BulletPenetratable { Value = 0 });
            SetComponentEnabled<DestroyEntityFlag>(entity, false);

            AddBuffer<PenetratedBuffer>(entity);
        }
    }
}

public class PlayerSubBulletAuthoring : MonoBehaviour
{
    public float speed;
    public GameObject bullet;
    public class Baker : Baker<PlayerSubBulletAuthoring>
    {
        public override void Bake(PlayerSubBulletAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<PlayerBulletTag>(entity);
            AddComponent<BulletTag>(entity);
            AddComponent<PlayerBulletSpeed>(entity, new PlayerBulletSpeed { Value = authoring.speed });
            AddComponent<BulletDamage>(entity, new BulletDamage { Value = 0.5f });
            AddComponent<BulletPenetratable>(entity, new BulletPenetratable { Value = 0 });
            AddBuffer<PenetratedBuffer>(entity);
            AddComponent<DestroyEntityFlag>(entity);
            SetComponentEnabled<DestroyEntityFlag>(entity, false);

        }
    }
}

