using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HumboldtSquid : Commons.PieceLogic
    {
        public HumboldtSquid(PieceConfig cfg) : base(cfg, AmbushPredatorMoves.Quiets, AmbushPredatorMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new SnappingStrike(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new TrueBite(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new HumboldtSquidPassive(this)));
        }
    }
}