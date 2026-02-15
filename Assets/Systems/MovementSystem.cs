using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public partial struct MovementSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        foreach (var (transform, speed, team, target)
                 in SystemAPI.Query<
                     RefRW<LocalTransform>,
                     RefRO<MoveSpeed>,
                     RefRO<TeamTag>,
                     RefRO<Target>>())
        {
            // If no target, don't move
            if (target.ValueRO.Entity == Entity.Null)
                continue;

            // Make sure target still exists
            if (!SystemAPI.Exists(target.ValueRO.Entity))
                continue;

            var targetTransform = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.Entity);

            float3 direction = math.normalize(
                targetTransform.Position - transform.ValueRW.Position
            );

            transform.ValueRW.Position += direction * speed.ValueRO.Value * deltaTime;
        }
    }
}
