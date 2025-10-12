using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Piece.PieceLogic;
using UnityEngine;

namespace Game.Effects.RegionalEffect
{
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
}