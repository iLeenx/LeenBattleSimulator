using Unity.Entities;
using UnityEngine;

public partial class HPDisplaySystem : SystemBase
{
    protected override void OnUpdate()
    {
        foreach (var (hp, entity) in
                 SystemAPI.Query<RefRO<HP>>()
                          .WithEntityAccess()
                          .WithAll<HPDisplay>())
        {
            var display =
                EntityManager.GetComponentObject<HPDisplay>(entity);

            display.hpText.text =
                Mathf.CeilToInt(hp.ValueRO.Value).ToString();
        }
    }
}
