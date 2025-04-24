using UnityEngine;
using Unity.Entities;
using UnityEngine.UI;
using TMPro;

public class PlayerManagerAuthoring : MonoBehaviour
{
    public int Life;

    public class Baker : Baker<PlayerManagerAuthoring>
    {
        public override void Bake(PlayerManagerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new PlayerManagerComp
            {
                Life = authoring.Life,
            });
            AddComponent<GameObjectSync>(entity);
        }
    }
}



