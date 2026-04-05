using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;

namespace Game.Effects.RegionalEffect
{
    public class PsionicShock : FieldEffect
    {
        public PsionicShock() : base(RegionalEffectType.PsionicShock)
        {
        }

        protected override void ApplyEffect(int currentTurn)
        {
            var board = BoardUtils.PieceBoard();
            var pieces = new List<PieceLogic>();
            foreach (var piece in board)
                if (piece != null)
                    pieces.Add(piece);

            var randomInd = Random.Range(0, pieces.Count);
            ActionManager.EnqueueAction(new ApplyEffect(new Stunned(1, pieces[randomInd])));
        }
    }
}