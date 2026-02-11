using Game.Action;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
namespace Game.Effects.Others
{
    public class TemporalWarpReturn : Effect, IEndTurnEffect
    {
        private const int TurnToActive = 4;
        private int turnsPassed = 0;
        private readonly int Target;
        public TemporalWarpReturn(PieceLogic piece, int target) : base(12, 1, piece, "effect_temporal_warp_return")
        {
            Target = target;
            EndTurnEffectType = EndTurnEffectType.EndOfEnemyTurn;
        }

        public EndTurnEffectType EndTurnEffectType { set; get; }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            turnsPassed++;
            if (turnsPassed >= TurnToActive)
            {
                if (PieceOn(Target) != null) 
                {
                    ActionManager.EnqueueAction(new KillPiece(Target));
                }

                ActionManager.EnqueueAction(new NormalMove(Piece.Pos, Target));
                Duration = 0;
            }
        }
    }
}