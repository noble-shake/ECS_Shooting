using Unity.Entities;
using UnityEngine;

public class PlayerCollisionObjectAutohring : MonoBehaviour
{
    public class Baker : Baker<PlayerCollisionObjectAutohring>
    {
        public override void Bake(PlayerCollisionObjectAutohring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<PlayerSlowAnimComp>(entity);
        }
    }
}