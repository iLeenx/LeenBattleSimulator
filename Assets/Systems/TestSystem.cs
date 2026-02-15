using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[BurstCompile]
public partial struct TestSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        Debug.Log("TestSystem CREATED");
    }

    public void OnUpdate(ref SystemState state)
    {
        foreach (var team in SystemAPI.Query<RefRO<TeamTag>>())
        {
            Debug.Log("Unit team: " + team.ValueRO.Value);
        }
    }
}
