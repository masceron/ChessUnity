using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Buffs;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using SnappingStrike = Game.Effects.Traits.SnappingStrike;

namespace Game.Effects.Augmentation
{
    public class CursedJawPassive : Effect, IOnApplyTrigger, IAfterPieceActionTrigger
    {
        private const int stunDuration = 1;

        public CursedJawPassive(PieceLogic piece) : base(-1, 1, piece, "effect_cursed_jaw_passive")
        {
        }

        public AfterActionPriority Priority => AfterActionPriority.Debuff;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not ICaptures) return;
            if (action.GetMaker() != Piece) return;

            ActionManager.EnqueueAction(new ApplyEffect(new Stunned(stunDuration, Piece)));
        }

        public void OnApply()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new SnappingStrike(Piece)));
            ActionManager.EnqueueAction(new ApplyEffect(new TrueBite(-1, Piece)));
        }
    }
}