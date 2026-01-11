using System.Linq;
using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Effects.Augmentation
{
    public class RibcagePassive : Effect, IStartTurnEffect
    {
        public RibcagePassive(sbyte duration, sbyte strength, PieceLogic piece) : base(duration, strength, piece,
            "effect_ribcage_passive")
        {
            StartTurnEffectType = StartTurnEffectType.StartOfAllyTurn;
        }
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