using Game.Effects;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class PhronimaActive : Action, ISkills
    {
        private const int increaseDuration = 2;

        [MemoryPackConstructor]
        private PhronimaActive()
        {
        }

        public PhronimaActive(int from, int to) : base(from)
        {
            Target = to;
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = GetMaker();
            if (maker == null || pieceAI == null) return 0;
            if (pieceAI.Color != maker.Color) return -50;
            return 0;
        }

        private void ApplyEffect(int pos)
        {
            var targetPiece = GetTarget();

            foreach (var effect in targetPiece.Effects)
                //áp dụng cho debuff và chỉ tăng duration nếu nó là hữu hạn
                if (effect.Category == EffectCategory.Debuff && effect.Duration > 0)
                    effect.Duration += increaseDuration;
                else if (effect.Category == EffectCategory.Debuff)
                    Debug.Log(
                        $"[PhronimaActive] Effect {effect.EffectName} on piece {targetPiece.Type} at position {Target} has infinite duration, skipping duration increase.");
        }

        protected override void ModifyGameState()
        {
            ApplyEffect(Target);
        }
    }
}