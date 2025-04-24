using UnityEngine;
using Unity.Entities;

public class BossAuthoringToad : MonoBehaviour
{
    public GameObject bullet;
    public float HP;

    public class Baker : Baker<BossAuthoringToad>
    {
        public override void Bake(BossAuthoringToad authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
        }
    }
}
