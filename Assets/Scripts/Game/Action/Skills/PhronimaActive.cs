using Game.Common;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    public class PhronimaActive : Action, ISkills
    {
        public int AIPenaltyValue => PieceOn(Target).Color != PieceOn(Maker).Color ? -50 : 0;

        private const int increaseDuration = 2;
        public PhronimaActive(int from, int to) : base(from)
        {
            Target = to;
        }

        protected override void ModifyGameState()
        {
            var targetPiece = BoardUtils.PieceOn(Target);

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
    }
}
