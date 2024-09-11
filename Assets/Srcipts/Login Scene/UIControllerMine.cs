using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIControllerMine : MonoBehaviour
{
    EventSystem system;

    public static UIControllerMine Instance;
    [SerializeField] GameObject canvasLoginScreen;
    [SerializeField] GameObject canvasLobbyscreen;
    [SerializeField] GameObject canvasRoomScreen;
    [SerializeField] GameObject btnStart;
    [SerializeField] GameObject TMP_Message;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
        system = EventSystem.current;
        
    }


    public void OnLoginScrenn()
    {
        canvasLoginScreen.SetActive(true) ;
        canvasLobbyscreen.SetActive(false);
        canvasRoomScreen.SetActive(false);
        TMP_Message.SetActive(true);
    }

    public void OnLobbyScreen()
    {
        canvasLoginScreen.SetActive(false);
        canvasLobbyscreen.SetActive(true);
        canvasRoomScreen.SetActive(false);
        TMP_Message.SetActive(true);
    }

    public void OnRoomScreen()
    {
        canvasLoginScreen.SetActive(false);
        canvasLobbyscreen.SetActive(false);
        canvasRoomScreen.SetActive(true);
        TMP_Message.SetActive(false);
    }

    public void OnEnoughtStartGame()
    {
        btnStart.SetActive(true);
    }

}
