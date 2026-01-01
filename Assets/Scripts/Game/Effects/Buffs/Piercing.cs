using Game.Action;
using Game.Action.Captures;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Buffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Piercing: Effect, IBeforePieceActionEffect
    {
        public Piercing(sbyte duration, PieceLogic piece) : base(duration, 1, piece, "effect_piercing")
        {}

        public void OnCallBeforePieceAction(Action.Action action)
        {
            if (action is ICaptures && action.Maker == Piece.Pos) action.Flag |= ActionFlag.Unblockable;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 30;
        }
    }
}