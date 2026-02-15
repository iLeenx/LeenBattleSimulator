using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class HPWorldTextManager : MonoBehaviour
{
    [Header("Text look")]
    public TMP_FontAsset font;
    public float height = 2.0f;
    public float fontSize = 3.5f;   // 3D TMP units (not UI font size)
    public float scale = 0.2f;

    EntityManager em;
    EntityQuery query;

    readonly Dictionary<Entity, TextMeshPro> texts = new();

    void Start()
    {
        em = World.DefaultGameObjectInjectionWorld.EntityManager;

        query = em.CreateEntityQuery(
            ComponentType.ReadOnly<HP>(),
            ComponentType.ReadOnly<LocalTransform>()
        );
    }

    void LateUpdate()
    {
        if (em == null) return;

        using var entities = query.ToEntityArray(Allocator.Temp);

        // mark all as unseen, then remove leftovers
        var seen = new HashSet<Entity>();

        for (int i = 0; i < entities.Length; i++)
        {
            var e = entities[i];
            if (!em.Exists(e)) continue;

            seen.Add(e);

            if (!texts.TryGetValue(e, out var tmp) || tmp == null)
            {
                tmp = CreateTextObject();
                texts[e] = tmp;
            }

            var hp = em.GetComponentData<HP>(e);
            var lt = em.GetComponentData<LocalTransform>(e);

            tmp.text = Mathf.CeilToInt(hp.Value).ToString();

            float3 pos = lt.Position + new float3(0, height, 0);
            tmp.transform.position = new Vector3(pos.x, pos.y, pos.z);

            // face camera
            if (Camera.main != null)
            {
                tmp.transform.forward = Camera.main.transform.forward;
            }
        }

        // cleanup: remove texts for entities that no longer exist
        var toRemove = new List<Entity>();
        foreach (var kvp in texts)
        {
            if (!seen.Contains(kvp.Key))
            {
                if (kvp.Value != null) Destroy(kvp.Value.gameObject);
                toRemove.Add(kvp.Key);
            }
        }
        for (int i = 0; i < toRemove.Count; i++)
            texts.Remove(toRemove[i]);
    }

    TextMeshPro CreateTextObject()
    {
        var go = new GameObject("HP_Text");
        var tmp = go.AddComponent<TextMeshPro>();
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontSize = fontSize;
        tmp.enableAutoSizing = false;
        if (font != null) tmp.font = font;

        go.transform.localScale = Vector3.one * scale;
        return tmp;
    }
}
