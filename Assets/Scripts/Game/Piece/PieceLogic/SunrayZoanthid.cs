using Game.Action;
using Game.Action.Internal;
using Game.Effects.Condition;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SunrayZoanthid : Commons.PieceLogic
    {
        private const int Radius = 3;
        private const int Probability = 100;

        public SunrayZoanthid(PieceConfig cfg) : base(cfg, CoralMove.Quiets, RookMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new SunrayZoanthidDiurnal(Radius, Probability, this)));
        }
    }
}
