using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PorcelainCrab : Commons.PieceLogic
    {
        public PorcelainCrab(PieceConfig cfg) : base(cfg, ShellfishMoves.Quiets, ShellfishMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Piercing(-1, this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Vigorous(this)));
        }
    }
}