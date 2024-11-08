using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText; // Reference to the UI text component displaying the score
    private int score = 0;

    private void Start()
    {
        UpdateScoreText();
    }

    // Method to increase the score and update the display
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
    }

    // Update the score text in the UI
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }
}
