using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


[CreateAssetMenu]
public class TeamConfig : ScriptableObject
{
    public int TeamId;
    public List<UnitData> Units;
}
