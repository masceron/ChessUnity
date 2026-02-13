using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Traits;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Effects.Augmentation
{
    public class RibcagePassive : Effect, IStartTurnTrigger
    {
        public RibcagePassive(int duration, int strength, PieceLogic piece) : base(duration, strength, piece,
            "effect_ribcage_passive")
        {
            StartTurnEffectType = StartTurnEffectType.StartOfAllyTurn;
        }

        public StartTurnTriggerPriority Priority => StartTurnTriggerPriority.Buff;

        public StartTurnEffectType StartTurnEffectType { get; }
        public void OnCallStart(Action.Action lastMainAction)
        {
            var (rank, file) = RankFileOf(Piece.Pos);
            
            foreach (var (r, f) in MoveEnumerators.AroundUntil(rank, file, 3))
            {
                var pOn = PieceOn(IndexOf(r, f));
                if (pOn == null || pOn.Color != Piece.Color) continue; 
                ActionManager.EnqueueAction(new ApplyEffect(new LongReach(Piece, 1, 1)));
            }
           
        }
    }
}