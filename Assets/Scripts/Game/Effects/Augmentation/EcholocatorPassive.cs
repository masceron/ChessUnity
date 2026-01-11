using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Effects.Buffs;
using System.Linq;
using static Game.Common.BoardUtils;
using Game.Effects.Debuffs;
using Game.Common;
namespace Game.Effects.Augmentation
{
    public class EcholocatorPassive : Effect, IEndTurnEffect
    {
        private byte lastUsed;
        private const byte TurnsToActive = 3;

        public EcholocatorPassive(PieceLogic piece) : base(-1, 1, piece, "effect_echolocator_passive")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfEnemyTurn;
        }

        public void OnCallEnd(Action.Action action)
        {
            if (action.Maker != Piece.Pos || !Piece.Effects.Any(e => e.EffectName == "effect_ambush")) return;
            
            lastUsed++;
            if (lastUsed < TurnsToActive) return;
            MakeActive();
            lastUsed = 0;
            
        }

        private void MakeActive()
        {
            foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(RankOf(Piece.Pos), FileOf(Piece.Pos), 4))
            {
                var index = IndexOf(rankOff, fileOff);
                var pOn = PieceOn(index);
                if (pOn == null || pOn.Color == Piece.Color) continue;
                ActionManager.EnqueueAction(new ApplyEffect(new Marked(1, pOn)));
            }
        }
        public EndTurnEffectType EndTurnEffectType { get; }
    }
}