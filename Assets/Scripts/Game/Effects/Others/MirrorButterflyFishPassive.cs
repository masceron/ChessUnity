using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Others
{
    public class MirrorButterflyFishPassive : Effect, IApplyEffect
    {
        private const int chance = 25;
        public MirrorButterflyFishPassive(PieceLogic piece) : base(-1, 1, piece, "effect_mirror_butterfly_fish_passive")
        {
        }


        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            // TODO: figure out what is a target skill.
            var maker = applyEffect.SourcePiece;
            var target = BoardUtils.PieceOn(applyEffect.Target);

            if (maker != null && target == Piece && MatchManager.Roll(chance))
            {
                ActionManager.EnqueueAction(new ApplyEffect(applyEffect.Effect, Piece));
            }
        }
    }
}