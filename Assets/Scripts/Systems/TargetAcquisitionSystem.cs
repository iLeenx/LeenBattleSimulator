using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;

// picks targets - if target dies clear it and pick a new one

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial class TargetAcquisitionSystem : SystemBase
{
    protected override void OnUpdate()
    {
        // build lists of alive entities for each  team
        // then each attacker picks a random enemy from the other team

        // currently designed for 2 sides

        uint seed = (uint)SystemAPI.Time.ElapsedTime.GetHashCode() + 1;
        var rand = new Unity.Mathematics.Random(seed);

        var em = EntityManager;

        // Collect alive entities per team
        var teamQuery = GetEntityQuery(
            ComponentType.ReadOnly<TeamTag>(),
            ComponentType.Exclude<IsDead>()
        );

        var teamEntities = teamQuery.ToEntityArray(Allocator.Temp);
        var team0 = new NativeList<Entity>(Allocator.Temp);
        var team1 = new NativeList<Entity>(Allocator.Temp);

        for (int i = 0; i < teamEntities.Length; i++)
        {
            var e = teamEntities[i];
            var t = em.GetComponentData<TeamTag>(e);
            if (t.Value == 0) team0.Add(e);
            else team1.Add(e);
        }

        teamEntities.Dispose();

        // Query all attackers (units that can have targets)
        var attackerQuery = GetEntityQuery(
            ComponentType.ReadWrite<Target>(),
            ComponentType.ReadOnly<TeamTag>(),
            ComponentType.Exclude<IsDead>()
        );

        using (var attackers = attackerQuery.ToEntityArray(Allocator.Temp))
        {
            for (int i = 0; i < attackers.Length; i++)
            {
                var attacker = attackers[i];
                var target = em.GetComponentData<Target>(attacker);
                var team = em.GetComponentData<TeamTag>(attacker);

                // If current target is invalid or dead, clear it
                if (target.Entity != Entity.Null)
                {
                    if (!em.Exists(target.Entity) || em.HasComponent<IsDead>(target.Entity))
                    {
                        target.Entity = Entity.Null;
                    }
                }

                // Acquire a new target if needed
                if (target.Entity == Entity.Null)
                {
                    if (team.Value == 0 && team1.Length > 0)
                    {
                        int idx = rand.NextInt(0, team1.Length);
                        target.Entity = team1[idx];
                    }
                    else if (team.Value != 0 && team0.Length > 0)
                    {
                        int idx = rand.NextInt(0, team0.Length);
                        target.Entity = team0[idx];
                    }
                }

                em.SetComponentData(attacker, target);
            }
        }

        team0.Dispose();
        team1.Dispose();
    }
}