using Unity.Entities;
using Unity.Collections;
using UnityEngine;

public partial class WinSystem : SystemBase
{
    bool gameEnded = false;

    protected override void OnUpdate()
    {
        if (gameEnded) return;

        var em = EntityManager;

        var query = GetEntityQuery(
            ComponentType.ReadOnly<TeamTag>(),
            ComponentType.Exclude<IsDead>()
        );

        using var entities = query.ToEntityArray(Allocator.Temp);

        if (entities.Length == 0) return;

        int survivingTeam = em.GetComponentData<TeamTag>(entities[0]).Value;

        for (int i = 1; i < entities.Length; i++)
        {
            int team = em.GetComponentData<TeamTag>(entities[i]).Value;
            if (team != survivingTeam)
                return;
        }

        gameEnded = true;
        Debug.Log($"TEAM {survivingTeam} WINS");
    }
}
