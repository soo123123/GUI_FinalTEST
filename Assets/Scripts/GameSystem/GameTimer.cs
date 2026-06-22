using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    public static float ElapsedTime {get; private set;}
    private float elapsedTime;

    void Update()
    {
        elapsedTime += Time.deltaTime;
        ElapsedTime = elapsedTime;

        int minutes = (int)(elapsedTime / 60);
        int seconds = (int)(elapsedTime % 60);

        timeText.text = $"{minutes:00}:{seconds:00}";
    }
}