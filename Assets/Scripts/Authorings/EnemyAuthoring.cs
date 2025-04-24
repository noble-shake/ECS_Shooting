using UnityEngine;
using Unity.Entities;

public class EnemyAuthoring : MonoBehaviour
{
    public GameObject bullet;
    public float HP;

    public class Baker : Baker<EnemyAuthoring>
    {
        public override void Bake(EnemyAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<EnemyTag>(entity);
            AddComponent<EnemyTestBullet>(entity, new EnemyTestBullet
            {
                BulletPrefab = GetEntity(authoring.bullet, TransformUsageFlags.Dynamic)
            });
            AddComponent<EnemyMaxHP>(entity, new EnemyMaxHP { Value = authoring.HP });

            AddBuffer<DamageBuffer>(entity);

            AddComponent<EnemyCurrentHP>(entity, new EnemyCurrentHP { Value = authoring.HP });

            AddComponent<DestroyEntityFlag>(entity);
            SetComponentEnabled<DestroyEntityFlag>(entity, false);
        }
    }
}
