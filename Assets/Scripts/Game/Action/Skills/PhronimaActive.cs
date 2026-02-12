using MemoryPack;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class PhronimaActive : Action, ISkills
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = PieceOn(Maker);
            if (maker == null || pieceAI == null) return 0;
            if (pieceAI.Color != maker.Color) return -50;
            return 0;
        }

        private const int increaseDuration = 2;
        public PhronimaActive(int from, int to) : base(from)
        {
            Target = to;
        }

        private void ApplyEffect(int pos)
        {
            var targetPiece = PieceOn(Target);

            foreach (var effect in targetPiece.Effects)
            {
                //áp dụng cho debuff và chỉ tăng duration nếu nó là hữu hạn
                if (effect.Category == Effects.EffectCategory.Debuff && effect.Duration > 0)
                {
                    effect.Duration += increaseDuration;
                } else if (effect.Category == Effects.EffectCategory.Debuff)
                {
                    Debug.Log($"[PhronimaActive] Effect {effect.EffectName} on piece {targetPiece.Type} at position {Target} has infinite duration, skipping duration increase.");
                }
            }
        }
        protected override void ModifyGameState()
        {
            ApplyEffect(Target);
        }

       
    }
}
