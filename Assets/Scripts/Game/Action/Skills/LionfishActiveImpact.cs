using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using UnityEngine;

namespace Game.Action.Skills
{
    public class LionfishActiveImpact : Action, IAOE
    {
        private readonly int _stack;

        public LionfishActiveImpact(int maker, int target, int stack) : base(Game.Common.BoardUtils.GetEntityByID(maker), target)
        {
            Maker = maker;
            _stack = stack;
        }

        protected override void ModifyGameState()
        {
            var pieceTarget = BoardUtils.PieceOn((int)Target);
            var pieceMaker = GetMakerAsPiece();
            if (pieceTarget == null || pieceMaker == null) return;

            ActionManager.EnqueueAction(new ApplyEffect(new Poison(_stack, pieceTarget), pieceMaker));
        }
    }

}

