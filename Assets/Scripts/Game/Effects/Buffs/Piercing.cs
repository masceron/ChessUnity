using Game.Action;
using Game.Piece.PieceLogic;

namespace Game.Effects.Buffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Piercing: Effect
    {
        public Piercing(sbyte duration, PieceLogic piece) : base(duration, 1, piece, EffectName.Piercing)
        {}

        public override void OnCallPieceAction(Action.Action action)
        {
            if (action != null && action.Maker == Piece.Pos) action.Result = ActionResult.Unblockable;
        }
    }
}