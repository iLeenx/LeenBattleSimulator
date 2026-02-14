using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Prefab (GameObject with UnitAuthoring)")]
    public GameObject unitGameObjectPrefab;

    [Header("Team settings")]
    public int unitsPerTeam = 5;
    public float gridSpacing = 1.8f;
    public float teamOffset = 4f; // distance between teams on X axis

    private Vector3[] gridPositions;

    void Awake()
    {
        PrepareGridPositions();
    }

    void PrepareGridPositions()
    {
        gridPositions = new Vector3[9];
        int idx = 0;
        for (int z = -1; z <= 1; z++)
        {
            for (int x = -1; x <= 1; x++)
            {
                gridPositions[idx++] = new Vector3(x * gridSpacing, 0f, z * gridSpacing);
            }
        }
    }

    void Start()
    {
        SpawnDefaultTeams();
    }

    public void SpawnDefaultTeams()
    {
        if (unitGameObjectPrefab == null)
        {
            Debug.LogError("Spawner: unitGameObjectPrefab is not assigned.");
            return;
        }

        SpawnTeam(0, new Vector3(-teamOffset, 0f, 0f));
        SpawnTeam(1, new Vector3(teamOffset, 0f, 0f));
    }

    void SpawnTeam(int teamId, Vector3 teamCenter)
    {
        int spawnCount = Mathf.Min(unitsPerTeam, gridPositions.Length);

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 worldPos = teamCenter + gridPositions[i];
            GameObject go = Instantiate(unitGameObjectPrefab, worldPos, Quaternion.identity);
            var authoring = go.GetComponent<UnitAuthoring>();
            if (authoring != null)
            {
                authoring.TeamId = teamId;
                authoring.GridIndex = i;
            }
        }

        Debug.Log($"Spawner: Spawned team {teamId} with {spawnCount} units at center {teamCenter}.");
    }
}