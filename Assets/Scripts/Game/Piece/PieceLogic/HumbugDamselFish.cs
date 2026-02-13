using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HumbugDamselFish : Commons.PieceLogic
    {
        public HumbugDamselFish(PieceConfig cfg) : base(cfg, VersatileDefenderMove.Quiets,
            VersatileDefenderMove.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new HumbugDamselFishPassive(this)));
        }
    }
}