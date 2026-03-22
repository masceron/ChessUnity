using Game.Action;
using Game.Action.Internal;
using Game.Effects.SpecialAbility;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BrainCoral : Commons.PieceLogic
    {
        public BrainCoral(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new BrainCoralPassive(this)));
        }
        
    }
}