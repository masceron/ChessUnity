using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using UnityEngine;

namespace Game.Action.Skills
{
    public class LionfishActiveImpact : Action, IAOE
    {
        public LionfishActiveImpact(int maker, int target) : base(Game.Common.BoardUtils.GetEntityByID(maker), target)
        {
            Maker = maker;
        }

        protected override void ModifyGameState()
        {
            var pieceTarget = BoardUtils.PieceOn((int)Target);
            var pieceMaker = GetMakerAsPiece();
            if (pieceTarget == null || pieceMaker == null) return;

            ActionManager.EnqueueAction(new ApplyEffect(new Poison(1, pieceTarget), pieceMaker));
        }
    }

}

