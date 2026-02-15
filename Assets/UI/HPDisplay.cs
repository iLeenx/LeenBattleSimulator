using TMPro;
using Unity.Entities;
using UnityEngine;
using Unity.Collections;

public class HPDisplay : MonoBehaviour
{
    public TMP_Text hpText;
    public int UniqueId;

    EntityManager em;
    EntityQuery query;

    void Start()
    {
        em = World.DefaultGameObjectInjectionWorld.EntityManager;
        query = em.CreateEntityQuery(typeof(UnitId), typeof(HP));
    }

    void Update()
    {
        if (hpText == null || em == null) return;

        using var entities = query.ToEntityArray(Allocator.Temp);

        if (hpText != null && hpText.text == "")
            hpText.text = "UI1";

        for (int i = 0; i < entities.Length; i++)
        {
            var e = entities[i];
            var id = em.GetComponentData<UnitId>(e);
            if (id.Value != UniqueId) continue;

            var hp = em.GetComponentData<HP>(e);
            hpText.text = Mathf.CeilToInt(hp.Value).ToString();
            return;
        }

        if (hpText != null && hpText.text == "")
            hpText.text = "UI2";
    }
}
