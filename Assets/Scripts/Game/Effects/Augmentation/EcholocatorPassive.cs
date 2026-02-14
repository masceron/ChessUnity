using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.Augmentation
{
    public class EcholocatorPassive : Effect, IEndTurnTrigger
    {
        private const byte TurnsToActive = 3;
        private byte _lastUsed;

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

        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Buff;

        public EndTurnEffectType EndTurnEffectType { get; }

        private void MakeActive()
        {
            var listPieces = SkillRangeHelper.GetActiveEnemyPieceInRadius(Piece.Pos, 4);
            foreach (var piece in listPieces)
                ActionManager.EnqueueAction(new ApplyEffect(new Marked(1, PieceOn(piece))));
        }
    }
}