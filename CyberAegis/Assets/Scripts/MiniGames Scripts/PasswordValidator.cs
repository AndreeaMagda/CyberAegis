using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PasswordValidator : MonoBehaviour
{
    public TMP_InputField passwordInput;
    public TMP_Text feedbackText;
    public TMP_InputField nameInput;
    public TMP_Text player;

    public string playerName="";

    void Start()
    {
        nameInput.onEndEdit.AddListener(SetPlayerName);
        passwordInput.onValueChanged.AddListener(ValidatePassword);
    }

    public void SetPlayerName(string name)
    {
        nameInput.gameObject.SetActive(false);
        passwordInput.gameObject.SetActive(true);
        playerName = nameInput.text;
        Debug.Log("Player name set to: " + playerName);
    }

    public void OnApplicationQuit()
    {
       passwordInput.gameObject.SetActive(false);
    }

    public void ValidatePassword(string password)
    {
        nameInput.gameObject.SetActive(false);
        bool isValid = true;
        string feedback = "Password must:\n";

        if (password.Length < 8)
        {
            feedback += "- Be at least 8 characters\n";
            isValid = false;
        }
        if (!System.Text.RegularExpressions.Regex.IsMatch(password, "[A-Z]"))
        {
            feedback += "- Contain an uppercase letter\n";
            isValid = false;
        }
        if (!System.Text.RegularExpressions.Regex.IsMatch(password, "[a-z]"))
        {
            feedback += "- Contain a lowercase letter\n";
            isValid = false;
        }
        if (!System.Text.RegularExpressions.Regex.IsMatch(password, "[0-9]"))
        {
            feedback += "- Contain a number\n";
            isValid = false;
        }
        if (!System.Text.RegularExpressions.Regex.IsMatch(password, "[!@#$%^&*]"))
        {
            feedback += "- Contain a special character\n";
            isValid = false;
        }

        if (password.Contains(playerName))
        {
            feedback += "- Not contain your name\n";
            isValid = false;
        }

        feedbackText.text = isValid ? "Password is secure!" : feedback;
    }
}
