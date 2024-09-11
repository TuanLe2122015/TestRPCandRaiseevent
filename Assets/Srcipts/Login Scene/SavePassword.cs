using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SavePassword : MonoBehaviour
{
    public static SavePassword Instance;

    [SerializeField] TMP_InputField  InputUserName;
    [SerializeField] TMP_InputField InputPassword;
    [SerializeField] TMP_InputField InputInGameName;
    [SerializeField] TMP_InputField InputRoomName;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        SavePassword.Instance.PushPasswordToScreen();
    }

    public void SavingPassword()
    {
        PlayerPrefs.SetString("ID", InputUserName.text);
        PlayerPrefs.SetString("Password", InputPassword.text);
    }

    public void PushPasswordToScreen()
    {
        InputUserName.text = PlayerPrefs.GetString("ID");
        InputPassword.text = PlayerPrefs.GetString("Password");
    }

    public void SavingInGameName()
    {
        PLayerManagerMine.Instance.InGameName = InputInGameName.text;
        PlayerPrefs.SetString("InGameName", InputInGameName.text);
        PlayerPrefs.SetString("RoomName", InputRoomName.text);
    }
    public void PushInGameName()
    {
        InputInGameName.text = PlayerPrefs.GetString("InGameName");
        InputRoomName.text = PlayerPrefs.GetString("RoomName");
    }
}
