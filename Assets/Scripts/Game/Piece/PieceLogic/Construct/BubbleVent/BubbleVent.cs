using Game.Action;
using Game.Movesets;
using Game.Action.Internal;

namespace Game.Piece.PieceLogic.Construct
{
    public class BubbleVent : PieceLogic
    {
        public BubbleVent(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new BubbleVentPassive(this)));
        }
    }
}

