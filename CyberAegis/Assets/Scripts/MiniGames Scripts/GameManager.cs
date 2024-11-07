using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject introducere;     // Reference to the Introducere screen
    public GameObject game;            // Reference to the Game screen
    public GameObject feedbackObject;  // Reference to FeedbackObject
    public GameObject restartButton;   // Reference to RestartButton
    public GameObject booksText;       // Reference to BooksText
    public TitleInputManager titleInputManager;
    public GameObject instructionText;

    public void StartGame()
    {
        introducere.SetActive(false);       // Hide Introducere
        game.SetActive(true);               // Show Game
        feedbackObject.SetActive(false);    // Hide FeedbackObject
        restartButton.SetActive(false);     // Hide RestartButton initially
        booksText.SetActive(false);         // Hide BooksText initially
    }
}