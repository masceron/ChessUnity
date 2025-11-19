using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;

namespace Game.Piece.PieceLogic
{
    public class LivingCoral : Commons.PieceLogic
    {
        public LivingCoral(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new LivingCoralPassive(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new Construct(this)));
        }
    }
}
