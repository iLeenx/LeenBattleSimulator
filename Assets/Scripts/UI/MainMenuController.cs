using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartBattle()
    {
        SceneManager.LoadScene("BattleScene");
    }
}
