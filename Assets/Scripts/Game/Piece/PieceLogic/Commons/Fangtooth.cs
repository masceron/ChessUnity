using Game.Movesets;
using Game.Action;
using Game.Effects.Buffs;
using Game.Action.Internal;
using Game.Effects.Traits;


namespace Game.Piece.PieceLogic.Commons
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

