using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct SpawnSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<SpawnerData>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var em = state.EntityManager;

        foreach (var (spawner, entity) in
                 SystemAPI.Query<RefRO<SpawnerData>>().WithEntityAccess())
        {
            int gridSize = 3;
            int totalSlots = gridSize * gridSize;

            for (int team = 0; team < 2; team++)
            {
                for (int i = 0; i < spawner.ValueRO.UnitsPerTeam && i < totalSlots; i++)
                {
                    var instance = em.Instantiate(spawner.ValueRO.UnitPrefab);

                    int x = i % 3 - 1;
                    int z = i / 3 - 1;

                    float3 pos = new float3(
                        (team == 0 ? -spawner.ValueRO.TeamOffset : spawner.ValueRO.TeamOffset)
                        + x * spawner.ValueRO.Spacing,
                        0,
                        z * spawner.ValueRO.Spacing
                    );

                    em.SetComponentData(instance, LocalTransform.FromPosition(pos));
                    em.SetComponentData(instance, new TeamTag { Value = team });
                    em.SetComponentData(instance, new GridSlot { Index = i });
                }
            }

            em.DestroyEntity(entity);
        }
    }
}
