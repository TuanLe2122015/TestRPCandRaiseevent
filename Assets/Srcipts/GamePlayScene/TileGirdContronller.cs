//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
//using Photon.Pun;
//using Photon.Realtime;

public class TileGirdContronller : MonoBehaviour
{
    // Dung de luu vi tri o TileGird trong mang StatusBoardGird[,]
    private byte IndexInGird_Row;
    private byte IndexInGird_Column;
    
    GameObject board;

    GameObject TileAnchorGird;
    GameObject ChessOnTile = null;

    static float Anchor_Z;
    static float Anchor_X;

    //PhotonView photonView_TileGird;

    // Ham` tranh' viec Awake Tile duoc goi sau Awake Board
    public void AwakeOnBoard()
    {
        TileAnchorGird = GameObject.Find("AnchorTile_Corner_Board");
        board = GameObject.Find("Board");
        Anchor_Z = TileAnchorGird.transform.position.z;
        Anchor_X = TileAnchorGird.transform.position.x;

        IndexInGird_Row = (byte)((Anchor_Z - transform.position.z) / 2);// so voi Tile goc, thi cac tile con lai Z giam, X tang, moi tile cach nhau 1 .
        IndexInGird_Column = (byte)((transform.position.x - Anchor_X) / 2);
    }

    private void OnMouseUp()
    {
        //photonView_TileGird.RPC("PunRPC_OnMouseUp_TileGird", RpcTarget.All);
        OnMouseUp_TileGird();
        
    }
    
    private void OnMouseUp_TileGird()
    {
        board.GetComponent<BoardController>().WhenClickGirdTile(IndexInGird_Row, IndexInGird_Column, ChessOnTile, this.gameObject);
       
    }

    public byte GetIndexInGird_Row()
    {
        return IndexInGird_Row;
    }

    public byte GetIndexInGird_Column()
    {
        return IndexInGird_Column;
    }

    public bool SS_RC(byte R, byte C)
    {
        if (IndexInGird_Row != R) return false;
        if (IndexInGird_Column != C) return false;

        //Debug.Log("In Tile: " + IndexInGird_Row + " - " + IndexInGird_Column);
        return true;
    }

    public void SetChessOnTile( GameObject chessSet)
    {
        ChessOnTile = chessSet;
    }

    public GameObject GetChessOnTile()
    {
        return ChessOnTile;
    }
}
