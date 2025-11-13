using Game.Action;
using Game.Action.Internal;
using Game.Effects.Others;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Melibe : PieceLogic
    {
        public Melibe(PieceConfig cfg) : base(cfg, SmallPredatorMoves.Quiets, SmallPredatorMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new MelibePassive(this)));
        }
    }
}