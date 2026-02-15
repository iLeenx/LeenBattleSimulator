using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleUIController : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text winnerText;

    void Awake()
    {
        if (panel != null) panel.SetActive(false);
    }

    public void ShowWinner(int teamId)
    {
        if (panel != null) panel.SetActive(true);

        // team 0 = white/player, team 1 = red/enemy 
        if (winnerText != null)
            //winnerText.text = teamId == 0 ? "TEAM 0 WINS!" : "TEAM 1 WINS!";
            winnerText.text = $"Team {teamId} WINS!";

    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
