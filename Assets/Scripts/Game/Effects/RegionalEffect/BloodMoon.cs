using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;

namespace Game.Effects.RegionalEffect
{
    public class BloodMoon : RegionalEffect
    {
        public BloodMoon() : base(RegionalEffectType.BloodMoon)
        {
        }

        protected override void ApplyEffect(int currentTurn)
        {
            if (MatchManager.Ins.GameState.IsDay) return;
            var board = MatchManager.Ins.GameState.PieceBoard;
            var pieces = new List<PieceLogic>();
            foreach (var piece in board)
                if (piece != null)
                    pieces.Add(piece);

            var randomInd = Random.Range(0, pieces.Count);
            ActionManager.EnqueueAction(new ApplyEffect(new Bleeding(5, pieces[randomInd])));
        }
    }
}