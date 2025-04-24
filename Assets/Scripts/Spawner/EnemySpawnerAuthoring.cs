using UnityEngine;
using Unity.Entities;

public class EnemySpawnerAuthoring : MonoBehaviour
{
    public GameObject EnemyAPrefab;
    public GameObject EnemyBPrefab;
    public GameObject EnemyCPrefab;

    public class Baker : Baker<EnemySpawnerAuthoring>
    {
        public override void Bake(EnemySpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent<EnemySpawnerTag>(entity);
            AddComponent(entity, new EnemySpawnerPrefabComp
            {
                EnemyAEntity = GetEntity(authoring.EnemyAPrefab, TransformUsageFlags.Dynamic),
                EnemyBEntity = GetEntity(authoring.EnemyBPrefab, TransformUsageFlags.Dynamic),
                EnemyCEntity = GetEntity(authoring.EnemyCPrefab, TransformUsageFlags.Dynamic)
            });
            AddComponent<StageBeginComp>(entity);
            SetComponentEnabled<StageBeginComp>(entity, false);
        }
    }
}

