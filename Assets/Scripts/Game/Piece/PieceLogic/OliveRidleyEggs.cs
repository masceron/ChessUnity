using Game.Action;
using Game.Action.Internal;
using Game.Effects.SpecialAbility;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class OliveRidleyEggs : Commons.PieceLogic
    {
        public OliveRidleyEggs(PieceConfig cfg)
            : base(
                cfg,
                (list, pos, isPlayer) => 0,                
                (list, pos, excludeEmptyTile) => 0
            )   
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new OliveRidleyEggsPassive(this)));
        }
    }
}