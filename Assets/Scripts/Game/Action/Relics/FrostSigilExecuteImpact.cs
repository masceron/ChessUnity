using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Managers;
using UnityEngine;

namespace Game.Action.Relics
{
    public class FrostSigilExecuteImpact : Action, IAOE
    {
        private int ProbabilityBound = 25;

        public FrostSigilExecuteImpact(int maker, int target, int probabilityBound) : base(maker)
        {
            Maker = maker;
            Target = target;
            ProbabilityBound = probabilityBound;
        }

        protected override void ModifyGameState()
        {
            var pieceTarget = BoardUtils.PieceOn((int)Target);
            var pieceMaker = BoardUtils.PieceOn(Maker);
            ActionManager.EnqueueAction(new ApplyEffect(new Slow(3, 1, pieceTarget), pieceMaker));

            if (MatchManager.Roll(ProbabilityBound))
                ActionManager.EnqueueAction(new ApplyEffect(new Bound(3, pieceTarget), pieceMaker));
        }
    }

}
