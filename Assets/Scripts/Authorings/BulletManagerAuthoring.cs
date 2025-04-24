using UnityEngine;
using Unity.Entities;

public class BulletManagerAuthoring : MonoBehaviour
{


    public class Baker : Baker<BulletManagerAuthoring>
    {
        public override void Bake(BulletManagerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);


        }
    }
}
