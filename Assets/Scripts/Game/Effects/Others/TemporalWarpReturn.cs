using Game.Action;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
namespace Game.Effects.Others
{
    public class TemporalWarpReturn : Effect, IEndTurnTrigger
    {
        private const int TurnToActive = 4;
        private int _turnsPassed;
        private readonly int _target;
        public TemporalWarpReturn(PieceLogic piece, int target) : base(12, 1, piece, "effect_temporal_warp_return")
        {
            _target = target;
            EndTurnEffectType = EndTurnEffectType.EndOfEnemyTurn;
        }

        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Other;

        public EndTurnEffectType EndTurnEffectType { set; get; }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            _turnsPassed++;
            if (_turnsPassed >= TurnToActive)
            {
                if (PieceOn(_target) != null) 
                {
                    ActionManager.EnqueueAction(new KillPiece(_target));
                }

                ActionManager.EnqueueAction(new NormalMove(Piece.Pos, _target));
                Duration = 0;
            }
        }
    }
}