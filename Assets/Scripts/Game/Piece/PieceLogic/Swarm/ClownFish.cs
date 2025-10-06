
using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Movesets;

namespace Game.Piece.PieceLogic.Swarm
{
    public class ClownFish : PieceLogic
    {
        private (int, int)[] anotherPieceAt = new (int, int)[8]
        {
            (2, 0), (2, 2), (0, 2), (-2, 2),
            (-2, 0), (-2, -2), (0, -2), (2, -2)
        };
        public ClownFish(PieceConfig cfg) : base(cfg, BishopMoves.Quiets, BishopMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Demolisher(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new ClownFishPassive(this)));
        }
        
    }
}