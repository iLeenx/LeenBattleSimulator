using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TeamConfig : ScriptableObject
{
    public int TeamId;
    public List<UnitData> Units;
}
