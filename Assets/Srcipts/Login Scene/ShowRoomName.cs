using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class ShowRoomName : MonoBehaviour
{
    public static ShowRoomName Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void ShowRooNameCur()
    {
        transform.GetComponent<Text>().text = "Room: " + PhotonNetwork.CurrentRoom.Name;
    }
}
