using Unity.Entities;
using Unity.Collections;
using UnityEngine;

public partial class ApplyEnemyTeamConfigSystem : SystemBase
{
    bool applied = false;

    protected override void OnUpdate()
    {
        if (applied) return;

        // Find a provider in scene
        var provider = Object.FindFirstObjectByType<TeamConfigProvider>();
        if (provider == null || provider.enemyTeams == null || provider.enemyTeams.Count == 0)
            return;

        int idx = Mathf.Clamp(SelectedTeam.EnemyTeamIndex, 0, provider.enemyTeams.Count - 1);
        var config = provider.enemyTeams[idx];
        if (config == null) return;

        var em = EntityManager;

        // Get all alive enemy units (team 1)
        var query = GetEntityQuery(typeof(TeamTag), typeof(HP), typeof(AttackStat), typeof(AttackSpeed), typeof(AttackRange), typeof(MoveSpeed), typeof(GridSlot));
        using var entities = query.ToEntityArray(Allocator.Temp);

        for (int i = 0; i < entities.Length; i++)
        {
            var e = entities[i];
            var team = em.GetComponentData<TeamTag>(e);
            if (team.Value != 1) continue;

            // find matching unit data by GridIndex
            int grid = em.GetComponentData<GridSlot>(e).Index;
            UnitData data = null;
            for (int u = 0; u < config.Units.Count; u++)
            {
                if (config.Units[u].GridIndex == grid)
                {
                    data = config.Units[u];
                    break;
                }
            }
            if (data == null) continue;

            // apply stats
            em.SetComponentData(e, new HP { Value = data.MaxHP, Max = data.MaxHP });
            em.SetComponentData(e, new AttackStat { Value = data.Attack });
            em.SetComponentData(e, new AttackSpeed { Value = data.AttackSpeed });
            em.SetComponentData(e, new AttackRange { Value = data.AttackRange });
            em.SetComponentData(e, new MoveSpeed { Value = data.MoveSpeed });
        }

        applied = true;
        Debug.Log($"Applied enemy config: TeamId={config.TeamId}");
    }
}
