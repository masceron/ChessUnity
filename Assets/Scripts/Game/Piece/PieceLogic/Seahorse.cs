using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Seahorse: PieceLogic
    {
        public Seahorse(PieceConfig cfg) : base(cfg, KnightSurpass.Quiets, KnightSurpass.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Surpass(this)));
        }
    }
}