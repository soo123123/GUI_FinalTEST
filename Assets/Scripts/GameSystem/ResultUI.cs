using TMPro;
using UnityEngine;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private TMP_Text survivalText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text finalScoreText;

    void Start()
    {
        int finalScore =
            GameManager.Score +
            Mathf.FloorToInt(GameManager.SurvivalTime * 10);

    survivalText.text =
        $"Survival Time : {GameManager.SurvivalTime:F0}";

    scoreText.text =
        $"Kill Score : {GameManager.Score}";

    finalScoreText.text =
        $"Final Score : {finalScore}";
        
    }
}