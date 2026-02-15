using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    //public void ShowWinner(int teamId)
    //{
    //    if (panel != null) panel.SetActive(true);

    //    // team 0 = white/player, team 1 = red/enemy 
    //    if (winnerText != null)
    //        //winnerText.text = teamId == 0 ? "TEAM 0 WINS!" : "TEAM 1 WINS!";
    //        winnerText.text = $"Team {teamId} WINS!";

    //}

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
            displayTeamId = SelectedTeam.EnemyTeamId; // enemy preset id from config
        }

        winnerText.text = $"Team {displayTeamId} WINS!";
    }



    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
