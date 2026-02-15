using Unity.Entities;

// Keep ecs/components data only — no methods
// each struct is a component that lives on an entity
// systems read/write these to make the simulation work

public struct TeamTag : IComponentData { public int Value; }
public struct HP : IComponentData { public float Value; public float Max; }
public struct AttackStat : IComponentData { public float Value; }
public struct AttackSpeed : IComponentData { public float Value; }
public struct AttackRange : IComponentData { public float Value; }
public struct MoveSpeed : IComponentData { public float Value; }
public struct Target : IComponentData { public Entity Entity; }
public struct AttackTimer : IComponentData { public float Time; }
public struct GridSlot : IComponentData { public int Index; }
public struct IsDead : IComponentData { public bool Value; }