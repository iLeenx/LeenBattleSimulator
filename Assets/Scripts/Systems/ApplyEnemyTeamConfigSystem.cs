using Unity.Entities;
using Unity.Collections;
using UnityEngine;

public partial class ApplyEnemyTeamConfigSystem : SystemBase
{
    private bool applied = false;

    protected override void OnUpdate()
    {
        if (applied) return;

        // Find a provider in scene
        var provider = Object.FindFirstObjectByType<TeamConfigProvider>();
        if (provider == null || provider.enemyTeams == null || provider.enemyTeams.Count == 0)
        {
            // No provider yet, keep trying
            return;
        }

        int idx = Mathf.Clamp(SelectedTeam.EnemyTeamIndex, 0, provider.enemyTeams.Count - 1);
        var config = provider.enemyTeams[idx];
        if (config == null)
        {
            Debug.LogWarning("ApplyEnemyTeamConfigSystem: selected config is null.");
            return;
        }

        var em = EntityManager;

        // Query all units that have the required components
        var query = GetEntityQuery(
            ComponentType.ReadOnly<TeamTag>(),
            ComponentType.ReadWrite<HP>(),
            ComponentType.ReadWrite<AttackStat>(),
            ComponentType.ReadWrite<AttackSpeed>(),
            ComponentType.ReadWrite<AttackRange>(),
            ComponentType.ReadWrite<MoveSpeed>(),
            ComponentType.ReadOnly<GridSlot>()
        );

        using var entities = query.ToEntityArray(Allocator.Temp);

        // If entities aren't baked/ready yet, don't lock applied=true
        if (entities.Length == 0)
        {
            // Debug.Log("ApplyEnemyTeamConfigSystem: no entities yet, waiting...");
            return;
        }

        // If config has no unit entries, nothing to apply
        if (config.Units == null || config.Units.Count == 0)
        {
            Debug.LogWarning($"ApplyEnemyTeamConfigSystem: TeamId={config.TeamId} has no Units entries.");
            return;
        }

        bool anyApplied = false;

        // Optional: one-time header log per attempt
        Debug.Log($"ApplyEnemyTeamConfigSystem: trying apply TeamId={config.TeamId} (selected index={idx}), entities={entities.Length}, configUnits={config.Units.Count}");

        for (int i = 0; i < entities.Length; i++)
        {
            var e = entities[i];

            var team = em.GetComponentData<TeamTag>(e);
            if (team.Value != 1) continue; // apply ONLY to enemy team

            int grid = em.GetComponentData<GridSlot>(e).Index;

            // find matching UnitData by GridIndex
            UnitData data = null;
            for (int u = 0; u < config.Units.Count; u++)
            {
                if (config.Units[u] != null && config.Units[u].GridIndex == grid)
                {
                    data = config.Units[u];
                    break;
                }
            }

            if (data == null)
            {
                Debug.LogWarning($"ApplyEnemyTeamConfigSystem: NO MATCH for enemy grid={grid} in TeamId={config.TeamId}. Make sure config has UnitData with GridIndex {grid}.");
                continue;
            }

            // apply stats
            em.SetComponentData(e, new HP { Value = data.MaxHP, Max = data.MaxHP });
            em.SetComponentData(e, new AttackStat { Value = data.Attack });
            em.SetComponentData(e, new AttackSpeed { Value = data.AttackSpeed });
            em.SetComponentData(e, new AttackRange { Value = data.AttackRange });
            em.SetComponentData(e, new MoveSpeed { Value = data.MoveSpeed });

            anyApplied = true;
            Debug.Log($"ApplyEnemyTeamConfigSystem: applied HP={data.MaxHP} to enemy grid={grid} (TeamId preset {config.TeamId})");
        }

        // Only lock if we successfully applied to at least one enemy
        if (anyApplied)
        {
            applied = true;
            Debug.Log($"ApplyEnemyTeamConfigSystem: DONE applying TeamId={config.TeamId}");
        }
        else
        {
            // Keep trying next frame (maybe team entities not ready/matching yet)
            Debug.LogWarning("ApplyEnemyTeamConfigSystem: applied to 0 enemies this frame. Will retry.");
        }
    }

    // Call this from BattleUIController.Awake() if you want a hard reset per battle load
    public void ResetApply()
    {
        applied = false;
    }
}
