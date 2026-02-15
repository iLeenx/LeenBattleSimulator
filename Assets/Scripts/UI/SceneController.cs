using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void StartBattle()
    {
        SceneManager.LoadScene("Game 1");
    }
    public void BackToMain()
    {
        SceneManager.LoadScene("Main");
    }
}
