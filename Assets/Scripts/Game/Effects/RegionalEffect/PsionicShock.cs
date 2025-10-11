using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Piece.PieceLogic;
using static Game.Common.BoardUtils;

public class PsionicShock : RegionalEffect{
    protected override void ApplyEffect(int currentTurn)
    {
        PieceLogic[] board = MatchManager.Ins.GameState.PieceBoard;
        List<PieceLogic> pieces = new List<PieceLogic>();
        foreach(PieceLogic piece in board){
            if (piece != null){
                pieces.Add(piece);
            }
        }
        int randomInd = Random.Range(0, pieces.Count);
        ActionManager.ExecuteImmediately(new ApplyEffect(new Stunned(1, pieces[randomInd])));
    }
}