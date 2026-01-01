using Game.Action;
using Game.Action.Internal;
using Game.Effects.SpecialAbility;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FlyingFish: Commons.PieceLogic
    {
        public FlyingFish(PieceConfig cfg) : base(cfg, FlyingFishMoves.Quiets, KnightSurpass.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Surpass(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new FlyingfishPassive(this)));
        }
    }
}