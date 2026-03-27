using Game.Action;
using Game.Action.Captures;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Buffs
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class TrueBite : Effect, IBeforePieceActionTrigger
    {
        public TrueBite(int duration, PieceLogic piece) : base(duration, -1, piece, "effect_true_bite")
        {
        }

        public BeforeActionPriority Priority => BeforeActionPriority.Declaration;

        public void OnCallBeforePieceAction(Action.Action action)
        {
            if (action is ICaptures && action.GetMaker() == Piece) action.Flag |= ActionFlag.Undodgeable;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 30;
        }
    }
}