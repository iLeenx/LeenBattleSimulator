using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public partial struct WinSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        int team0 = 0;
        int team1 = 0;

        foreach (var team in SystemAPI.Query<RefRO<TeamTag>>())
        {
            if (team.ValueRO.Value == 0) team0++;
            if (team.ValueRO.Value == 1) team1++;
        }

        if (team0 > 0 && team1 == 0)
        {
            UnityEngine.Debug.Log("Team 0 Wins!");
        }
        else if (team1 > 0 && team0 == 0)
        {
            UnityEngine.Debug.Log("Team 1 Wins!");
        }
    }
}
