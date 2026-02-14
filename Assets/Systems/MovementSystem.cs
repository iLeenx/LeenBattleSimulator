using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial class MovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float dt = (float)SystemAPI.Time.DeltaTime;

        // Get a writable lookup (we will read/write LocalTransform for entities)
        var localTransformLookup = GetComponentLookup<LocalTransform>(false);
        localTransformLookup.Update(this);

        new MovementJob
        {
            Dt = dt,
            LocalTransformLookup = localTransformLookup
        }.Run();
    }

    partial struct MovementJob : IJobEntity
    {
        public float Dt;
        public ComponentLookup<LocalTransform> LocalTransformLookup;

        // Note: we take Entity instead of ref LocalTransform to avoid aliasing
        void Execute(Entity entity, in MoveSpeed ms, in AttackRange ar, in Target target)
        {
            if (target.Entity == Entity.Null) return;
            if (!LocalTransformLookup.HasComponent(target.Entity)) return;
            if (!LocalTransformLookup.HasComponent(entity)) return;

            var trans = LocalTransformLookup[entity];
            var targetTrans = LocalTransformLookup[target.Entity];

            float3 targetPos = targetTrans.Position;
            float3 dir = targetPos - trans.Position;
            float dist = math.length(dir);
            if (dist > ar.Value)
            {
                float3 moveDir = math.normalize(dir);
                float3 move = moveDir * ms.Value * Dt;
                float maxMove = dist - ar.Value;
                if (math.length(move) > maxMove) move = moveDir * maxMove;
                trans = LocalTransform.FromPosition(trans.Position + move);
                LocalTransformLookup[entity] = trans; // write back
            }
        }
    }
}