using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using UnityEngine;

namespace Game.Action.Skills
{
    public class ElectricEelActiveImpact : Action, IAOE
    {
        public ElectricEelActiveImpact(int maker, int target) : base(Game.Common.BoardUtils.GetEntityByID(maker), target)
        {
            Maker = maker;
        }

        protected override void ModifyGameState()
        {
            var pieceTarget = BoardUtils.PieceOn((int)Target);
            var pieceMaker = GetMakerAsPiece();
            if (pieceTarget == null || pieceMaker == null) return;

            ActionManager.EnqueueAction(new ApplyEffect(new Stunned(1, pieceTarget), pieceMaker));
        }
    }

}
