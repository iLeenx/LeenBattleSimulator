using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuTeamsUI : MonoBehaviour
{
    public List<TeamConfig> enemyTeams;      // drag your TeamConfig assets here
    public Transform content;               // ScrollView/Viewport/Content
    public Button buttonPrefab;             // a UI Button prefab
    public TMP_Text selectedText;           // optional label

    void Start()
    {
        BuildButtons();
        SelectTeam(0);
    }

    void BuildButtons()
    {
        for (int i = content.childCount - 1; i >= 0; i--)
            Destroy(content.GetChild(i).gameObject);

        for (int i = 0; i < enemyTeams.Count; i++)
        {
            int idx = i;
            var btn = Instantiate(buttonPrefab, content);

            // button label
            var label = btn.GetComponentInChildren<TMP_Text>();
            label.text = enemyTeams[idx] != null ? $"Team {enemyTeams[idx].TeamId}" : $"Team {idx + 1}";

            btn.onClick.AddListener(() => SelectTeam(idx));
        }
    }

    public void SelectTeam(int idx)
    {
        SelectedTeam.EnemyTeamIndex = Mathf.Clamp(idx, 0, enemyTeams.Count - 1);
        SelectedTeam.EnemyTeamId = enemyTeams[idx].TeamId;

        if (selectedText != null)
            selectedText.text = $"selected: team {enemyTeams[SelectedTeam.EnemyTeamIndex].TeamId}";
    }

    public void StartBattle()
    {
        SceneManager.LoadScene("BattleScene");
    }
}
