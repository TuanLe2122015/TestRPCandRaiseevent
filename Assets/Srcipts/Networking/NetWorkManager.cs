using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class NetWorkManager : MonoBehaviourPunCallbacks
{
    public static NetWorkManager Instance;

    #region Private Serializable Fields
    /// <summary>
    /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
    /// </summary>
    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    
    private byte maxPlayersPerRoom = 20;
    [SerializeField]
    TMP_InputField InGameNameInputField;
    [SerializeField]
    TMP_InputField roomNameInputField;

    //GameObject objBoxHandler;

    #endregion


    #region Private Fields


    /// <summary>
    /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
    /// </summary>
    string gameVersion = "1";


    #endregion


    #region MonoBehaviour CallBacks


    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    /// </summary>
    void Awake()
    {
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;

        Instance = this;
    }


    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    /// </summary>
    void Start()
    {
        Connect();
        DontDestroyOnLoad(gameObject);
    }


    #endregion


    #region Public Methods


    /// <summary>
    /// Start the connection process.
    /// - If already connected, we attempt joining a random room
    /// - if not yet connected, Connect this application instance to Photon Cloud Network
    /// </summary>
    public void Connect()
    {
        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
        {
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            //PhotonNetwork.JoinRandomRoom();
            Debug.Log("Connected to Photon sever");
        }
        else
        {
            //AppSettings chinaSettings = new AppSettings();
            //chinaSettings.UseNameServer = true;
            //chinaSettings.Server = "ns.photonengine.cn";
            //chinaSettings.AppIdRealtime = "ChinaPUNAppId"; // TODO: replace with your own PUN AppId unlocked for China region
            //chinaSettings.AppVersion = "ChinaAppVersion"; // optional
            //PhotonNetwork.ConnectUsingSettings(chinaSettings);
            // #Critical, we must first and foremost connect to Photon Online Server.

            PhotonNetwork.ConnectUsingSettings();
            //PhotonNetwork.ConnectToRegion("asia");
            PhotonNetwork.GameVersion = gameVersion;
        }
       // PhotonNetwork.JoinLobby();
        
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text) || string.IsNullOrEmpty(InGameNameInputField.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text, new RoomOptions { MaxPlayers = maxPlayersPerRoom },null,null);
        SavePassword.Instance.SavingInGameName();
    }

    public void FindRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text) || string.IsNullOrEmpty(InGameNameInputField.text))
        {
            return;
        }
        PhotonNetwork.JoinRoom(roomNameInputField.text);
        SavePassword.Instance.SavingInGameName();
    }

    public void JoinRandomRoom()
    {
        if (string.IsNullOrEmpty(InGameNameInputField.text))
        {
            return;
        }
        PhotonNetwork.JoinRandomRoom();
        SavePassword.Instance.SavingInGameName();
    }

    public void ExitRoom()
    {
        ShowPlayerJoinRoom.Instance.OnLeftRoom();
        PhotonNetwork.LeaveRoom();
    }

    public void SetNickName(string nickName)
    {
        PhotonNetwork.NickName = nickName;
    }

    #endregion

    #region MonoBehaviourPunCallbacks Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
        //PhotonNetwork.JoinRandomRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRoomFailed() was called by PUN : " + message);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        UIControllerMine.Instance.OnRoomScreen();
        ShowPlayerJoinRoom.Instance.OnJoinedRoom();
        ShowRoomName.Instance.ShowRooNameCur();
    }

    public override void OnLeftRoom()
    {
        UIControllerMine.Instance.OnLobbyScreen();
        Debug.Log("Left the room.");
    }   

    public override void OnJoinedLobby()
    {
        UIControllerMine.Instance.OnLobbyScreen();
        Debug.Log("On Joined Lobby");
    }

    //public override void OnLeftRoom()
    //{

    //}
    //public override void OnPlayerLeftRoom(Player otherPlayer)
    //{

    //}

    //public override void OnPlayerEnteredRoom(Player otherPlayer)
    //{



    //}


    #endregion
}
