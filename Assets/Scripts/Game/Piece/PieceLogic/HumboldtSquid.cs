using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HumboldtSquid : PieceLogic
    {
        public HumboldtSquid(PieceConfig cfg) : base(cfg, AmbushPredatorMoves.Quiets, AmbushPredatorMoves.Captures)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new SnappingStrike(this)));
            ActionManager.EnqueueAction(new ApplyEffect(new TrueBite(this)));
            ActionManager.EnqueueAction(new ApplyEffect(new HumboldtSquidPassive(this)));
        }
    }
}