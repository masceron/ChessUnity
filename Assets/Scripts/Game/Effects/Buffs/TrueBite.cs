using Game.Action;
using Game.Action.Captures;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Buffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class TrueBite: Effect, IBeforePieceActionEffect
    {
        public TrueBite(PieceLogic piece) : base(-1, -1, piece, "effect_true_bite")
        {}

        public void OnCallBeforePieceAction(Action.Action action)
        {
            if (action is ICaptures && action.Maker == Piece.Pos)
            {
                action.Flag |= ActionFlag.Undodgeable;
            }
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 30;
        }

        
    }
}