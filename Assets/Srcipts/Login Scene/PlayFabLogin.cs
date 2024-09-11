using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class PlayFabLogin : MonoBehaviour
{
    [SerializeField] TMP_InputField MailLoginInput;
    [SerializeField] TMP_InputField PasswordLoginInput;

    [SerializeField] TextMeshProUGUI MessageText;

    public void Start()
    {
        UIControllerMine.Instance.OnLoginScrenn();
    }

    public void Button_PlayFab_RegisterAndLogin()
    {
        
        var request = new RegisterPlayFabUserRequest
        {
            DisplayName = MailLoginInput.text,
            Email = MailLoginInput.text,
            Password = PasswordLoginInput.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);
        Button_PlayFab_Login();
    }

    private void OnRegisterFailure(PlayFabError obj)
    {
        MessageText.text = obj.ErrorMessage;
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult obj)
    {   
        MessageText.text = "Register Success...";
    }

    public void Button_PlayFab_Login()
    {
        var request = new LoginWithEmailAddressRequest
        {

            Email = MailLoginInput.text,
            Password = PasswordLoginInput.text,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams()
            {
                GetPlayerProfile = true
            }
        };

        
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

    public void Button_PLaFab_Login_WithAnAndroidDevice()
    {
        var request = new LoginWithAndroidDeviceIDRequest 
        {

            AndroidDeviceId = ReturnAndroidID(),
          CreateAccount = true
        };
        PlayFabClientAPI.LoginWithAndroidDeviceID(request, OnAndroidLoginSuccess, OnLoginFailure);
    }

    private void OnAndroidLoginSuccess(LoginResult result)
    {
        //Debug.Log("Congratulations, you made your first successful API call!");
        MessageText.text = "Wellcome\n" + ReturnAndroidID().ToString();
        NetWorkManager.Instance.SetNickName(ReturnAndroidID().ToString());
        SavePassword.Instance.PushInGameName();
        NetWorkManager.Instance.OnJoinedLobby();
    }

    public static string ReturnAndroidID()
    {
        string deviceID = SystemInfo.deviceUniqueIdentifier;
        return deviceID;
    }

    private void OnLoginSuccess(LoginResult result)
    {
        //Debug.Log("Congratulations, you made your first successful API call!");
        MessageText.text =  "Wellcome\n" + result.InfoResultPayload.PlayerProfile.DisplayName;
        SavePassword.Instance.SavingPassword();
        NetWorkManager.Instance.SetNickName(result.InfoResultPayload.PlayerProfile.DisplayName);
        SavePassword.Instance.PushInGameName();
        NetWorkManager.Instance.OnJoinedLobby();
    }
    IEnumerator LoadNextScense()
    {
        yield return new WaitForSeconds(2);
        //MessageText.text = "Login in ...";
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    private void OnLoginFailure(PlayFabError error)
    {
        //Debug.LogWarning("Something went wrong with your first API call.  :(");
        //Debug.LogError("Here's some debug information:");
        //Debug.LogError(error.GenerateErrorReport());
        MessageText.text = error.GenerateErrorReport();
    }

    public void Button_PlayFab_RecoverAccount()
    {
        var request = new SendAccountRecoveryEmailRequest
        {

            Email = MailLoginInput.text,
            TitleId = "55FEE"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnRecoverSuccess, OnRecoverFailure);
    }

    private void OnRecoverFailure(PlayFabError obj)
    {
        //Debug.LogError(obj.GenerateErrorReport());
        MessageText.text = obj.GenerateErrorReport();   
    }

    private void OnRecoverSuccess(SendAccountRecoveryEmailResult obj)
    {
        MessageText.text = "Recover mail has sended...";
    }

}