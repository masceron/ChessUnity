using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    public class GluttonousJawPassive : Effect, IAfterPieceActionTrigger
    {
        public GluttonousJawPassive(int duration, int strength, PieceLogic piece) : base(duration, strength, piece,
            "effect_gluttonous_jaw_passive")
        {
        }

        public AfterActionPriority Priority => AfterActionPriority.Buff;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not ICaptures) return;
            if (action.GetMaker() != Piece) return;

            var maker = PieceOn(action.Maker);
            if (maker.Effects.Any(e => e.EffectName == "effect_consume"))
                if (action.Result == ResultFlag.Success)
                    ActionManager.EnqueueAction(new ApplyEffect(new LongReach(maker, 2, 5)));
        }
    }
}