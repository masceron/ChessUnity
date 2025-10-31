using Game.Action; 
using Game.Action.Internal;
using Game.Effects.Traits;

namespace Game.Piece.PieceLogic.Construct
{
    public class DormantFossil : PieceLogic
    {
        public DormantFossil(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new DormantFossilPassive(this)));
        }
    }
}