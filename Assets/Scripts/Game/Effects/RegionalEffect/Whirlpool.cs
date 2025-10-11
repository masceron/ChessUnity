using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Game.Action;
using Game.Action.Quiets;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Piece.PieceLogic;
using static Game.Common.BoardUtils;

//Xuất hiện vào turn thứ 30 của ván nếu chưa kết thúc, tạo ra vòng xoáy 2x2 giữa bàn cờ, 
// mỗi turn hút tất cả các quân vào gần với xoáy nước 1 ô. 
//Quân nào bị vào xoáy nước auto mất
public class Whirlpool : RegionalEffect{
    private int startTurn = 4;
    protected Whirlpool() : base(){

    }
    protected override void ApplyEffect(int currentTurn)
    {
        if (currentTurn <= startTurn){
            Debug.Log("Whirlpool is not started yet!");
            return;
        }
        PieceLogic[] board = MatchManager.Ins.GameState.PieceBoard;
        foreach(PieceLogic piece in board){
            if (piece != null) {
                int targetPosition = 0;
                ActionManager.ExecuteImmediately(new NormalMove(piece.Pos, targetPosition));
            }
        }

    }
}