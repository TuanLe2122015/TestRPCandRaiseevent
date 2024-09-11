using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ShowPlayerJoinRoom : MonoBehaviour
{
    public static ShowPlayerJoinRoom Instance;
    byte slotIndex = 0;

    [SerializeField] GameObject Obj_Btn_StartGame;

    [SerializeField] GameObject SlotRedTeam;
    [SerializeField] GameObject SlotBlackTeam;

    string[] nickName = new string[20];
    string[] InGameName = new string[20];
    string stringNonePlayer = ".......";

    PhotonView PhotonViewShowPlayer;

    private void Awake()
    {

        Obj_Btn_StartGame.SetActive(false);
        Instance = this;

        // default setup
        for (int i = 0; i < 10; i ++)
        {
            InGameName[i] = stringNonePlayer;
            InGameName[i + 10] = stringNonePlayer;
            nickName[i] = stringNonePlayer;
            nickName[i + 10] = stringNonePlayer;
        }

        PhotonViewShowPlayer = transform.GetComponent<PhotonView>();


        // Intantiate index of button is slot in room
        for (byte i = 0; i < 20; i++)
        {
            if (i < 10)
            {
                GameObject SlotR = SlotRedTeam.transform.GetChild(i).gameObject;
                SlotR.AddComponent<SlotController>().indexOfSlot = i;
            }
            else
            {
                GameObject SlotR = SlotBlackTeam.transform.GetChild(i - 10).gameObject;
                SlotR.AddComponent<SlotController>().indexOfSlot = i;
            }
        }
    }
    public void OnJoinedRoom()
    {
        
        PhotonViewShowPlayer.RPC("RPC_PlaceToSlot_Master", RpcTarget.MasterClient,
            PhotonNetwork.NickName , PLayerManagerMine.Instance.InGameName);
        CheckToDisplayBtn_StartGame();

    }
    [PunRPC]
    private void CheckToDisplayBtn_StartGame()
    {
        // Test mode
        //Obj_Btn_StartGame.SetActive(true);
        //return;
        // test mode

        if (PhotonNetwork.IsMasterClient)
        {
            Obj_Btn_StartGame.SetActive(false);
            if (nickName[0] != stringNonePlayer && nickName[10] != stringNonePlayer)
            {
                Obj_Btn_StartGame.SetActive(true);
            }
        }
    }

    public void OnLeftRoom()
    {
        // default setup
        for (int i = 0; i < 10; i++)
        {
            InGameName[i] = stringNonePlayer;
            InGameName[i + 10] = stringNonePlayer;
            nickName[i] = stringNonePlayer;
            nickName[i + 10] = stringNonePlayer;
        }
        PhotonViewShowPlayer.RPC("RPC_ExitRoomSentToMaster", RpcTarget.MasterClient, 
            PhotonNetwork.NickName, PLayerManagerMine.Instance.InGameName);
    }

    // Auto place player to slot when join room
    [PunRPC]
    void RPC_PlaceToSlot_Master(string nickName, string InGameName)// Only for Master
    {
        for (byte i = 0; i < 20; i++)
        {
            if (this.InGameName[i] == stringNonePlayer) // // Only for Master
            {
                slotIndex = i;
                this.nickName[i] = nickName;
                this.InGameName[i] = InGameName;// Only for Master
                RefreshUIPlayersNameInRoom();
                break;
            }
        }
        PhotonViewShowPlayer.RPC("MasterSentToAllOther", RpcTarget.Others, this.nickName, this.InGameName);
    }
    [PunRPC]
    void MasterSentToAllOther(string [] nickName, string [] inGameName)
    {
        this.nickName = nickName;
        this.InGameName = inGameName;
        RefreshUIPlayersNameInRoom();
    }
    [PunRPC]
    void RPC_ExitRoomSentToMaster(string nickName, string inGameName)// Only for Master
    {
        for (int i = 0; i < 20; i++)// Only for Master
        {
            if (nickName == this.nickName[i])// Only for Master
            {
                this.nickName[i] = stringNonePlayer;// Only for Master
                this.InGameName[i] = stringNonePlayer;
                RefreshUIPlayersNameInRoom();
                break;
            }
        }
        PhotonViewShowPlayer.RPC("MasterSentToAllOther", RpcTarget.Others, this.nickName,this.InGameName);
    }
    void RefreshUIPlayersNameInRoom()
    {
        for (byte i = 0; i < 20; i++)
        {
            if (i < 10)
            {
                GameObject SlotR = SlotRedTeam.transform.GetChild(i).gameObject;
                SlotR.transform.GetChild(0).GetComponent<Text>().text = InGameName[i];
            }
            else
            {
                GameObject SlotR = SlotBlackTeam.transform.GetChild(i -10).gameObject;
                SlotR.transform.GetChild(0).GetComponent<Text>().text = InGameName[i];
            }
            if (nickName[i] == PhotonNetwork.NickName)
                slotIndex = i;
        }
    }

    public void OnClickSlotButton(byte indexSlotClick)
    {
        if (PhotonNetwork.IsMasterClient) // Neu la Master client thi` co' toan` quyen` doi~ cho~ voi' client khac'
        {
            if (indexSlotClick == slotIndex)// Master chuyen~ den' slot phai~ khac' slot dang o~
                return;
            string temp_1 = nickName[indexSlotClick];
            string temp_2 = InGameName[indexSlotClick];

            nickName[indexSlotClick] = nickName[slotIndex];
            InGameName[indexSlotClick] = InGameName[slotIndex];

            nickName[slotIndex] = temp_1;
            InGameName[slotIndex] = temp_2;

            slotIndex = indexSlotClick;
            // Master thông báo cho toàn player in room
            PhotonViewShowPlayer.RPC("MasterSentToAllOther", RpcTarget.Others, this.nickName, this.InGameName);
            //Debug.Log("Index of Slot :" + slotIndex);
            RefreshUIPlayersNameInRoom();
        }
        else
        {
            if (indexSlotClick == slotIndex|| nickName[indexSlotClick] != stringNonePlayer)// Client chi~ duoc phep doi~ cho~ den cho~ trong'
                return;
            string temp_1 = nickName[indexSlotClick];
            string temp_2 = InGameName[indexSlotClick];

            nickName[indexSlotClick] = nickName[slotIndex];
            InGameName[indexSlotClick] = InGameName[slotIndex];

            nickName[slotIndex] = temp_1;
            InGameName[slotIndex] = temp_2;

            
            // Thông báo cho Master in room
            PhotonViewShowPlayer.RPC("ClientSentToMaster", RpcTarget.MasterClient, slotIndex, indexSlotClick);
            slotIndex = indexSlotClick;
            //Debug.Log("Index of Slot :" + slotIndex);
            RefreshUIPlayersNameInRoom();
        }

        // Moi~ khi player doi~ cho~, kiem tra de~ setactive Start_Game button
        //CheckToDisplayBtn_StartGame();
        PhotonViewShowPlayer.RPC("CheckToDisplayBtn_StartGame", RpcTarget.AllBuffered, null);
    }

    [PunRPC]
    void ClientSentToMaster(byte indexClient, byte indexSlotClick) // Only sent for Master client
    {
        string temp_1 = nickName[indexSlotClick];
        string temp_2 = InGameName[indexSlotClick];

        nickName[indexSlotClick] = nickName[indexClient];
        InGameName[indexSlotClick] = InGameName[indexClient];

        nickName[indexClient] = temp_1;
        InGameName[indexClient] = temp_2;
        PhotonViewShowPlayer.RPC("MasterSentToAllOther", RpcTarget.Others, this.nickName, this.InGameName);
        RefreshUIPlayersNameInRoom();
    }
    public void Button_StartGame()
    {
        PhotonViewShowPlayer.RPC("PunRPC_UpdateInforAllPlayer", RpcTarget.All, null);
        PhotonNetwork.LoadLevel(1);
    }
    [PunRPC]
    private void PunRPC_UpdateInforAllPlayer()
    {
        PLayerManagerMine.Instance.SetUserType(slotIndex);
    }
  
}
