using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;

// deals damage over time
// when HP reaches 0 IsDead

// ECB = Entity Command Buffer
// ECB is a safe way to change entities (add/remove/destroy components) without breaking ECS while it’s running

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial class AttackSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var em = EntityManager;
        float dt = (float)SystemAPI.Time.DeltaTime;

        // ECB to defer structural changes (adding IsDead)
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        // Query attackers that have the components we need
        var query = GetEntityQuery(
            ComponentType.ReadWrite<AttackTimer>(),
            ComponentType.ReadOnly<AttackStat>(),
            ComponentType.ReadOnly<AttackSpeed>(),
            ComponentType.ReadOnly<Target>()
        );

        using (var attackers = query.ToEntityArray(Allocator.Temp))
        {
            for (int i = 0; i < attackers.Length; i++)
            {
                var attacker = attackers[i];

                var timer = em.GetComponentData<AttackTimer>(attacker);
                var atk = em.GetComponentData<AttackStat>(attacker);
                var atkSpd = em.GetComponentData<AttackSpeed>(attacker);
                var target = em.GetComponentData<Target>(attacker);

                timer.Time += dt;

                if (target.Entity == Entity.Null)
                {
                    em.SetComponentData(attacker, timer);
                    continue;
                }

                if (!em.Exists(target.Entity))
                {
                    // target no longer exists -> clear and continue
                    timer.Time = 0f;
                    em.SetComponentData(attacker, timer);
                    continue;
                }

                float interval = 1f / math.max(0.0001f, atkSpd.Value);
                if (timer.Time < interval)
                {
                    em.SetComponentData(attacker, timer);
                    continue;
                }

                // attack
                timer.Time = 0f;
                em.SetComponentData(attacker, timer);

                if (em.HasComponent<HP>(target.Entity))
                {
                    var hp = em.GetComponentData<HP>(target.Entity);
                    hp.Value -= atk.Value;
                    em.SetComponentData(target.Entity, hp);

                    if (hp.Value <= 0f)
                    {
                        // mark dead via ECB (deferred structural change)
                        ecb.AddComponent<IsDead>(target.Entity, new IsDead { Value = true });
                    }
                }
            }
        }

        // Playback deferred structural changes and dispose ECB
        ecb.Playback(em);
        ecb.Dispose();
    }
}