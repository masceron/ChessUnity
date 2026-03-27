using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using UnityEngine;

namespace Game.Action.Skills
{
    public class LionfishActiveImpact : Action, IAOE
    {
        public LionfishActiveImpact(int maker, int target) : base(maker)
        {
            Maker = maker;
            Target = target;
        }

        protected override void ModifyGameState()
        {
            var pieceTarget = BoardUtils.PieceOn((int)Target);
            var pieceMaker = BoardUtils.PieceOn(Maker);
            if (pieceTarget == null || pieceMaker == null) return;

            ActionManager.EnqueueAction(new ApplyEffect(new Poison(1, pieceTarget), pieceMaker));
        }
    }

}

