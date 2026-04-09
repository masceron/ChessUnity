using Game.Action;
using Game.Action.Internal;
using Game.Effects.SpecialAbility;
using Game.Movesets;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class OliveRidleyEggs : Commons.PieceLogic
    {
        public OliveRidleyEggs(PieceConfig cfg)
            : base(
                cfg,
                None.Quiets,
                None.Captures
            )   
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new OliveRidleyEggsPassive(this)));
        }
    }
}