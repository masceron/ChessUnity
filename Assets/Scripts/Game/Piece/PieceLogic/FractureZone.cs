using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class FractureZone : Commons.PieceLogic
    {
        public FractureZone(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new FractureZonePassive(this)));
        }
    }
}

