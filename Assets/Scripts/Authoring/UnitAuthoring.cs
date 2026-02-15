using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

// this is the transforms GameObject to Entity ECS
// this on the Unit prefab
// baker runs when the SubScene bakes and creates an entity with all the components

public class UnitAuthoring : MonoBehaviour
{
    // values becomes like a component in run 

    public int TeamId = 0;
    public float MaxHP = 100f;
    public float Attack = 10f;
    public float AttackSpeed = 1f;
    public float AttackRange = 1.5f;
    public float MoveSpeed = 2f;
    public int GridIndex = 0;
    public int UniqueId = 0;


    class Baker : Baker<UnitAuthoring>
    {
        public override void Bake(UnitAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            // add all gameplay data as components
            AddComponent(entity, new TeamTag { Value = authoring.TeamId });
            AddComponent(entity, new HP { Value = authoring.MaxHP, Max = authoring.MaxHP });
            AddComponent(entity, new AttackStat { Value = authoring.Attack });
            AddComponent(entity, new AttackSpeed { Value = authoring.AttackSpeed });
            AddComponent(entity, new AttackRange { Value = authoring.AttackRange });
            AddComponent(entity, new MoveSpeed { Value = authoring.MoveSpeed });
            AddComponent(entity, new Target { Entity = Entity.Null });
            AddComponent(entity, new AttackTimer { Time = 0f });
            AddComponent(entity, new GridSlot { Index = authoring.GridIndex });
            AddComponent(entity, new UnitId { Value = authoring.UniqueId });


            var display = authoring.GetComponent<HPDisplay>();
            if (display != null)
            {
                AddComponentObject(entity, display);
            }
        }
    }
}
