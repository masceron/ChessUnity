using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Effects.SpecialAbility;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]

    public class FrilledShark : Commons.PieceLogic
    {
        public FrilledShark(PieceConfig cfg) : base(cfg, KnightMoves.Quiets, KnightMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Sanity(-1, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new FrilledSharkPassive(this)));
        }
    }
}

