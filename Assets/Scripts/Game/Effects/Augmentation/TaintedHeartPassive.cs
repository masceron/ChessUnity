using Game.Action.Internal;
using Game.Common;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class TaintedHeartPassive : Effect
    {
        public TaintedHeartPassive(PieceLogic piece) : base(-1, 1, piece, "effect_tainted_heart_passive")
        {
        }
        
        
    }
}