using Game.AI;
using Game.Common;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    public class PhronimaActive : Action, ISkills, IAIAction
    {
        private const int increaseDuration = 2;
        public PhronimaActive(int from, int to) : base(from)
        {
            Target = to;
        }

        private void ApplyEffect(int pos)
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
        protected override void ModifyGameState()
        {
            ApplyEffect(Target);
        }

        public void CompleteActionForAI()
        {
            var listPieces = GetPiecesInRadius(RankOf(Maker), FileOf(Maker), 3, p => p != null && p.Color != PieceOn(Maker).Color);
            if (listPieces.Count == 0) return;
            
            listPieces.Sort((a, b) =>
            {
                int buffCountA = 0, buffCountB = 0;
                foreach (var effect in a.Effects)
                {
                    if (effect.Category == Effects.EffectCategory.Buff)
                        buffCountA++;
                }

                foreach (var effect in b.Effects)
                {
                    if (effect.Category == Effects.EffectCategory.Buff)
                        buffCountB++;
                }

                return buffCountA.CompareTo(buffCountB);
            });

            var idx = UnityEngine.Random.Range(0, listPieces.Count - 1);
            
            ApplyEffect(listPieces[idx].Pos);
        }
    }
}

