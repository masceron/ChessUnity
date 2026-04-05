using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;

namespace Game.Effects.FieldEffect
{
    public class BloodMoon : FieldEffect
    {
        public BloodMoon() : base(FieldEffectType.BloodMoon)
        {
        }

        protected override void ApplyEffect(int currentTurn)
        {
            if (BoardUtils.IsDay()) return;
            var board = BoardUtils.PieceBoard();
            var pieces = new List<PieceLogic>();
            foreach (var piece in board)
                if (piece != null)
                    pieces.Add(piece);

            var randomInd = Random.Range(0, pieces.Count);
            ActionManager.EnqueueAction(new ApplyEffect(new Bleeding(5, pieces[randomInd])));
        }
    }
}