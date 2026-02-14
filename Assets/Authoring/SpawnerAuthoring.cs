using Unity.Entities;
using UnityEngine;

public class SpawnerAuthoring : MonoBehaviour
{
    public GameObject UnitPrefab;
    public int UnitsPerTeam = 5;
    public float Spacing = 1.8f;
    public float TeamOffset = 4f;

    class Baker : Baker<SpawnerAuthoring>
    {
        public override void Bake(SpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new SpawnerData
            {
                UnitPrefab = GetEntity(authoring.UnitPrefab, TransformUsageFlags.Dynamic),
                UnitsPerTeam = authoring.UnitsPerTeam,
                Spacing = authoring.Spacing,
                TeamOffset = authoring.TeamOffset
            });
        }
    }
}
