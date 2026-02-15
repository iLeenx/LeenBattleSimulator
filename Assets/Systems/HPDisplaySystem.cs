using Unity.Entities;
using UnityEngine;

public partial class HPDisplaySystem : SystemBase
{
    protected override void OnUpdate()
    {
        var em = EntityManager;

        foreach (var (hp, entity) in
                 SystemAPI.Query<RefRO<HP>>().WithEntityAccess())
        {
            if (!em.HasComponent<HPDisplay>(entity))
                continue;

            var display = em.GetComponentObject<HPDisplay>(entity);
            if (display == null || display.hpText == null)
                continue;

            display.hpText.text = Mathf.CeilToInt(hp.ValueRO.Value).ToString();
        }
    }
}
