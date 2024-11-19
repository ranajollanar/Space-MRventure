using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using System.Diagnostics;
using UnityEngine.SceneManagement;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.ComponentModel;

public class PlayFabRegister : MonoBehaviour

{
    public SceneController sceneController;
    //register
    public TMP_Text mesgText;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField nameInput;

    //login
    public TMP_InputField emailLogin;
    public TMP_InputField passwordLogin;
    public TMP_Text mesgTextLogin;

    public void RegisterButton()
    {
        if (passwordInput.text.Length < 6)
        {
            mesgText.text="Password too short!";
            return;
        }

        var request = new RegisterPlayFabUserRequest
        {
            DisplayName = nameInput.text,
            Email = emailInput.text,
            Password = passwordInput.text,
            RequireBothUsernameAndEmail = false
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        mesgText.text="Registered and logged in!";
        sceneController.SwitchScenes("HomeScene");
    }

    void OnError(PlayFabError error)
    {
        mesgText.text="Error: " + error.ErrorMessage;
        UnityEngine.Debug.Log(error.GenerateErrorReport());
    }

    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = emailLogin.text,
            Password = passwordLogin.text
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    void OnLoginSuccess(LoginResult result)
    {
        mesgTextLogin.text="Logged in!";
        UnityEngine.Debug.Log("Successful login/account create!");
        GetCharacters();
        sceneController.SwitchScenes("HomeScene");
    }

    void GetCharacters()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnCharactersDataReceived, OnError);
    }

    void OnCharactersDataReceived(GetUserDataResult result)
    {
        UnityEngine.Debug.Log("Received characters data!");
    }

    public void ResetPasswordButton()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailInput.text,
            TitleId = "7C596"
        };

        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
    }

    void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        mesgText.text="Password reset email sent!";
    }
}