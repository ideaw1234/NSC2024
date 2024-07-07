using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using PlayFab.ClientModels;
using PlayFab;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class LoginPlayfab : MonoBehaviour

{
    [SerializeField] TextMeshProUGUI MessageRegisterText;
    [SerializeField] TextMeshProUGUI MessageLoginText;

    [Header("Login")]
    [SerializeField] TMP_InputField EmailLogin_Input;
    [SerializeField] TMP_InputField PasswordLogin_Input;
    [SerializeField] GameObject LoginPage;

    [Header("Register")]
    [SerializeField] string username;
    [SerializeField] TMP_InputField Username_Input;
    [SerializeField] TMP_InputField EmailRegister_Input;
    [SerializeField] TMP_InputField PasswordRegister_Input;
    [SerializeField] GameObject RegisterPage;

    [Header("Forgot Password")]
    [SerializeField] TMP_InputField EmailForgotPassword_Input;

    #region Button Methods

    public void RegeisterButton()
    {
        if (string.IsNullOrEmpty(EmailRegister_Input.text) || string.IsNullOrEmpty(PasswordRegister_Input.text) || string.IsNullOrEmpty(Username_Input.text))
        {
            MessageRegisterText.text = "Please enter username ,  email and password.";
            return;
        }
        if (PasswordRegister_Input.text.Length < 6 || PasswordRegister_Input.text.Length > 100)
        {
            MessageRegisterText.text = "Password must be between 6 and 100 characters.";
            return;
        }

        var request = new RegisterPlayFabUserRequest
        {
            Email = EmailRegister_Input.text,
            Password = PasswordRegister_Input.text,
            Username = Username_Input.text,
            RequireBothUsernameAndEmail = false 
           
        };
        
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    public void LoginButton()
    {


        if (string.IsNullOrEmpty(EmailLogin_Input.text) || string.IsNullOrEmpty(PasswordLogin_Input.text))
        {
            MessageLoginText.text = "Please enter both email and password.";
            return;
        }

        if (!IsValidEmail(EmailLogin_Input.text))
        {
            MessageLoginText.text = "Please enter a valid email address";
            return;
        }

        if (PasswordLogin_Input.text.Length < 6 || PasswordLogin_Input.text.Length > 100)
        {
            MessageLoginText.text = "Password must be between 6 and 100 characters.";
            return;
        }

        var request = new LoginWithEmailAddressRequest
        {
            Email = EmailLogin_Input.text,
            Password = PasswordLogin_Input.text,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }


    public void ForgotPassword()
    {
        if (!IsValidEmail(EmailForgotPassword_Input.text))
        {
            MessageLoginText.text = "Please enter a valid email address";
            return;
        }

        var request = new SendAccountRecoveryEmailRequest
        {
            Email = EmailForgotPassword_Input.text,
            TitleId = PlayFabSettings.TitleId
        };
        Debug.Log(request);
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnForgotPasswordSuccess, OnError);
    }
    #endregion
    #region private Methods

    private void UpdateDisplayName(string displayName)
    {
        Debug.Log($"Updating Playfab account's Displayname to {displayName}");
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = displayName
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameSuccess, OnError);
    }

    private bool IsValidEmail(string email)
    {
        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailPattern);
    }
    #endregion
    #region Public Methods
    public void OpenLoginPage()
    {
        LoginPage.SetActive(true);
        RegisterPage.SetActive(false);
        Debug.Log("Login Page Opened");
    }

    public void OpenRegisterPage()
    {
        LoginPage.SetActive(false);
        RegisterPage.SetActive(true);
        Debug.Log("Register Page Opened");
    }

    //public void SetUsername(string name)
    //{
    //    username = name;
    //    PlayerPrefs.SetString("Username", username);
    //}

    #endregion
    #region Playfab Callbacks
    private void OnError(PlayFabError Error)
    {
        MessageRegisterText.text = Error.ErrorMessage;
        MessageLoginText.text = Error.ErrorMessage;

        if (Error.GenerateErrorReport() == "")
            Debug.Log("Error" + Error.GenerateErrorReport());
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Register Success");
        UpdateDisplayName(Username_Input.text);
        MessageRegisterText.text = "";
        OpenLoginPage();
    }

    private void OnLoginSuccess(LoginResult result)
    {
        string username_login = null;
        if (result.InfoResultPayload != null)
        {
            username_login = result.InfoResultPayload.PlayerProfile.DisplayName;
            PlayerPrefs.SetString("Username", username_login);
        }
        
        Debug.Log("Login Success");
        MessageLoginText.text = "";
        ScreenController.LoadScreen("Menu");

    }
    private void OnForgotPasswordSuccess(SendAccountRecoveryEmailResult result)
    {
        MessageLoginText.text = "Password Recovery Email Sent";
        Debug.Log("Password Recovery Email Sent");
    }
    private void OnDisplayNameSuccess(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log($"You have updated the displayname of the playfab account!");
    }
    #endregion


}
