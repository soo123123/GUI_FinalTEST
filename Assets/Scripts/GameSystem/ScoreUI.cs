using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    void Update()
    {
        scoreText.text = $"Score : {GameManager.Score}";
    }
}