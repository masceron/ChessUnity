using Game.Action;
using Game.Action.Internal;
using Game.Effects.SpecialAbility;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BasketStar : Commons.PieceLogic
    {
        public BasketStar(PieceConfig cfg) : base(cfg, PhantomJellyMoves.Quiets, PhantomJellyMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new BasketStarPassive(this)));
        }
    }
}