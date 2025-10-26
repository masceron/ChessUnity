using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Piece.PieceLogic;
using UnityEngine;
namespace Game.Effects.RegionalEffect
{
    public class BloodMoon : RegionalEffect
    {
        protected override void ApplyEffect(int currentTurn)
        {
            if (MatchManager.Ins.GameState.IsDay) return;
            PieceLogic[] board = MatchManager.Ins.GameState.PieceBoard;
            List<PieceLogic> pieces = new List<PieceLogic>();
            foreach(PieceLogic piece in board){
                if (piece != null){
                    pieces.Add(piece);
                }
            }
            int randomInd = Random.Range(0, pieces.Count);
            ActionManager.ExecuteImmediately(new ApplyEffect(new Bleeding(5, pieces[randomInd])));
        }
    }
}