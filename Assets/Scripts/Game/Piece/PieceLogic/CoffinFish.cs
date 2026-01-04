using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class CoffinFish : Commons.PieceLogic
    {
        public CoffinFish(PieceConfig cfg) : base(cfg, KingMoves.Quiets, None.Captures)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Demolisher(this)));
            ActionManager.EnqueueAction(new ApplyEffect(new CoffinFishVengeful(this, 25)));
        }
    }
}