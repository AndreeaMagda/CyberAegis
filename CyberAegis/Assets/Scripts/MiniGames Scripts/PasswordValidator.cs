using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;

public class PasswordValidator : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField passwordInput;
    public TMP_Text feedbackText;
    public TMP_InputField nameInput;

    private string playerName = "";

    private void Start()
    {
        nameInput.onEndEdit.AddListener(SetPlayerName);
        passwordInput.onValueChanged.AddListener(ValidatePassword);
    }

    private void SetPlayerName(string name)
    {
        playerName = nameInput.text;
        Debug.Log("Player name set to: " + playerName);
        nameInput.gameObject.SetActive(false);
        passwordInput.gameObject.SetActive(true);
    }

    private void ValidatePassword(string password)
    {
        var feedbackMessages = new List<string>();
        bool isValid = true;

        var validationChecks = new List<(bool condition, string message)>
        {
            (password.Length >= 8, "- Be at least 8 characters"),
            (Regex.IsMatch(password, "[A-Z]"), "- Contain an uppercase letter"),
            (Regex.IsMatch(password, "[a-z]"), "- Contain a lowercase letter"),
            (Regex.IsMatch(password, "[0-9]"), "- Contain a number"),
            (Regex.IsMatch(password, "[!@#$%^&*]"), "- Contain a special character"),
            (!password.Contains(playerName), "- Not contain your name")
        };

        foreach (var (condition, message) in validationChecks)
        {
            if (!condition)
            {
                feedbackMessages.Add(message);
                isValid = false;
            }
        }

        feedbackText.text = isValid ? "Password is secure!" : $"Password must:\n{string.Join("\n", feedbackMessages)}";
    }

    private void OnApplicationQuit()
    {
        passwordInput.gameObject.SetActive(false);
    }
}