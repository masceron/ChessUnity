using Game.Action.Internal;
using Game.Effects;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using ZLinq;

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

        public PhronimaActive(PieceLogic from, PieceLogic to) : base(from, to)
        {
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = GetMakerAsPiece();
            if (maker == null || pieceAI == null) return 0;
            if (pieceAI.Color != maker.Color) return -50;
            return 0;
        }

        private void ApplyEffect()
        {
            var targetPiece = GetTargetAsPiece();

            foreach (var effect in targetPiece.Effects.Where(effect => effect.Category == EffectCategory.Debuff && effect.Duration > 0))
                effect.Duration += increaseDuration;
        }

        protected override void ModifyGameState()
        {
            ApplyEffect();
            ActionManager.EnqueueAction(new CooldownSkill(GetMakerAsPiece()));
        }
    }
}