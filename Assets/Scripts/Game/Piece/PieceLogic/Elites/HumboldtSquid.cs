using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Effects.Buffs;

namespace Game.Piece.PieceLogic.Elites
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