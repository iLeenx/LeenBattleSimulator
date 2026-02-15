using Unity.Entities;
using Unity.Collections;
using UnityEngine;

public partial class WinSystem : SystemBase
{
    bool gameEnded = false;

    public void ResetWin()
    {
        gameEnded = false;
    }

    protected override void OnUpdate()
    {
        if (gameEnded) return;

        var em = EntityManager;

        var query = GetEntityQuery(
            ComponentType.ReadOnly<TeamTag>(),
            ComponentType.Exclude<IsDead>()
        );

        using var units = query.ToEntityArray(Allocator.Temp);
        if (units.Length == 0) return;

        int survivingTeam = em.GetComponentData<TeamTag>(units[0]).Value;

        for (int i = 1; i < units.Length; i++)
        {
            int team = em.GetComponentData<TeamTag>(units[i]).Value;
            if (team != survivingTeam)
                return;
        }

        gameEnded = true;

        var ui = Object.FindFirstObjectByType<BattleUIController>();
        if (ui != null)
            ui.ShowWinner(survivingTeam);
    }
}
