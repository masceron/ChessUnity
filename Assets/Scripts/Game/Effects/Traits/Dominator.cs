using Game.Piece.PieceLogic;
using static Game.Common.BoardUtils;
namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Dominator: Effect
    {
        public Dominator(PieceLogic piece) : base(-1, 1, piece, EffectName.Dominator)
        {}

        public override void OnCallPieceAction(Action.Action action)
        {
            var target = PieceOn(action.Target);
            if (target.PieceRank <= Piece.PieceRank) return;
        }

    }
}