using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TitleInputManager : MonoBehaviour
{
    public InputField inputField;
    public Text feedbackText;
    public Text timerText;
    public Text temporaryText;
    public float timerDuration = 120f;

    private string[] correctTitles = { "MAESTRUL INTERNETULUI", "PAROLA PIERDUTA", "SIGURANTA ONLINE", "CALATORIA CIBERNETICA" };
    private int currentTitleIndex = 0;
    private float timeRemaining;
    private bool timerActive = false;
    private string generatedCode;
    private bool codeEntryMode = false;

    void Start()
    {
        inputField.text = "";
        feedbackText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(true);
        temporaryText.gameObject.SetActive(true);
        timeRemaining = timerDuration;
        timerActive = true;
        inputField.onEndEdit.AddListener(OnTitleEntered);
        StartCoroutine(TimerCountdown());
        ShowTemporaryText();
       //s ShowTemporaryText("Introduceți titlul corect pentru a continua...");
    }

    private void ShowTemporaryText()
    {
      //  temporaryText.text = text;
        temporaryText.gameObject.SetActive(true);
    }

    private void OnTitleEntered(string enteredText)
    {
        if (!timerActive) return;

        if (codeEntryMode)
        {
            // Testul pentru codul introdus
            if (enteredText == generatedCode)
            {
                DisplaySuccessMessage();
            }
            else
            {
                feedbackText.text = "Cod incorect. Reîncercați.";
                feedbackText.gameObject.SetActive(true);
            }
            return;
        }

        if (enteredText.ToUpper() == correctTitles[currentTitleIndex])
        {
            currentTitleIndex++;
            inputField.text = "";

            if (currentTitleIndex == correctTitles.Length)
            {
                timerActive = false;
                GenerateRandomCode();
                feedbackText.text = "Felicitări! Codul dvs. este " + generatedCode;
                feedbackText.gameObject.SetActive(true);
                inputField.placeholder.GetComponent<Text>().text = "Introduceți codul"; 
                inputField.text = "";
                codeEntryMode = true;
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
        // Ascunde inputField-ul și timer-ul și afișează mesajul de succes
        inputField.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        temporaryText.gameObject.SetActive(false);// Ascunde textul temporar
        feedbackText.text = "AȚI REUȘIT!";
        feedbackText.gameObject.SetActive(true);
    }

    private void ResetInput(string message)
    {
        feedbackText.text = message;
        temporaryText.gameObject.SetActive(false);
        feedbackText.gameObject.SetActive(true);
        inputField.placeholder.GetComponent<Text>().text = "Introduceți titlul";
        inputField.text = "";
        currentTitleIndex = 0;
        timeRemaining = timerDuration;
        timerActive = true;
        codeEntryMode = false;
    }
}
