using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int Score = 0;
    public static float SurvivalTime = 0;

    public void RestartGame()
    {
        Score = 0;
        SurvivalTime = 0;

        SceneManager.LoadScene("GameScene");
    }

    public void GoToTitle()
    {
        Score = 0;
        SurvivalTime = 0;

        SceneManager.LoadScene("TitleScene");
    }
}