
using Game.Action.Internal;
using Game.Action;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic.Commons
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MoorishIdols : PieceLogic
    {
        public MoorishIdols(PieceConfig cfg) : base(cfg, UpDoorMoves.Quiets, UpDoorMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Evasion(-1, 25, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new FreeMovement(this)));
        }
    }
}

