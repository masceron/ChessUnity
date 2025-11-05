using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Piece.PieceLogic;
using Game.Movesets;
using Game.Effects.Traits;
using Game.Effects.Buffs;
using Game.Effects.Debuffs;
using Game.Effects.Buffs;

namespace Game.Piece.PieceLogic.Elites
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HumboldtSquid : PieceLogic
    {
        public HumboldtSquid(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
            ActionManager.EnqueueAction(new ApplyEffect(new SnappingStrike(this)));
            ActionManager.EnqueueAction(new ApplyEffect(new TrueBite(this)));
            ActionManager.EnqueueAction(new ApplyEffect(new HumboldtSquidPassive(this)));
        }
    }
}