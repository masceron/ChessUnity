using Game.Action;
using Game.Action.Internal;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Others
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PureMinded: Effect, IEndTurnTrigger
    {
        private const int RoundsPerSelfPurify = 5;
        private int _count;
        public PureMinded(PieceLogic piece) : base(-1, 1, piece, "effect_pure_minded")
        {   
            _count = RoundsPerSelfPurify;
        }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            if (_count > 0) _count--;
            if (_count == 0) {
                ActionManager.EnqueueAction(new Purify(Piece.Pos, Piece.Pos));
                _count = RoundsPerSelfPurify;
            }
        }

        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Buff;

        public EndTurnEffectType EndTurnEffectType => EndTurnEffectType.EndOfEnemyTurn;

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 10;
        }
    
    }
}