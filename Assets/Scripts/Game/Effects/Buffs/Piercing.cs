using Game.Action;
using Game.Action.Captures;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Buffs
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Piercing : Effect, IBeforePieceActionTrigger
    {
        public Piercing(int duration, PieceLogic piece) : base(duration, 1, piece, "effect_piercing")
        {
        }

        public BeforeActionPriority Priority => BeforeActionPriority.Declaration;

        public void OnCallBeforePieceAction(Action.Action action)
        {
            if (action is ICaptures && action.GetMaker() == Piece) action.Flag |= ActionFlag.Unblockable;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 30;
        }
    }
}