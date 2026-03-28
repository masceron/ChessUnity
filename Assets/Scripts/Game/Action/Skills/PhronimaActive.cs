using Game.Effects;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using UnityEngine;
using ZLinq;
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

        public PhronimaActive(int from, int to) : base(from, to)
        {
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

            foreach (var effect in targetPiece.Effects.Where(effect => effect.Category == EffectCategory.Debuff && effect.Duration > 0))
                effect.Duration += increaseDuration;
        }

        protected override void ModifyGameState()
        {
            ApplyEffect(GetTargetPos());
        }
    }
}