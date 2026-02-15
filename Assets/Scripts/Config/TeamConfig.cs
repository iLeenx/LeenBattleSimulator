using System.Collections.Generic;
using UnityEngine;

// scriptable object = config data (no code logic)
// each team contains 5 UnitData Units

[CreateAssetMenu]
public class TeamConfig : ScriptableObject
{
    public int TeamId;
    public List<UnitData> Units;
}
