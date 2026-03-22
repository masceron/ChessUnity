using Game.Action;
using Game.Action.Internal;
using Game.Effects.SpecialAbility;
using Game.Effects.States;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class WireCoralGoby : Commons.PieceLogic
    {
        public WireCoralGoby(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Symbiotic(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new WireCoralGobyPassive(this)));
        }
    }
}