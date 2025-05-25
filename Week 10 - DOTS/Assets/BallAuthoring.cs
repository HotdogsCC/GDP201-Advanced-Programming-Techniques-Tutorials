using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class BallAuthoring : MonoBehaviour
{
    public float speed;
    public float3 direction;
    public float time;

    private class Baker : Baker<BallAuthoring>
    {
        public override void Bake(BallAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new BallData
            {
                speed = authoring.speed,
                direction = authoring.direction,
                time = authoring.time
            });
        }
    }
    
}
