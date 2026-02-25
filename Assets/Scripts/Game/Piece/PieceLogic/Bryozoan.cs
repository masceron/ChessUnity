
using Game.Action;
using Game.Action.Internal;
using Game.Effects.Others;

namespace Game.Piece.PieceLogic
{
    public class Bryozoan : Commons.PieceLogic
    {
        public Bryozoan(PieceConfig cfg) : base(cfg)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new BryozoanPassive(this)));
        }
        
    }
}