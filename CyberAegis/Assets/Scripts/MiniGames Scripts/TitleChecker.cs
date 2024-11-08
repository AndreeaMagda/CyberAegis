using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TitleInputManager : MonoBehaviour
{
    public InputField inputField;
    public Text feedbackText;
    public Text timerText;
    public Text booksText;
    public GameObject feedbackObject;
    public Button restartButton;
    public Button verifyCodeButton; // Button for verifying the code
    public float timerDuration = 120f;

    private string[] correctTitles = { "MAESTRUL INTERNETULUI", "PAROLA PIERDUTA", "SIGURANTA ONLINE", "CALATORIA CIBERNETICA" };
    private int currentTitleIndex = 0;
    private float timeRemaining;
    private bool timerActive = false;
    private string generatedCode;
    private bool codeEntryMode = false;

    void Start()
    {
        InitializeGame();
        restartButton.onClick.AddListener(ResetGame); // Assign Restart function to the button
        restartButton.gameObject.SetActive(false); // Initially hide the restart button

        verifyCodeButton.onClick.AddListener(OnCodeVerification); // Assign code verification to button
        verifyCodeButton.gameObject.SetActive(false); // Hide verify button until code entry mode
    }

    private void InitializeGame()
    {
        inputField.text = "";
        feedbackText.gameObject.SetActive(false);
        feedbackObject.SetActive(false);
        timerText.gameObject.SetActive(true);
        booksText.gameObject.SetActive(true);
        ResetTimer();
        inputField.onEndEdit.AddListener(OnTitleEntered);
        ShowTemporaryText();
    }

    public void ResetTimer()
    {
        StopAllCoroutines(); // Stop any existing coroutines to prevent multiple timers running simultaneously
        timeRemaining = timerDuration;
        timerActive = true;
        StartCoroutine(TimerCountdown());
    }

    private void ShowTemporaryText()
    {
        booksText.gameObject.SetActive(true);
    }

    public void OnTitleEntered(string enteredText)
    {
        enteredText = enteredText.Trim();
        Debug.Log("OnTitleEntered() called");
        Debug.Log("Entered Text: " + enteredText);
        Debug.Log("Generated Code: " + generatedCode);

        if (!timerActive) return;

        if (codeEntryMode)
        {
            // Do nothing in OnTitleEntered when in code entry mode, as code is verified by button
            return;
        }

        // Title entry mode
        if (enteredText.ToUpper() == correctTitles[currentTitleIndex])
        {
            currentTitleIndex++;
            inputField.text = "";
            feedbackObject.SetActive(false);

            if (currentTitleIndex == correctTitles.Length)
            {
                booksText.gameObject.SetActive(false);
                timerActive = false;
                GenerateRandomCode();

                // Show the generated code and switch to code entry mode
                feedbackText.text = "Felicitări! Codul dvs. este " + generatedCode;
                feedbackObject.SetActive(true);
                feedbackText.gameObject.SetActive(true);
                restartButton.gameObject.SetActive(false);

                // Change the placeholder text to prompt for code input
                inputField.placeholder.GetComponent<Text>().text = "Introduceți codul";
                inputField.text = "";

                // Activate code entry mode and show the verify button
                codeEntryMode = true;
                verifyCodeButton.gameObject.SetActive(true); // Show verify button for code entry
               // restartButton.gameObject.SetActive(true); // Show the restart button on success
            }
        }
        else
        {
            ResetInput("Titlu incorect! Reîncepe.");
        }
    }

    private IEnumerator TimerCountdown()
    {
        while (timerActive && timeRemaining > 0)
        {
            yield return new WaitForSeconds(1f);
            timeRemaining--;

            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);
            timerText.text = $"Timp rămas: {minutes:D2}:{seconds:D2}";

            if (timeRemaining <= 0)
            {
                ResetInput("Timpul a expirat! Reîncepe.");
            }
        }
    }

    private void GenerateRandomCode()
    {
        int randomNumber = Random.Range(10, 100);
        generatedCode = "CYB3R" + randomNumber.ToString();
        Debug.Log("Generated Code: " + generatedCode);
    }

    private void DisplaySuccessMessage()
    {
        inputField.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        booksText.gameObject.SetActive(false);
        feedbackText.text = "AȚI REUȘIT!";
        feedbackObject.SetActive(true);
        feedbackText.gameObject.SetActive(true);
    }

    private void ResetInput(string message)
    {
        feedbackText.text = message;
        booksText.gameObject.SetActive(false);
        feedbackObject.SetActive(true);
        feedbackText.gameObject.SetActive(true);
        inputField.placeholder.GetComponent<Text>().text = "Introduceți titlul";
        inputField.text = "";
        currentTitleIndex = 0;
        ResetTimer();

        restartButton.gameObject.SetActive(true); // Show the restart button on incorrect entry
    }

    public void ResetGame()
    {
        // Reset all game elements and hide the restart button
        InitializeGame();
        restartButton.gameObject.SetActive(false);
        verifyCodeButton.gameObject.SetActive(false); // Hide the verify button when restarting
    }

    // Method for code verification
    public void OnCodeVerification()
    {
        string enteredText = inputField.text.Trim();
        if (enteredText == generatedCode)
        {
            DisplaySuccessMessage();
            timerActive = false; // Stop the timer after successful entry
            inputField.interactable = false;
            verifyCodeButton.gameObject.SetActive(false); // Hide the verify button after successful entry
            restartButton.gameObject.SetActive(false); // Show the restart button after successful entry

        }
        else
        {
            feedbackText.text = "Cod incorect. Reîncercați.";
            feedbackObject.SetActive(true);
            feedbackText.gameObject.SetActive(true);
        }
    }
}
