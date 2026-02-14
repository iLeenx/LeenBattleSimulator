using Unity.Entities;

public struct SpawnerData : IComponentData
{
    public Entity UnitPrefab;
    public int UnitsPerTeam;
    public float Spacing;
    public float TeamOffset;
}
