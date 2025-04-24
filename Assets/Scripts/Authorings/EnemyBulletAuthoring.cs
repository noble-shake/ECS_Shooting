using UnityEngine;
using Unity.Entities;

public class EnemyBulletAuthoring : MonoBehaviour
{
    public class Baker : Baker<EnemyBulletAuthoring>
    {
        public override void Bake(EnemyBulletAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<EnemyBulletTag>(entity);
            AddComponent<BulletAimingTag>(entity);
            AddComponent<BulletTag>(entity);
            AddComponent<DestroyEntityFlag>(entity);
            SetComponentEnabled<DestroyEntityFlag>(entity, false);

        }
    }
}

