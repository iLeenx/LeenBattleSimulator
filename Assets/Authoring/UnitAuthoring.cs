using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class UnitAuthoring : MonoBehaviour
{
    [Header("Stats")]
    public int TeamId = 0;
    public float MaxHP = 100f;
    public float Attack = 10f;
    public float AttackSpeed = 1f; // attacks per second
    public float AttackRange = 1.5f;
    public float MoveSpeed = 2f;
    public int GridIndex = 0; // 0..8 for 3x3

    bool converted = false;

    void Awake()
    {
        if (!Application.isPlaying) return;
        if (converted) return;
        converted = true;
        ConvertToEntityAtRuntime();
    }

    void ConvertToEntityAtRuntime()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        if (world == null)
        {
            Debug.LogError("UnitAuthoring: Default world not found.");
            return;
        }

        var em = world.EntityManager;

        var archetype = em.CreateArchetype(
    typeof(TeamTag), typeof(HP), typeof(AttackStat), typeof(AttackSpeed),
    typeof(AttackRange), typeof(MoveSpeed), typeof(Target), typeof(AttackTimer),
    typeof(GridSlot), typeof(LocalTransform)
);

        Entity e = em.CreateEntity(archetype);

        em.SetComponentData(e, new TeamTag { Value = TeamId });
        em.SetComponentData(e, new HP { Value = MaxHP, Max = MaxHP });
        em.SetComponentData(e, new AttackStat { Value = Attack });
        em.SetComponentData(e, new AttackSpeed { Value = AttackSpeed });
        em.SetComponentData(e, new AttackRange { Value = AttackRange });
        em.SetComponentData(e, new MoveSpeed { Value = MoveSpeed });
        em.SetComponentData(e, new Target { Entity = Entity.Null });
        em.SetComponentData(e, new AttackTimer { Time = 0f });
        em.SetComponentData(e, new GridSlot { Index = GridIndex });

        float3 pos = transform.position;

        em.SetComponentData(e,
            LocalTransform.FromPosition(pos)
        );

        Destroy(gameObject);
    }
}