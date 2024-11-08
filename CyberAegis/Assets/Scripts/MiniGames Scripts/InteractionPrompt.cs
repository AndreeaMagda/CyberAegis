using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractionPrompt : MonoBehaviour
{
    public GameObject promptPanel;
    public TMP_Text promptText;

    public Button friendButton;
    public Button strangerButton;

    private bool isFriend;
    private GameObject currentCharacter;
    private ScoreManager scoreManager;
    private CharacterSpawner characterSpawner

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        characterSpawner = FindObjectOfType<CharacterSpawner>();
        
        promptPanel.SetActive(false);

        friendButton.onClick.AddListener(() => HandleChoice(true));
        strangerButton.onClick.AddListener(() => HandleChoice(false));
    }

    // Called by the CharacterSpawner to show the prompt for a character
    public void ShowPrompt(bool characterIsFriend, GameObject character)
    {
        isFriend = characterIsFriend;
        currentCharacter = character;

        promptText.text = "Is this a friend or a stranger?";
        promptPanel.SetActive(true);
    }

    // Handle the player's choice
    private void HandleChoice(bool playerThinksFriend)
    {
        if (playerThinksFriend == isFriend)
        {
            Debug.Log("Correct! That was a " + (isFriend ? "friend." : "stranger."));
            scoreManager.AddScore(10);
        }
        else
        {
            Debug.Log("Incorrect. That was a " + (isFriend ? "friend." : "stranger.") + ".");
            scoreManager.AddScore(-5);
        }


        // Destroy the character after the choice
        if (currentCharacter != null)
        {
            Destroy(currentCharacter);
        }

        // Hide the prompt panel after a choice is made
        promptPanel.SetActive(false);

        if(characterSpawner != null)
        {
            characterSpawner.EnableSpawning();
        }
    }
}
