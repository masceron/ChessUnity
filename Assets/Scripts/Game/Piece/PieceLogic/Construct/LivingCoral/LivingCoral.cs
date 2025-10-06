using Game.Action; 
using Game.Action.Internal; 

namespace Game.Piece.PieceLogic.Construct.LivingCoral
{
    public class LivingCoral : PieceLogic
    {
        public LivingCoral(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new LivingCoralPassive(this)));
        }
    }
}
