using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public Button createAccountButton;

    [SerializeField] private GameObject errorPopup;
    [SerializeField] private Text errorPopupText;

    async void Start()
    {
        Debug.Log("Starting AuthManager initialization...");

        await InitializeUnityServicesAsync();

        if (createAccountButton != null)
        {
            createAccountButton.onClick.AddListener(OnCreateAccountButtonClicked);
        }
        else
        {
            Debug.Log ("CreateAccountButton is not assigned!");
        }

        if (errorPopup != null)
        {
            errorPopup.SetActive(false);
        }
        else
        {
            Debug.Log("ErrorPopup is not assigned!");
        }
    }


    async Task InitializeUnityServicesAsync()
    {
        try
        {
            await UnityServices.InitializeAsync();
            Debug.Log("Unity Services initialized successfully.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to initialize Unity Services: " + ex.Message);
        }
    }

    public async void OnCreateAccountButtonClicked()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (!IsPasswordValid(password))
        {
            ShowErrorPopup("Password must have at least 1 uppercase, 1 lowercase, 1 digit, 1 symbol, and be between 8 and 30 characters long.");
            return;
        }

        await SignUpWithUsernamePasswordAsync(username, password);
    }


    async Task SignUpWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            Debug.Log("SignUp is successful.");
            SceneManager.LoadScene("MainScene");
        }
        catch (AuthenticationException ex)
        {
            HandleAuthenticationError(ex);
        }
        catch (RequestFailedException ex)
        {
            HandleAuthenticationError(ex);
        }
    }

    private void HandleAuthenticationError(System.Exception ex)
    {
        Debug.LogException(ex);

        if (ex is RequestFailedException requestFailedException)
        {
            // Check if the error message contains specific text about the password requirements
            if (requestFailedException.Message.Contains("Password does not match requirements"))
            {
                ShowErrorPopup("Password must have at least 1 uppercase, 1 lowercase, 1 digit, 1 symbol, and be between 8 and 30 characters long.");
                return;
            }
        }

        // For any other errors, show a general error message
        ShowErrorPopup("An error occurred. Please try again.");
    }


    private void ShowErrorPopup(string message)
    {
        errorPopupText.text = message;
        errorPopup.SetActive(true);
        StartCoroutine(HideErrorPopupAfterDelay(3f)); // Hide after 3 seconds
    }

    private IEnumerator HideErrorPopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        errorPopup.SetActive(false);
    }

    private bool IsPasswordValid(string password)
    {
        if (password.Length < 8 || password.Length > 30)
            return false;
        if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"[A-Z]")) // Uppercase letter
            return false;
        if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"[a-z]")) // Lowercase letter
            return false;
        if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"\d")) // Digit
            return false;
        if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"[!@#$%^&*(),.?""\:{}|<>]")) // Symbol

            return false;

        return true;
    }


}
