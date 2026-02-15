using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

public class UnitAuthoring : MonoBehaviour
{
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


            //AddComponentObject(entity, authoring.GetComponent<HPDisplay>());

            var display = authoring.GetComponent<HPDisplay>();
            if (display != null)
            {
                AddComponentObject(entity, display);
            }
        }
    }
}
