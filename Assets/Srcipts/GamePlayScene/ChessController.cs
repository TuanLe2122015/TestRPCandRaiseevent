using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessController : MonoBehaviour
{
    float Anchor_X;
    float Anchor_Z;
    const float position_Y = 0.65f;
    GameObject TileAnchorGird ;
    byte ValueChess;
    const byte countRow = 10;
    const byte countColumn = 9;

    byte IndexRowChess ;
    byte IndexColumnChess ;

    byte[] ArrayAvailableMove = new byte[34];// Quan Xe co the di chuyen 17 Tile
    byte countMoveTile = 0;

    // Start is called before the first frame update
    void Awake()
    {
        TileAnchorGird = GameObject.Find("AnchorTile_Corner_Board");
        Anchor_X = TileAnchorGird.transform.position.x;
        Anchor_Z = TileAnchorGird.transform.position.z;
    }
    public void SetValueChess(byte setVa)
    {
        ValueChess = setVa;
    }

    public void SetPositon(int row, int column)
    {
        TileAnchorGird = GameObject.Find("AnchorTile_Corner_Board");
        Anchor_X = TileAnchorGird.transform.position.x;
        Anchor_Z = TileAnchorGird.transform.position.z;

        float position_z = Anchor_Z - 2*row;
        float position_x = Anchor_X + 2*column;
        transform.SetPositionAndRotation(new Vector3(position_x,position_Y,position_z),transform.rotation);
    }

    public byte GetValueChess()
    {
        return ValueChess;
    }

    public void MoveToTileGird(GameObject tileByClick)
    {
        //Debug.Log("Move chess 2");
        float position_z = tileByClick.transform.position.z;
        float position_x = tileByClick.transform.position.x;
        this.transform.SetPositionAndRotation(new Vector3(position_x, position_Y, position_z), transform.rotation);
    }

    public byte [] ArrayAvailableMoveTile(byte rowChess, byte ColumnChess, byte [,] statusMatrix)
    {
        countMoveTile = 0;
        IndexRowChess = rowChess;
        IndexColumnChess = ColumnChess;
        switch (ValueChess)
        {
            // Available Tile Quan xe
            case 5:case 6:case 25:case 26:
                    Quan_XaAvailableMove( statusMatrix);
                break;
            case 7: case 8: case 27:case 28:
                Quan_PhaoAvailableMove(statusMatrix);
                break;
            // Quan ma~
            case 9:case 10:case 29:case 30:
                Quan_MaAvailableMove(statusMatrix);
                break;
            // Quan voi
            case 3:case 4:case 23:case 24:
                Quan_VoiAvailableMove(statusMatrix);
                break;
            // Quan Sy
            case 1:case 2: case 21:case 22:
                Quan_SyAvailableMove(statusMatrix);
                break;
            // Quan Tot'
            case 11: case 12:case 13:case 14: case 15:case 31:case 32:case 33: case 34: case 35:
                Quan_TotAvailableMove(statusMatrix);
                break;
            // General
            case 0: case 20:
                The_King_ChessAvailableMove(statusMatrix);
                break;
        }
        return ArrayAvailableMove;
    }

    #region Logic Chess piece
    private void The_King_ChessAvailableMove(byte[,] statusMatrix)
    {
        byte centerOfKingDoom_Row = 8, centerOfKingDoom_Column = 4;

        byte RowToCheckKing = 0;
        byte ValueChessOppoment = 0;

        if (ValueChess == 0)
        {
            ValueChessOppoment = 20;
            RowToCheckKing = 7;
            centerOfKingDoom_Row = 1;
        }

        if (
                (Mathf.Pow(IndexRowChess + 1 - centerOfKingDoom_Row, 2) + Mathf.Pow(IndexColumnChess - centerOfKingDoom_Column, 2)) < 2.25
                && IsDifferentTeamOrBlank((byte)(IndexRowChess + 1), (byte)(IndexColumnChess), IndexRowChess, IndexColumnChess, statusMatrix)
            )
        {
            AddIndexTileAvailableMoveArray((byte)(IndexRowChess + 1), (byte)(IndexColumnChess));
        }

        if (
                (Mathf.Pow(IndexRowChess - 1 - centerOfKingDoom_Row, 2) + Mathf.Pow(IndexColumnChess - centerOfKingDoom_Column, 2)) < 2.25
                && IsDifferentTeamOrBlank((byte)(IndexRowChess - 1), (byte)(IndexColumnChess), IndexRowChess, IndexColumnChess, statusMatrix)
            )
        {
            AddIndexTileAvailableMoveArray((byte)(IndexRowChess - 1), (byte)(IndexColumnChess));
        }

        if (
                (Mathf.Pow(IndexRowChess - centerOfKingDoom_Row, 2) + Mathf.Pow(IndexColumnChess + 1 - centerOfKingDoom_Column, 2)) < 2.25
                && IsDifferentTeamOrBlank((byte)(IndexRowChess), (byte)(IndexColumnChess + 1), IndexRowChess, IndexColumnChess, statusMatrix)
            )
        {
            AddIndexTileAvailableMoveArray((byte)(IndexRowChess), (byte)(IndexColumnChess + 1));
        }

        if (
                (Mathf.Pow(IndexRowChess - centerOfKingDoom_Row, 2) + Mathf.Pow(IndexColumnChess - 1 - centerOfKingDoom_Column, 2)) < 2.25
                && IsDifferentTeamOrBlank((byte)(IndexRowChess), (byte)(IndexColumnChess - 1), IndexRowChess, IndexColumnChess, statusMatrix)
            )
        {
            AddIndexTileAvailableMoveArray((byte)(IndexRowChess), (byte)(IndexColumnChess - 1));
        }

        // Neu' hai tuong' doi' mat. nhau
        // Neu' 2 tuong' cung` mot. cot. va` giua~ chung' khong co' bat' ky` Quan co` nao`
        for (int i = 0; i < 3; i++)
        {
            if (statusMatrix[i + RowToCheckKing, IndexColumnChess] == ValueChessOppoment)// Neu' 2 tuong' tren cung` mot. cot.
            {
                if (ValueChessOppoment == 20) // Neu' la Red King
                {
                    bool Facing = true;
                    //Debug.Log("Red King is clicked ...");
                    for (int g = IndexRowChess + 1; g < i + RowToCheckKing; g++)// Giua~ 2 King Chess khong co bat' ky` Quan co` nao`
                    {
                        if (statusMatrix[g, IndexColumnChess] != 40) // Co' Quan co` giua~ 2 Vua
                        {
                            Facing = false;
                            break;
                        }
                    }
                    if (!Facing) break;
                    //Debug.Log("Tow King are facing ...");
                    AddIndexTileAvailableMoveArray((byte)(i + RowToCheckKing), IndexColumnChess);
                }
                else // Neu la Black King
                {
                    bool Facing = true;
                    //Debug.Log("Black King is clicked ...");
                    for (int g = IndexRowChess - 1; g > i + RowToCheckKing; g--)// Giua~ 2 King Chess khong co bat' ky` Quan co` nao`
                    {
                        if (statusMatrix[g, IndexColumnChess] != 40) // Co' quan co` giua~ 2 vua
                        {
                            Facing = false;
                            break;
                        }
                    }
                    if (!Facing) break;
                    //Debug.Log("Tow King are facing ...");
                    AddIndexTileAvailableMoveArray((byte)(i + RowToCheckKing), IndexColumnChess);
                }

                break;
            }
        }
    }

    private void Quan_TotAvailableMove(byte[,] statusMatrix)
    {
        int MoveTot_Ahead = 1, River = 4;
        // Black team
        if (ValueChess > 20)
        {
            MoveTot_Ahead = -1;
            River = 5;
        }

        if (IsDifferentTeamOrBlank((byte)(IndexRowChess + MoveTot_Ahead), IndexColumnChess, IndexRowChess, IndexColumnChess, statusMatrix)

            )
        {
            AddIndexTileAvailableMoveArray((byte)(IndexRowChess + MoveTot_Ahead), IndexColumnChess);
        }
        //  Quan tot' da~ qua khoi~ song
        if ((River == 4 && IndexRowChess > River) || (River == 5 && IndexRowChess < River))
        {
            if (IsDifferentTeamOrBlank((byte)(IndexRowChess), (byte)(IndexColumnChess + 1), IndexRowChess, IndexColumnChess, statusMatrix))
            {
                AddIndexTileAvailableMoveArray((byte)(IndexRowChess ), (byte)(IndexColumnChess + 1));
            }
            if (IsDifferentTeamOrBlank((byte)(IndexRowChess), (byte)(IndexColumnChess-1), IndexRowChess, IndexColumnChess, statusMatrix))
            {
                AddIndexTileAvailableMoveArray((byte)(IndexRowChess ), (byte)(IndexColumnChess - 1));
            }

        }
    }

    void Quan_SyAvailableMove(byte[,] statusMatrix)
    {
        byte centerKingDoom_Row, centerKingDom_Column;
        
        // Red team
        if (ValueChess < 20)
        {
            centerKingDoom_Row = 1;
            centerKingDom_Column = 4;
        }
        else // Black team
        {
            centerKingDoom_Row = 8;
            centerKingDom_Column = 4;
        }

        if (
                (Mathf.Pow(IndexRowChess + 1 - centerKingDoom_Row, 2) + Mathf.Pow(IndexColumnChess + 1 - centerKingDom_Column, 2)) < 2.25
                && IsDifferentTeamOrBlank((byte)(IndexRowChess + 1), (byte)(IndexColumnChess + 1), IndexRowChess, IndexColumnChess, statusMatrix)
            )
        {
            AddIndexTileAvailableMoveArray((byte)(IndexRowChess + 1), (byte)(IndexColumnChess + 1));
        }

        if ((Mathf.Pow(IndexRowChess + 1 - centerKingDoom_Row,2) + Mathf.Pow(IndexColumnChess - 1 - centerKingDom_Column,2)) < 2.25
            && IsDifferentTeamOrBlank((byte)(IndexRowChess + 1), (byte)(IndexColumnChess - 1), IndexRowChess, IndexColumnChess, statusMatrix)
            )
        {
            AddIndexTileAvailableMoveArray((byte)(IndexRowChess + 1), (byte)(IndexColumnChess - 1));
        }
        if ((Mathf.Pow( IndexRowChess - 1 - centerKingDoom_Row,2) + Mathf.Pow(IndexColumnChess + 1 - centerKingDom_Column,2)) < 2.25
            && IsDifferentTeamOrBlank((byte)(IndexRowChess - 1), (byte)(IndexColumnChess + 1), IndexRowChess, IndexColumnChess, statusMatrix)
            )
        {
            AddIndexTileAvailableMoveArray((byte)(IndexRowChess - 1), (byte)(IndexColumnChess + 1));
        }
        if ((Mathf.Pow(IndexRowChess - 1 - centerKingDoom_Row,2) + Mathf.Pow(IndexColumnChess - 1 - centerKingDom_Column,2)) < 2.25
            && IsDifferentTeamOrBlank((byte)(IndexRowChess - 1), (byte)(IndexColumnChess - 1), IndexRowChess, IndexColumnChess, statusMatrix)
            )
        {
            AddIndexTileAvailableMoveArray((byte)(IndexRowChess - 1), (byte)(IndexColumnChess - 1));
        }

    }
    private void Quan_VoiAvailableMove(byte[,] statusMatrix)
    {
        // Red team Quan voi
        if (ValueChess < 20)
        {
            if (IndexRowChess + 1 <= 3 && IndexColumnChess + 1 <= countColumn - 1 &&
                statusMatrix[ IndexRowChess + 1, IndexColumnChess + 1] ==40 &&
                IsDifferentTeamOrBlank((byte)(IndexRowChess + 2), (byte)(IndexColumnChess + 2), IndexRowChess, IndexColumnChess, statusMatrix)
               )
            {
                AddIndexTileAvailableMoveArray((byte)(IndexRowChess + 2), (byte)(IndexColumnChess + 2));
            }
            if (IndexRowChess + 1 <= 3 && IndexColumnChess - 1 >= 1 &&
                statusMatrix[IndexRowChess + 1, IndexColumnChess - 1] == 40 &&
                IsDifferentTeamOrBlank((byte)(IndexRowChess + 2), (byte)(IndexColumnChess - 2), IndexRowChess, IndexColumnChess, statusMatrix)
               )
            {
                AddIndexTileAvailableMoveArray((byte)(IndexRowChess + 2), (byte)(IndexColumnChess - 2));
            }
            if (IndexRowChess - 1 >= 1 && IndexColumnChess - 1 >= 1 &&
                statusMatrix[IndexRowChess - 1, IndexColumnChess - 1] == 40 &&
                IsDifferentTeamOrBlank((byte)(IndexRowChess - 2), (byte)(IndexColumnChess - 2), IndexRowChess, IndexColumnChess, statusMatrix)
               )
            {
                AddIndexTileAvailableMoveArray((byte)(IndexRowChess - 2), (byte)(IndexColumnChess - 2));
            }
            if (IndexRowChess - 1 >= 1 && IndexColumnChess + 1 <= countColumn -1 &&
                statusMatrix[IndexRowChess - 1, IndexColumnChess + 1] == 40 &&
                IsDifferentTeamOrBlank((byte)(IndexRowChess - 2), (byte)(IndexColumnChess + 2), IndexRowChess, IndexColumnChess, statusMatrix)
               )
            {
                AddIndexTileAvailableMoveArray((byte)(IndexRowChess - 2), (byte)(IndexColumnChess + 2));
            }
        }
        else // Black team quan voi
        {
            if (IndexRowChess + 1 <= countRow - 1 && IndexColumnChess + 1 <= countColumn - 1 &&
                statusMatrix[IndexRowChess + 1, IndexColumnChess + 1] == 40 &&
                IsDifferentTeamOrBlank((byte)(IndexRowChess + 2), (byte)(IndexColumnChess + 2), IndexRowChess, IndexColumnChess, statusMatrix)
               )
            {
                AddIndexTileAvailableMoveArray((byte)(IndexRowChess + 2), (byte)(IndexColumnChess + 2));
            }
            if (IndexRowChess + 1 <= countRow - 1 && IndexColumnChess - 1 >= 1 &&
                statusMatrix[IndexRowChess + 1, IndexColumnChess - 1] == 40 &&
                IsDifferentTeamOrBlank((byte)(IndexRowChess + 2), (byte)(IndexColumnChess - 2), IndexRowChess, IndexColumnChess, statusMatrix)
               )
            {
                AddIndexTileAvailableMoveArray((byte)(IndexRowChess + 2), (byte)(IndexColumnChess - 2));
            }
            if (IndexRowChess - 1 >= 6 && IndexColumnChess - 1 >= 1 &&
                statusMatrix[IndexRowChess - 1, IndexColumnChess - 1] == 40
                && IsDifferentTeamOrBlank((byte)(IndexRowChess - 2), (byte)(IndexColumnChess - 2), IndexRowChess, IndexColumnChess, statusMatrix)
               )
            {
                AddIndexTileAvailableMoveArray((byte)(IndexRowChess - 2), (byte)(IndexColumnChess - 2));
            }
            if (IndexRowChess - 1 >= 6 && IndexColumnChess + 1 <= countColumn - 1 &&
                statusMatrix[IndexRowChess - 1, IndexColumnChess + 1] == 40 &&
                IsDifferentTeamOrBlank((byte)(IndexRowChess - 2), (byte)(IndexColumnChess + 2), IndexRowChess, IndexColumnChess, statusMatrix)
               )
            {
                AddIndexTileAvailableMoveArray((byte)(IndexRowChess - 2), (byte)(IndexColumnChess + 2));
            }
        }
    }

    void Quan_XaAvailableMove(byte[,] statusMatrix)
    {
        // Boi vi` Chess tren TileGird

        // Xet' theo cot
        for (byte i = 0; i < countRow; i++) // Tu` Quan co` dc chon. di len
        {
            // Neu vuot ngoai matrix break
            if (IndexRowChess + i + 1 > countRow - 1) break;

            bool facingOppomentChess = false;
            //Neu' dung quan co
            //...... cung` team break
            //...... khac' team, them vao` Tile co' the di, tai vi tri' quan co` khac' team, break
            if (statusMatrix[IndexRowChess + i + 1, IndexColumnChess] != 40)
            {
                // Cung` team: break
                // Or
                // Khac' team
                if(IsSameTeam(
                                (
                                byte)(IndexRowChess + i + 1), IndexColumnChess,
                                IndexRowChess, IndexColumnChess,
                                statusMatrix
                                )
                  )
                {
                    break;
                }
                facingOppomentChess = true;
            }


            ArrayAvailableMove[2 * countMoveTile] = (byte)(IndexRowChess + i + 1);
            ArrayAvailableMove[2 * countMoveTile + 1] = (byte)(IndexColumnChess);
            countMoveTile++;
            if (facingOppomentChess) break;

        }
        for (int i = 0; i < countRow; i++) // Tu` Quan co` dc chon di xuong'
        {
            // Neu vuot ngoai matrix break
            if (IndexRowChess - i - 1 < 0) break;

            bool facingOppomentChess = false;
            //Neu' dung quan co
            //...... cung` team break
            //...... khac' team, them vao` Tile co' the di, tai vi tri' quan co` khac' team, break
            if (statusMatrix[IndexRowChess - i - 1, IndexColumnChess] != 40)
            {
                // Cung` team: break
                // Or
                // Khac' team
                if (IsSameTeam(
                                (
                                byte)(IndexRowChess - i - 1), IndexColumnChess,
                                IndexRowChess, IndexColumnChess,
                                statusMatrix
                                )
                  )
                {
                    break;
                }
               facingOppomentChess = true;
            }


            ArrayAvailableMove[2 * countMoveTile] = (byte)(IndexRowChess - i - 1);
            ArrayAvailableMove[2 * countMoveTile + 1] = (byte)(IndexColumnChess);
            countMoveTile++;
            if (facingOppomentChess) break;

        }

        // Xet' theo hang`
        for (int i = 0; i < countColumn; i++)
        {
            // Neu vuot ngoai matrix break
            if (IndexColumnChess + i + 1 > countColumn-1) break;

            bool facingOppomentChess = false;
            //Neu' dung quan co
            //...... cung` team break
            //...... khac' team, them vao` Tile co' the di, tai vi tri' quan co` khac' team, break
            if (statusMatrix[IndexRowChess, IndexColumnChess + i + 1] != 40)
            {
                // Cung` team: break
                // Or
                // Khac' team
                if (IsSameTeam(
                                IndexRowChess, (byte)(IndexColumnChess+i+1),
                                IndexRowChess, IndexColumnChess,
                                statusMatrix
                              )
                  )
                {
                    break;
                }
                facingOppomentChess = true;
            }


            ArrayAvailableMove[2 * countMoveTile] = (byte)(IndexRowChess);
            ArrayAvailableMove[2 * countMoveTile + 1] = (byte)(IndexColumnChess + i + 1);
            countMoveTile++;
            if( facingOppomentChess) break;
        }
        for (int i = 0; i < countColumn; i++)
        {
            // Neu vuot ngoai matrix break
            if (IndexColumnChess - i - 1 < 0) break;

            bool facingOppomentChess = false;
            //Neu' dung quan co
            //...... cung` team break
            //...... khac' team, them vao` Tile co' the di, tai vi tri' quan co` khac' team, break
            if (statusMatrix[IndexRowChess, IndexColumnChess - i - 1] != 40)
            {
                // Cung` team: break
                // Or
                // Khac' team
                
                if (IsSameTeam(
                                IndexRowChess, (byte)(IndexColumnChess - i - 1),
                                IndexRowChess, IndexColumnChess,
                                statusMatrix
                              )
                  )
                {
                    break;
                }
                 facingOppomentChess = true;
            }

            ArrayAvailableMove[2 * countMoveTile] = (byte)(IndexRowChess );
            ArrayAvailableMove[2 * countMoveTile + 1] = (byte)(IndexColumnChess - i - 1);
            countMoveTile++;
            if (facingOppomentChess) break;
        }
    }
    void Quan_PhaoAvailableMove(byte[,] statusMatrix)
    {
        // Boi vi` Chess tren TileGird

        // Xet' theo cot
        for (int i = 0; i < countRow; i++) // Tu` Quan co` dc chon. di len
        {
            // Neu vuot ngoai matrix break
            if (IndexRowChess + i + 1 > countRow - 1) break;

            
            //Neu' Phao' dung bat' ky` quan co nao`
            if (statusMatrix[IndexRowChess + i + 1, IndexColumnChess] != 40)
            {
                // Xet' ben kia Quan dung. dau` tien dung. phai~
                for (int g = IndexRowChess + i + 2; g < countRow; g ++)
                {
                    if (g > countRow - 1) break;
                    if (statusMatrix[g, IndexColumnChess] != 40)
                    {
                        // Neu' dung. chess khac' team thi` them vao` ArrayAvailableMove
                        if(!IsSameTeam((byte)g, IndexColumnChess,IndexRowChess,IndexColumnChess, statusMatrix))
                        {
                            AddIndexTileAvailableMoveArray((byte)(g), (byte)(IndexColumnChess));
                        }
                        break;
                    }
                }

                break;
            }
            AddIndexTileAvailableMoveArray((byte)(IndexRowChess + i + 1), (byte)(IndexColumnChess));

        }

        for (int i = 0; i < countRow; i++) // Tu` Quan co` dc chon di xuong'
        {
            // Neu vuot ngoai matrix break
            if (IndexRowChess - i - 1 < 0) break;

            //Neu' dung quan co
            if (statusMatrix[IndexRowChess - i - 1, IndexColumnChess] != 40)
            {
                // kiem tra phia' ben kia quan Phao'
                    // Neu' dung. Quan co` dau` tien
                for (int g = IndexRowChess-  i - 2; g > -1; g--)
                {
                    if (g < 0) break;
                    // Neu' dung. Quan co` dau` tien
                    if (statusMatrix[g, IndexColumnChess] != 40)
                    {
                        // Neu' khac' team thi` them vao` Array Available Move
                        if (!IsSameTeam(IndexRowChess, IndexColumnChess, (byte)g, IndexColumnChess, statusMatrix))
                        {
                            AddIndexTileAvailableMoveArray((byte)(g), (byte)(IndexColumnChess));
                        }
                        break;
                    }

                }

                break;
            }
            AddIndexTileAvailableMoveArray((byte)(IndexRowChess - i - 1), (byte)(IndexColumnChess));
        }

        // Xet' theo hang`
        for (int i = 0; i < countColumn; i++)
        {
            // Neu vuot ngoai matrix break
            if (IndexColumnChess + i + 1 > countColumn - 1) break;

            //Neu' dung quan co
            if (statusMatrix[IndexRowChess, IndexColumnChess + i + 1] != 40)
            {
                // kiem tra phia' ben kia quan Phao'
                for (int g = IndexColumnChess + i + 2; g < countColumn; g++)
                {
                    if (g > countColumn - 1) break;
                    // Neu' dung. Quan co` dau` tien
                    if (statusMatrix[IndexRowChess, g] != 40)
                    {
                        // Neu' khac' team thi` them vao` Array Available Move
                        if (!IsSameTeam(IndexRowChess, IndexColumnChess, IndexRowChess, (byte)g, statusMatrix))
                        {
                            AddIndexTileAvailableMoveArray((byte)(IndexRowChess), (byte)(g));
                        }
                        break;
                    }

                }

                break;
            }
            AddIndexTileAvailableMoveArray((byte)(IndexRowChess), (byte)(IndexColumnChess + i + 1));
        }
        for (int i = 0; i < countColumn; i++)
        {
            // Neu vuot ngoai matrix break
            if (IndexColumnChess - i - 1 < 0) break;

            //Neu' dung quan co
            if (statusMatrix[IndexRowChess, IndexColumnChess - i - 1] != 40)
            {
                // kiem tra phia' ben kia quan Phao'
                for (int g = IndexColumnChess - i - 2; g > -1; g--)
                {
                    if (g < 0) break;
                    // Neu' dung. Quan co` dau` tien
                    if (statusMatrix[IndexRowChess, g] != 40)
                    {
                        // Neu' khac' team thi` them vao` Array Available Move
                        if (!IsSameTeam(IndexRowChess, IndexColumnChess, IndexRowChess, (byte)g, statusMatrix))
                        {
                            AddIndexTileAvailableMoveArray((byte)(IndexRowChess), (byte)(g));
                        }
                        break;
                    }

                }

                break;
            }
            AddIndexTileAvailableMoveArray((byte)(IndexRowChess), (byte)(IndexColumnChess - i - 1));
        }

    }
    void Quan_MaAvailableMove(byte[,] statusMatrix)
    {
        // Xet' 4 mat. ke` cua~ Quan ma~, neu' bi. can~ thi` khong the~ di chuyen~

        if (
                IndexRowChess + 1 < countRow - 1 &&
                statusMatrix[IndexRowChess + 1, IndexColumnChess] == 40 
           ) // Khong co' Quan can~ ke' ben va` phai~ nam` cach' bien 2 o
        {
            // Neu' la` Quan co` doi' thu~ hoac Tile trong' thi` add
            if (
                     IndexRowChess + 2 < countRow && IndexColumnChess - 1 > -1 &&
                    (statusMatrix[IndexRowChess + 2, IndexColumnChess - 1] == 40 || !IsSameTeam((byte)(IndexRowChess + 2), (byte)(IndexColumnChess - 1), IndexRowChess, IndexColumnChess, statusMatrix))
               )
            {
                AddIndexTileAvailableMoveArray((byte)(IndexRowChess + 2), (byte)(IndexColumnChess - 1));
            }
            if (
                    IndexRowChess + 2 < countRow && IndexColumnChess + 1 < countColumn &&
                    (statusMatrix[IndexRowChess + 2, IndexColumnChess + 1] == 40 || !IsSameTeam((byte)(IndexRowChess + 2), (byte)(IndexColumnChess + 1), IndexRowChess, IndexColumnChess, statusMatrix))
               )
            {
                AddIndexTileAvailableMoveArray((byte)(IndexRowChess + 2), (byte)(IndexColumnChess + 1));
            }
        }

        if (
            IndexRowChess - 1 > 0 &&
            statusMatrix[IndexRowChess - 1, IndexColumnChess] == 40
           )
        {
            if (
                    IndexRowChess - 2 > -1 && IndexColumnChess - 1 > -1 &&
                    (statusMatrix[IndexRowChess - 2, IndexColumnChess - 1] == 40 || !IsSameTeam((byte)(IndexRowChess - 2), (byte)(IndexColumnChess - 1), IndexRowChess, IndexColumnChess, statusMatrix))
                )
            {
                AddIndexTileAvailableMoveArray((byte)(IndexRowChess - 2), (byte)(IndexColumnChess - 1));
            }
            if (
                    IndexRowChess - 2 > -1 && IndexColumnChess + 1 < countColumn &&
                    (statusMatrix[IndexRowChess - 2, IndexColumnChess + 1] == 40 || !IsSameTeam((byte)(IndexRowChess - 2), (byte)(IndexColumnChess + 1), IndexRowChess, IndexColumnChess, statusMatrix))
               )
            {
                AddIndexTileAvailableMoveArray((byte)(IndexRowChess - 2), (byte)(IndexColumnChess + 1));
            }
        }

        if (
                IndexColumnChess + 1 < countColumn - 1 &&
                statusMatrix[IndexRowChess, IndexColumnChess + 1] == 40
           )
        {
            if (
                     IndexRowChess - 1 > -1 && IndexColumnChess + 2 < countColumn &&
                     (statusMatrix[IndexRowChess - 1, IndexColumnChess + 2] == 40 || !IsSameTeam((byte)(IndexRowChess - 1), (byte)(IndexColumnChess + 2), IndexRowChess, IndexColumnChess, statusMatrix))
                )
            {
                AddIndexTileAvailableMoveArray((byte)(IndexRowChess - 1), (byte)(IndexColumnChess + 2));
            }
            if (
                    IndexRowChess + 1 < countRow && IndexColumnChess + 2 < countColumn &&
                    (statusMatrix[IndexRowChess + 1, IndexColumnChess + 2] == 40 || !IsSameTeam((byte)(IndexRowChess + 1), (byte)(IndexColumnChess + 2), IndexRowChess, IndexColumnChess, statusMatrix))
                )
            {
                AddIndexTileAvailableMoveArray((byte)(IndexRowChess + 1), (byte)(IndexColumnChess + 2));
            }
        }

        if (
            IndexColumnChess - 1 > 0 &&
            statusMatrix[IndexRowChess, IndexColumnChess - 1] == 40
           )
        {
            if (
                    IndexRowChess - 1 > -1 && IndexColumnChess - 2 > -1 &&
                    (statusMatrix[IndexRowChess - 1, IndexColumnChess - 2] == 40 || !IsSameTeam((byte)(IndexRowChess - 1), (byte)(IndexColumnChess - 2), IndexRowChess, IndexColumnChess, statusMatrix))
                )
            {
                AddIndexTileAvailableMoveArray((byte)(IndexRowChess - 1), (byte)(IndexColumnChess - 2));
            }
            if (
                    IndexRowChess + 1 < countRow && IndexColumnChess - 2 > -1 &&
                    (statusMatrix[IndexRowChess + 1, IndexColumnChess - 2] == 40 || !IsSameTeam((byte)(IndexRowChess + 1), (byte)(IndexColumnChess - 2), IndexRowChess, IndexColumnChess, statusMatrix))
                )
            {
                AddIndexTileAvailableMoveArray((byte)(IndexRowChess + 1), (byte)(IndexColumnChess - 2));
            }
        }

    }
    #endregion


    void AddIndexTileAvailableMoveArray(byte RowIndex, byte ColumnIndex)
    {
        ArrayAvailableMove[2 * countMoveTile] = (byte)(RowIndex);
        ArrayAvailableMove[2 * countMoveTile + 1] = (byte)(ColumnIndex);
        countMoveTile++;
    }

    public byte GetCountTileAvalaible()
    {
        return countMoveTile;
    }

    // Dua vao` Matrix trang thai' kiem~ tra 2 Quan co` cung` team hay khong
    bool IsSameTeam(byte R_1, byte C_1, byte R_2, byte C_2 , byte[,] statusMatrix)
    {
        // Cung` team gia' tri. value chess cung` be' hon 16 OR cung` lon' 16
        if (    (statusMatrix[R_1, C_1] < 16 && statusMatrix[R_2, C_2] < 16) ||
                (statusMatrix[R_1, C_1] > 16 && statusMatrix[R_2, C_2] > 16)
           )
        {
            return true;
        }
            return false;
    }

    bool IsDifferentTeamOrBlank(byte R_1, byte C_1, byte R_2, byte C_2, byte[,] statusMatrix)
    {
        // Cung` team gia' tri. value chess cung` be' hon 16 OR cung` lon' 16
        if (R_1 < 0 || C_1 < 0) return false;
        if (R_1 > countRow - 1 || C_1 > countColumn - 1) return false;
        if (statusMatrix[R_1, C_1] == 40) return true;
        if ((statusMatrix[R_1, C_1] < 16 && statusMatrix[R_2, C_2] < 16) ||
                (statusMatrix[R_1, C_1] > 16 && statusMatrix[R_2, C_2] > 16)
           )
        {
            return false;
        }
        return true;
    }
}
