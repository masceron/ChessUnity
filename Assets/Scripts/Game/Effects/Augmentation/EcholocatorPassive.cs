using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Game.Effects.Debuffs;
using Game.Common;
using Game.Effects.Triggers;

namespace Game.Effects.Augmentation
{
    public class EcholocatorPassive : Effect, IEndTurnTrigger
    {
        private byte _lastUsed; 
        private const byte TurnsToActive = 3;

        public EcholocatorPassive(PieceLogic piece) : base(-1, 1, piece, "effect_echolocator_passive")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfEnemyTurn;
        }

        public void OnCallEnd(Action.Action action)
        {
            if (action.Maker != Piece.Pos || !Piece.Effects.Any(e => e.EffectName == "effect_ambush")) return;
            
            _lastUsed++;
            if (_lastUsed < TurnsToActive) return;
            MakeActive();
            _lastUsed = 0;
            
        }

        private void MakeActive()
        {
            var listPieces = SkillRangeHelper.GetActiveEnemyPieceInRadius(Piece.Pos, 4);
            foreach (var piece in listPieces)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Marked(1, PieceOn(piece))));
            }
        }

        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Buff;

        public EndTurnEffectType EndTurnEffectType { get; }
    }
}