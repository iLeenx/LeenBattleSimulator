using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// builds the enemy team buttons from TeamConfig assets.
// clicking a button sets SelectedTeam (index + teamId label).

public class MainMenuTeamsUI : MonoBehaviour
{
    public List<TeamConfig> enemyTeams;
    public Transform content;
    public Button buttonPrefab;
    public TMP_Text selectedText;

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
