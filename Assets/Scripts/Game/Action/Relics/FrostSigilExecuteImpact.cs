using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Debuffs;
using UnityEngine;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Action.Relics
{
    public class FrostSigilExecuteImpact : Action, IAOE
    {
        private int ProbabilityBound = 25;

        public FrostSigilExecuteImpact(PieceLogic maker, int target, int probabilityBound) : base(maker, target)
        {
            ProbabilityBound = probabilityBound;
        }

        protected override void ModifyGameState()
        {
            var pieceTarget = BoardUtils.PieceOn(GetTargetPos());
            var pieceMaker = GetMakerAsPiece();
            ActionManager.EnqueueAction(new ApplyEffect(new Slow(3, 1, pieceTarget), pieceMaker));

            if (MatchManager.Roll(ProbabilityBound))
                ActionManager.EnqueueAction(new ApplyEffect(new Bound(3, pieceTarget), pieceMaker));
        }
    }

}
