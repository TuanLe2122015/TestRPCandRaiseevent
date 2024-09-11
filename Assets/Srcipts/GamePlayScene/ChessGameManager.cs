using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ChessGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject chessBoard;
    [SerializeField] Camera mainCam;
    // Start is called before the first frame update
    void Start()
    {
        if (PLayerManagerMine.Instance.GetUserType() < 10)
        {
            mainCam.transform.eulerAngles = new Vector3(
                mainCam.transform.eulerAngles.x,
                mainCam.transform.eulerAngles.y,
                mainCam.transform.eulerAngles.z + 180
            );
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);

    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    
}
