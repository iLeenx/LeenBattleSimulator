using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

// moves units toward their target
// AttackSystem handle damage

public partial struct MovementSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        foreach (var (transform, speed, target, attackRange)
                 in SystemAPI.Query<
                     RefRW<LocalTransform>,
                     RefRO<MoveSpeed>,
                     RefRO<Target>,
                     RefRO<AttackRange>>())
        {
            if (target.ValueRO.Entity == Entity.Null)
                continue;

            if (!SystemAPI.Exists(target.ValueRO.Entity))
                continue;

            var targetTransform =
                SystemAPI.GetComponent<LocalTransform>(target.ValueRO.Entity);

            float3 direction = math.normalize(
                targetTransform.Position - transform.ValueRW.Position
            );

            // read target position and move toward it
            float distance = math.distance(
                transform.ValueRW.Position,
                targetTransform.Position
            );

            // stop moving if inside attack range
            if (distance <= attackRange.ValueRO.Value)
                continue;

            transform.ValueRW.Position +=
                direction * speed.ValueRO.Value * deltaTime;
        }
    }
}
