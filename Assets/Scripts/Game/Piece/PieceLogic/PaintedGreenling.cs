using Game.Action;
using Game.Action.Internal;
using Game.Effects.SpecialAbility;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class PaintedGreenling : Commons.PieceLogic
    {
        private const int Number = 1;
        private const int Duration = 3;
        private const int Radius = 1;
        public PaintedGreenling(PieceConfig cfg) : base(cfg, SpinningMoves.Quiets, None.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new PaintedGreenlingPassive(this, Number, Duration, Radius)));
        }
    }
}