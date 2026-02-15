using Unity.Entities;

// Keep components data only — no methods

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