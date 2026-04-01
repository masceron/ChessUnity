using Game.Action;
using Game.Action.Internal;
using Game.Effects.SpecialAbility;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PulseCoral : Commons.PieceLogic
    {
        public PulseCoral(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new PulseCoralPassive(this)));
        }
        
    }
}