using Game.Action;
using Game.Movesets;
using Game.Action.Internal;

namespace Game.Piece.PieceLogic.Construct
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class FractureZone : PieceLogic
    {
        public FractureZone(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new FractureZonePassive(this)));
        }
    }
}

