using Game.Action;
using Game.Action.Internal;
using Game.Data.Pieces;
using Game.Effects.Traits;
using Game.Moves;

namespace Game.Piece.PieceLogic.Swarm
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Seahorse: PieceLogic
    {
        public Seahorse(PieceConfig cfg) : base(cfg, KnightMoves.Quiets, KnightMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Surpass(this)));
        }
    }
}