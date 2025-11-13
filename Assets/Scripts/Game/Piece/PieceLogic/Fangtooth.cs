using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Fangtooth : PieceLogic
    {
        public Fangtooth(PieceConfig cfg) : base(cfg, SmallChargingMoves.Quiets, SmallChargingMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Piercing(-1, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Sanity(-1, this)));
        }
    }
}

