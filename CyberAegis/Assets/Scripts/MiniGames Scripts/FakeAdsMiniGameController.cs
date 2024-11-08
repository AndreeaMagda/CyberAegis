using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FakeAdsGameController : MonoBehaviour
{
    public List<Button> offerButtons;  // Lista de butoane pentru fiecare reclamă
    public List<string> explanations;  // Lista de explicații pentru fiecare imagine
    public List<string> feedbackMessages; // Lista de feedback pentru răspunsuri corecte/greșite

    public Text explanationText; // Textul pentru explicație
    public Text feedbackText;    // Textul pentru feedback
    public Button correctButton; // Butonul corect (Real)
    public Button incorrectButton; // Butonul greșit (Fals)
    public Button nextButton;    // Butonul pentru a trece la următoarea imagine

    private int currentOfferIndex = 0; // Indexul reclamei curente

    void Start()
    {
        InitializeOffers();
        DisplayOffer(currentOfferIndex); // Afișăm doar prima reclamă la început
        feedbackText.text = ""; // Ascunde feedback-ul inițial
        nextButton.gameObject.SetActive(false); // Ascunde butonul "Următorul" inițial

        // Asociază funcțiile butoanelor "Real" și "Fals"
        correctButton.onClick.AddListener(OnCorrectButtonPressed);
        incorrectButton.onClick.AddListener(OnIncorrectButtonPressed);
    }

    void InitializeOffers()
    {
        // Ascundem toate butoanele, cu excepția primului
        for (int i = 0; i < offerButtons.Count; i++)
        {
            offerButtons[i].gameObject.SetActive(i == 0);
        }
    }

    void DisplayOffer(int index)
    {
        offerButtons[index].gameObject.SetActive(true); // Activează butonul curent
        explanationText.text = explanations[index];     // Afișează explicația
        feedbackText.text = ""; // Resetare feedback
        nextButton.gameObject.SetActive(false); // Ascunde butonul "Următorul" până la feedback
    }

    public void OnCorrectButtonPressed()
    {
        feedbackText.text = feedbackMessages[currentOfferIndex]; // Feedback corect
        explanationText.text = explanations[currentOfferIndex];  // Afișează explicația
        nextButton.gameObject.SetActive(true); // Afișează butonul "Următorul"
    }

    public void OnIncorrectButtonPressed()
    {
        feedbackText.text = "Încercați din nou!"; // Feedback greșit
        explanationText.text = explanations[currentOfferIndex];  // Afișează explicația
        nextButton.gameObject.SetActive(true); // Afișează butonul "Următorul"
    }

    public void OnNextButtonPressed()
    {
        // Ascundem butonul curent
        offerButtons[currentOfferIndex].gameObject.SetActive(false);

        currentOfferIndex++;

        if (currentOfferIndex < offerButtons.Count)
        {
            // Afișăm următoarea reclamă
            DisplayOffer(currentOfferIndex);
        }
        else
        {
            Debug.Log("Sfârșitul jocului!");
            // Poți adăuga aici logica pentru a finaliza minijocul
        }
    }
}
