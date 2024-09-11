using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BoardController : MonoBehaviour
{
    [SerializeField] GameObject ObjWarnSphere;

    // Data phai~ can` thiet cap nhat cho tat' ca~ client
    byte[,] StatusBoardGird = new byte[10, 9];
    TurnTeamPlayer TurnTeam = TurnTeamPlayer.Red_Team_1;

    PhotonView boardPV;
    public GameObject HandleChess;
    GameObject CurentChess;
    [SerializeField]
    GameObject boradGird;
    byte[] ArrayAvailableMove = new byte[34];// Quan Xe co the di chuyen 17 Tile
    byte[] Temp_ArrayAvailableMove = new byte[34];
    byte countMoveTile = 1;

    enum TurnTeamPlayer
    {
        Red_Team_1 = 0, Red_Team_2 = 1,
        Black_Team_1 = 10, Black_Team_2 = 11
    }

    //bool A_ChessBeChossen = false;

    enum QuanCoTuong
    {
        Red_Tuong = 0, Red_Sy_1 = 1, Red_Sy_2 = 2, Red_Voi_1 = 3, Red_Voi_2 = 4,
        Red_Xe_1 = 5, Red_Xe_2 = 6, Red_Phao_1 = 7, Red_Phao_2 = 8, Red_Ma_1 = 9, Red_Ma_2 = 10,
        Red_Tot_1 = 11, Red_Tot_2 = 12, Red_Tot_3 = 13, Red_Tot_4 = 14, Red_Tot_5 = 15,

        Black_Tuong = 20, Black_Sy_1 = 21, Black_Sy_2 = 22, Black_Voi_1 = 23, Black_Voi_2 = 24,
        Black_Xe_1 = 25, Black_Xe_2 = 26, Black_Phao_1 = 27, Black_Phao_2 = 28, Black_Ma_1 = 29, Black_Ma_2 = 30,
        Black_Tot_1 = 31, Black_Tot_2 = 32, Black_Tot_3 = 33, Black_Tot_4 = 34, Black_Tot_5 = 35
    };

    // Theo thu tu Tuong - 0 , Sy - 1.2, Voi - 3.4, Xe - 5.6, Phao - 7.8, Ma - 9.10, Tot - 11.12.13.14.15 qu�n ??, ???c ?�nh s? t? 0 cho ??n 15
    // Theo th? t? nh? v?y v?i qu�n ?en, ???c ?�nh s� t? 20 cho ??n 35
    // ........... Tuong - 20 , Sy - 21.22, Voi - 23.24, Xe - 25.26, Phao - 27.28, Ma - 29.30, Tot - 31.32.33.34.35
    // Start is called before the first frame update
    void Awake()
    {
        SetupStartGame();
        // Photon setup
        boardPV = GetComponent<PhotonView>();
    }

    void SetupStartGame()
    {
        
        //Debug.Log("Set up start game ......");
        for (int p = 0; p < boradGird.transform.childCount; p++)
        {
            // Neu TileGird co Row, Column =  i, j cua Ma Tran trang thai, thi dat Quan co len TileGird
            boradGird.transform.GetChild(p).GetComponent<TileGirdContronller>().AwakeOnBoard();

            // Create Warnning Sphere
            GameObject ObjWarn = Instantiate(ObjWarnSphere, boradGird.transform.GetChild(p).position, Quaternion.identity);
            ObjWarn.transform.SetParent(boradGird.transform.GetChild(p));
            ObjWarn.transform.position = new Vector3 ( ObjWarn.transform.position.x, ObjWarn.transform.position.y +1, ObjWarn.transform.position.z);
            ObjWarn.SetActive(false);
        }
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                StatusBoardGird[i, j] = 40;// la vi tri khong co quan co
            }
        }

        // C�i ??t v? tr� qu�n c? ban ??u l�c, m?i v�o game
        // T?i h�ng 0:
        StatusBoardGird[0, 0] = (byte)QuanCoTuong.Red_Xe_1;
        StatusBoardGird[0, 1] = (byte)QuanCoTuong.Red_Ma_1;
        StatusBoardGird[0, 2] = (byte)QuanCoTuong.Red_Voi_1;
        StatusBoardGird[0, 3] = (byte)QuanCoTuong.Red_Sy_1;
        StatusBoardGird[0, 4] = (byte)QuanCoTuong.Red_Tuong;
        StatusBoardGird[0, 5] = (byte)QuanCoTuong.Red_Sy_2;
        StatusBoardGird[0, 6] = (byte)QuanCoTuong.Red_Voi_2;
        StatusBoardGird[0, 7] = (byte)QuanCoTuong.Red_Ma_2;
        StatusBoardGird[0, 8] = (byte)QuanCoTuong.Red_Xe_2;
        // T?i h�ng 2:
        StatusBoardGird[2, 1] = (byte)QuanCoTuong.Red_Phao_1;
        StatusBoardGird[2, 7] = (byte)QuanCoTuong.Red_Phao_2;
        // T?i h�ng 3:
        StatusBoardGird[3, 0] = (byte)QuanCoTuong.Red_Tot_1;
        StatusBoardGird[3, 2] = (byte)QuanCoTuong.Red_Tot_2;
        StatusBoardGird[3, 4] = (byte)QuanCoTuong.Red_Tot_3;
        StatusBoardGird[3, 6] = (byte)QuanCoTuong.Red_Tot_4;
        StatusBoardGird[3, 8] = (byte)QuanCoTuong.Red_Tot_5;
        // T?i h�ng 9:
        StatusBoardGird[9, 0] = (byte)QuanCoTuong.Black_Xe_1;
        StatusBoardGird[9, 1] = (byte)QuanCoTuong.Black_Ma_1;
        StatusBoardGird[9, 2] = (byte)QuanCoTuong.Black_Voi_1;
        StatusBoardGird[9, 3] = (byte)QuanCoTuong.Black_Sy_1;
        StatusBoardGird[9, 4] = (byte)QuanCoTuong.Black_Tuong;
        StatusBoardGird[9, 5] = (byte)QuanCoTuong.Black_Sy_2;
        StatusBoardGird[9, 6] = (byte)QuanCoTuong.Black_Voi_2;
        StatusBoardGird[9, 7] = (byte)QuanCoTuong.Black_Ma_2;
        StatusBoardGird[9, 8] = (byte)QuanCoTuong.Black_Xe_2;
        // T?i h�ng 7:
        StatusBoardGird[7, 1] = (byte)QuanCoTuong.Black_Phao_1;
        StatusBoardGird[7, 7] = (byte)QuanCoTuong.Black_Phao_2;
        // T?i h�ng 6:
        StatusBoardGird[6, 0] = (byte)QuanCoTuong.Black_Tot_1;
        StatusBoardGird[6, 2] = (byte)QuanCoTuong.Black_Tot_2;
        StatusBoardGird[6, 4] = (byte)QuanCoTuong.Black_Tot_3;
        StatusBoardGird[6, 6] = (byte)QuanCoTuong.Black_Tot_4;
        StatusBoardGird[6, 8] = (byte)QuanCoTuong.Black_Tot_5;

        // Cai dat ValueChess cua tung` Quan do ( red chess )
        for (int i = 0; i < HandleChess.transform.GetChild(0).transform.childCount; i++)
        {
            HandleChess.transform.GetChild(0).transform.GetChild(i).GetComponent<ChessController>().SetValueChess((byte)(i));
        }
        // Cai dat ValueChess cua tung` Quan den ( black chess )
        for (int i = 0; i < HandleChess.transform.GetChild(0).transform.childCount; i++)
        {
            HandleChess.transform.GetChild(1).transform.GetChild(i).GetComponent<ChessController>().SetValueChess((byte)(i + 20));
        }
        // Cai dat vi tri ban dau cua Quan Co (GameObject o trong game) khi bat dau game
        SetPositionObjChessBaseOnMatrix();
        Off_AvailableArrow_Object();
    }

    private void SetPositionObjChessBaseOnMatrix()
    {
        // duyet Obj Quan co
        for (int n = 0; n < 2; n++)// duyet gameobject quan co
        {
            for (int m = 0; m < HandleChess.transform.GetChild(n).transform.childCount; m++)
            {
                HandleChess.transform.GetChild(n).transform.GetChild(m).gameObject.SetActive(false);
            }
        }
        for (int p = 0; p < boradGird.transform.childCount; p++)
        {
            boradGird.transform.GetChild(p).GetComponent<TileGirdContronller>().SetChessOnTile(null);
        }
        for (int i = 0; i < 10; i++)// duyet Ma Tran trang thai Quan co
        {
            for (int j = 0; j < 9; j++)
            {
                if (StatusBoardGird[i, j] != 40)// Bang trang thai khac o trong'
                {
                    for (int n = 0; n < 2; n++)// duyet gameObject Quan co
                    {
                        for (int m = 0; m < HandleChess.transform.GetChild(n).transform.childCount; m++)
                        {
                            GameObject chessSet = HandleChess.transform.GetChild(n).transform.GetChild(m).gameObject;
                            
                            if (chessSet.GetComponent<ChessController>().GetValueChess() == (StatusBoardGird[i, j]))
                            {
                                chessSet.SetActive(true);
                                // Dat vi tri Quan co`, khi bat dau` game
                                chessSet.GetComponent<ChessController>().SetPositon(i, j);
                                //Dat tham chieu' chess Piece len o^ co` ( tile gird )
                                for (int p = 0; p < boradGird.transform.childCount; p++)
                                {
                                    // Neu TileGird co Row, Column =  i, j cua Ma Tran trang thai, thi dat Quan co len TileGird
                                    if (boradGird.transform.GetChild(p).GetComponent<TileGirdContronller>().SS_RC((byte)i, (byte)j))
                                    {
                                        boradGird.transform.GetChild(p).GetComponent<TileGirdContronller>().SetChessOnTile(chessSet);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        //WarnWhenGeneralIsInDanger();
    }
    GameObject preTileClick = null;
    public void WhenClickGirdTile(byte Row, byte Column, GameObject chessOnTile, GameObject TileGirdOnClick)
    {
        //  ------------  Khi click tren mot TileGird --------------
        if (CurentChess == null)
        {
            // ------ Khi chua chon bat ky Quan co nao ---------
            if (!CheckChessIsTrueTurn(chessOnTile)) return;
            CurentChess = chessOnTile;
            preTileClick = TileGirdOnClick;
            // Cai` dat. AvailableMoveTile cua~ Quan co` duoc chon.
            Off_AvailableArrow_Object();
            SetAvailableMoveTile(Row, Column, chessOnTile);
        }
        else
        {
            bool isChangePieceInSameTeam = false;
            // ------- Khi da~ chon dc Quan co, thi` di chuyen~ quan co` --------
            if (TileClickOnArrayAvailableMove(TileGirdOnClick))// co' Quan co` da~ duoc chon truoc' do' va` tren Available MoveTile
            {
                preTileClick.GetComponent<TileGirdContronller>().SetChessOnTile(null);
                SetStatusMatrix(TileGirdOnClick, preTileClick, CurentChess);
                
                if (chessOnTile != null)// Neu' di chuyen den' Chess doi' thu~ (Oppoment chess) thi` xoa' Quan doi' thu~
                {
                    chessOnTile.gameObject.SetActive(false);
                }
            }else
            {
                if (chessOnTile != null) // Khi click chon quan khac trong cung` team
                {
                    if (CheckChessIsTrueTurn(chessOnTile))
                    {
                        CurentChess = chessOnTile;
                        preTileClick = TileGirdOnClick;
                        // Cai` dat. AvailableMoveTile cua~ Quan co` duoc chon.
                        Off_AvailableArrow_Object();
                        SetAvailableMoveTile(Row, Column, chessOnTile);
                        isChangePieceInSameTeam = true;
                    }
                }
            }
            if (isChangePieceInSameTeam == false)
            {
                Off_AvailableArrow_Object();
                preTileClick = null;
                CurentChess = null;
            }
        }
        WarnWhenGeneralIsInDanger();
        ChecKAndShowResultWhenWinGame();
    }
    private bool CheckChessIsTrueTurn(GameObject chessOnTile)
    {
        if (!chessOnTile) return false;
        byte chessValue = chessOnTile.GetComponent<ChessController>().GetValueChess();


        byte userSlot = PLayerManagerMine.Instance.GetUserType();
        // Dang la` luot cua~ red team ( TurnTeam = Red_Team_1 || Red_Team_2 )
        if (    (TurnTeam == TurnTeamPlayer.Red_Team_1 || TurnTeam == TurnTeamPlayer.Red_Team_2)
                &&
                (userSlot == (byte)TurnTeamPlayer.Red_Team_1 || userSlot == (byte)TurnTeamPlayer.Red_Team_2)
           )
        {
            if (chessValue < 20) // Quan co` red team co
            {
                return true;
            }
        }

        if ((TurnTeam == TurnTeamPlayer.Black_Team_1 || TurnTeam == TurnTeamPlayer.Black_Team_2)
                &&
                (userSlot == (byte)TurnTeamPlayer.Black_Team_1 || userSlot == (byte)TurnTeamPlayer.Black_Team_2)
           ) // Dang la` luot cua~ black team
        {
            if (chessValue > 20) // Quan co` black team
            {
                return true;
            }
        }

        return false;
    }

    private void ChangeTurnTeam()
    {
        boardPV.RPC("PunRPCs_ChangeTurnTeam", RpcTarget.All);
        
    }
    
    // Dung` de~ kiem tra co the~ di chuyen~ chess den' Tile dc click hay khong
    bool TileClickOnArrayAvailableMove(GameObject TileGirdOnClick)
    {
        byte RowToMove = TileGirdOnClick.GetComponent<TileGirdContronller>().GetIndexInGird_Row();
        byte ColumnToMove = TileGirdOnClick.GetComponent<TileGirdContronller>().GetIndexInGird_Column();
        for (byte i = 0; i < countMoveTile; i++)
        {
            if (ArrayAvailableMove[i * 2] == RowToMove && ArrayAvailableMove[i * 2 + 1] == ColumnToMove)
            {
                return true;
            }
        }
        return false;
    }

    // Lay' Array available move tile gan' vao` mang~
    void SetAvailableMoveTile(byte R, byte C, GameObject chessOnTile)
    {
        if (!chessOnTile) return;

        ArrayAvailableMove = chessOnTile.GetComponent<ChessController>().ArrayAvailableMoveTile(R, C, StatusBoardGird);
        countMoveTile = chessOnTile.GetComponent<ChessController>().GetCountTileAvalaible();

        On_AvailableArrow_Object();
    }

    // Dua. vao` Array move tile de~ hien. danh' dau' Tile co' the~ di chuyen tren Screen. ( show gameObject on Screen)
    void On_AvailableArrow_Object()
    {
        for (byte i = 0; i < countMoveTile; i++)
        {
            for (byte p = 0; p < boradGird.transform.childCount; p++)
            {
                // So sanh' Index Row, column TileGird voi' Array
                if (boradGird.transform.GetChild(p).GetComponent<TileGirdContronller>().SS_RC(ArrayAvailableMove[i * 2], ArrayAvailableMove[i * 2 + 1]))
                {
                    if(StatusBoardGird[ArrayAvailableMove[i * 2], ArrayAvailableMove[i * 2 + 1]] != 40)
                        boradGird.transform.GetChild(p).transform.GetChild(0).gameObject.SetActive(true);
                    else
                    {
                        boradGird.transform.GetChild(p).transform.GetChild(1).gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    // Trai' nguoc. voi' ham` On_AvailableArrow_Object, thi` SetActive nhung~ Object tren = false
    void Off_AvailableArrow_Object()
    {
        countMoveTile = 0;
        for (byte p = 0; p < boradGird.transform.childCount; p++)
        {
            boradGird.transform.GetChild(p).transform.GetChild(0).gameObject.SetActive(false);
            boradGird.transform.GetChild(p).transform.GetChild(1).gameObject.SetActive(false);
        }
    }
    // Di chuyen~ Object tren Screen
    void MoveChessToTile(GameObject tileBuyClick)
    {
        //Debug.Log("Move chess 1");
        //CurentChess.GetComponent<ChessController>().MoveToTileGird(tileBuyClick);
        tileBuyClick.GetComponent<TileGirdContronller>().SetChessOnTile(CurentChess);
    }

    // Thay doi~ ma tran trang. thai' khi di chuyen~
   
    void SetStatusMatrix(GameObject TileOnClick, GameObject preTile, GameObject curChess)
    {
        //ChangeTurnTeam();
        MoveChessToTile(TileOnClick);
        byte row = TileOnClick.GetComponent<TileGirdContronller>().GetIndexInGird_Row();
        byte column = TileOnClick.GetComponent<TileGirdContronller>().GetIndexInGird_Column();

        byte pre_Row = preTile.GetComponent<TileGirdContronller>().GetIndexInGird_Row();
        byte pre_Column = preTile.GetComponent<TileGirdContronller>().GetIndexInGird_Column();

        //StatusBoardGird[pre_Row, pre_Column] = 40;
        boardPV.RPC("PunRPCs_SynchronizeMatrix", RpcTarget.All, pre_Row, pre_Column, (byte)40);
        byte vlChess = curChess.GetComponent<ChessController>().GetValueChess();
        //StatusBoardGird[row, column] = vlChess;
        boardPV.RPC("PunRPCs_SynchronizeMatrix", RpcTarget.All, row, column, vlChess);
    }

    private byte CheckWinGame() // Team nao` Win thi` tra~ ve` Value chess cua~ Tuong' do', con` khong thi` tra~ ve` 40
    {
        bool hasRed = false;
        bool hasBlack = false;
        for (byte i = 0; i < 10; i ++)
        {
            for (byte j = 0; j < 9; j++)
            {
                if (StatusBoardGird[i,j] == (byte)QuanCoTuong.Red_Tuong)
                {
                    hasRed = true;
                }
                if (StatusBoardGird[i, j] == (byte)QuanCoTuong.Black_Tuong)
                {
                    hasBlack = true;
                }
            }
        }

        if (hasRed == false) // Khong co tuong' do~ tren ban`
            return (byte)QuanCoTuong.Black_Tuong;

        if (hasBlack ==false) // khong co' truong' den tren ban`
            return (byte)QuanCoTuong.Red_Tuong;


        return 40;
    }
    private void WarnWhenGeneralIsInDanger()
    {
        bool RedInDanger = false;
        bool BlackInDanger = false;
        for (int i = 0; i < boradGird.transform.childCount; i++)
        {
            GameObject chessCurOnTile = boradGird.transform.GetChild(i).GetComponent<TileGirdContronller>().GetChessOnTile();
            
            // Kiem tra tung` TileGird co' chess tren no'
            if (chessCurOnTile!=null)
            {
                byte R = boradGird.transform.GetChild(i).GetComponent<TileGirdContronller>().GetIndexInGird_Row();
                byte C = boradGird.transform.GetChild(i).GetComponent<TileGirdContronller>().GetIndexInGird_Column();
                Temp_ArrayAvailableMove = chessCurOnTile.GetComponent<ChessController>().ArrayAvailableMoveTile(R, C, StatusBoardGird);
                int  temp_countMoveTile = chessCurOnTile.GetComponent<ChessController>().GetCountTileAvalaible();
                byte vlChessCur = chessCurOnTile.GetComponent<ChessController>().GetValueChess();
                for (int n = 0; n < temp_countMoveTile; n++)
                {
                    if (StatusBoardGird[Temp_ArrayAvailableMove[n * 2], Temp_ArrayAvailableMove[n * 2 + 1]] == 20
                        && vlChessCur <20)
                    {
                        BlackInDanger = true;
                    }
                    if ( StatusBoardGird[Temp_ArrayAvailableMove[n * 2], Temp_ArrayAvailableMove[n * 2 + 1]] == 0
                        && vlChessCur >=20)
                    {
                        RedInDanger = true;
                    }
                }
            }
        }
        // Show General is in danger
        for (int i = 0; i < boradGird.transform.childCount; i++)
        {
            boradGird.transform.GetChild(i).GetChild(2).gameObject.SetActive(false);
            GameObject chessCurOnTile = boradGird.transform.GetChild(i).GetComponent<TileGirdContronller>().GetChessOnTile();
            if (chessCurOnTile != null)
            {
                byte vlChessCur = chessCurOnTile.GetComponent<ChessController>().GetValueChess();
                if (BlackInDanger && vlChessCur == 20)
                {
                    boradGird.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);
                }
                if (RedInDanger && vlChessCur == 0)
                {
                    boradGird.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);
                }
            }
        }
    }
    private void ChecKAndShowResultWhenWinGame()
    {
        if (CheckWinGame() == (byte)QuanCoTuong.Red_Tuong)
        {
            // Turn off gird to can't control piece
            boradGird.SetActive(false);
            // Show to screen that Red team is Winer
            // do something to show result ....

        }
        if (CheckWinGame() == (byte)QuanCoTuong.Black_Tuong)
        {
            boradGird.SetActive(false);
            //Show to screen that Black team is winner
            // do something to show result
        }
    }
    #region Photon PunRPC
    [PunRPC]
    private void PunRPCs_ChangeTurnTeam()
    {
        if (TurnTeam == TurnTeamPlayer.Black_Team_1)
        {
            TurnTeam = TurnTeamPlayer.Red_Team_1;
        }else
        {
            TurnTeam = TurnTeamPlayer.Black_Team_1;
        }
    }

    [PunRPC]
    private void PunRPCs_SynchronizeMatrix(byte row, byte column, byte valueChes)
    {
        StatusBoardGird[row, column] = valueChes;
        SetPositionObjChessBaseOnMatrix();
    }

    #endregion

}
