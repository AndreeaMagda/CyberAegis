using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TitleInputManager : MonoBehaviour
{
    public InputField inputField;
    public Text feedbackText;
    public Text timerText;
    public Text temporaryText;
    public GameObject feedbackObject;
    public Button restartButton; // Reference to the restart button
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
    }

    private void InitializeGame()
    {
        inputField.text = "";
        feedbackText.gameObject.SetActive(false);
        feedbackObject.SetActive(false);
        timerText.gameObject.SetActive(true);
        temporaryText.gameObject.SetActive(true);
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
        temporaryText.gameObject.SetActive(true);
    }

    private void OnTitleEntered(string enteredText)
    {
        if (!timerActive) return;

        // Modul de introducere a codului
        if (codeEntryMode)
        {
            if (enteredText == generatedCode)
            {
                DisplaySuccessMessage();
            }
            else
            {
                feedbackText.text = "Cod incorect. Reîncercați.";
                feedbackObject.SetActive(true);
                feedbackText.gameObject.SetActive(true);
            }
            return;
        }

        // Verifică titlul introdus
        if (enteredText.ToUpper() == correctTitles[currentTitleIndex])
        {
            currentTitleIndex++;
            inputField.text = "";
            feedbackObject.SetActive(false);

            // Dacă toate titlurile sunt introduse corect
            if (currentTitleIndex == correctTitles.Length)
            {
                timerActive = false;
                GenerateRandomCode();
                feedbackText.text = "Felicitări! Codul dvs. este " + generatedCode;
                feedbackObject.SetActive(true);
                feedbackText.gameObject.SetActive(true);
                inputField.placeholder.GetComponent<Text>().text = "Introduceți codul";
                inputField.text = "";
                codeEntryMode = true; // Trecem în modul de introducere a codului
                restartButton.gameObject.SetActive(true); // Afișează butonul de restart pe succes
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
    }

    private void DisplaySuccessMessage()
    {
        inputField.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        temporaryText.gameObject.SetActive(false);
        feedbackText.text = "AȚI REUȘIT!";
        feedbackObject.SetActive(true);
        feedbackText.gameObject.SetActive(true);
    }

    private void ResetInput(string message)
    {
        feedbackText.text = message;
        temporaryText.gameObject.SetActive(false);
        feedbackObject.SetActive(true);
        feedbackText.gameObject.SetActive(true);
        inputField.placeholder.GetComponent<Text>().text = "Introduceți titlul";
        inputField.text = "";
        currentTitleIndex = 0;
        ResetTimer();
        codeEntryMode = false;
        restartButton.gameObject.SetActive(true); // Show the restart button on incorrect entry
    }

    public void ResetGame()
    {
        // Reset all game elements and hide the restart button
        InitializeGame();
        restartButton.gameObject.SetActive(false);
    }
}
