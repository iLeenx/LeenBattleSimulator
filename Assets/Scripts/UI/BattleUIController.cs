using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

// shows win panel in battle scene.
// WinSystem calls ShowWinner(teamTag).

public class BattleUIController : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text winnerText;

    void Start()
    {
        if (panel != null) panel.SetActive(false);

        var world = World.DefaultGameObjectInjectionWorld;
        if (world != null)
        {
            var winSystem = world.GetExistingSystemManaged<WinSystem>();
            if (winSystem != null)
                winSystem.ResetWin();
        }
    }

    public void ShowWinner(int winningTeamTag)
    {
        panel.SetActive(true);

        int displayTeamId;

        if (winningTeamTag == 0)
        {
            displayTeamId = 0; // player team id
        }
        else
        {
            displayTeamId = SelectedTeam.EnemyTeamId; // enemy id from config file
        }

        winnerText.text = $"Team {displayTeamId} WINS!";
    }



    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
