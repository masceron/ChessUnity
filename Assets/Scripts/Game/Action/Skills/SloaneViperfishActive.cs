using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [MemoryPackable]
    public partial class SloaneViperfishActive : Action, ISkills
    {
        private const int PoisonStack = 1;
        private const int BleedingTurnToDie = 5;
        
        [MemoryPackConstructor]
        private SloaneViperfishActive()
        {
        }

        public SloaneViperfishActive(PieceLogic maker, PieceLogic target) : base(maker, target)
        {
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            var maker = GetMakerAsPiece();
            if (maker == null || pieceAI == null) return 0;
            if (pieceAI.Color != maker.Color) return -25;
            return 0;
        }

        protected override void ModifyGameState()
        {
            var targetPiece = GetTargetAsPiece();
            var makerPiece = GetMakerAsPiece();
            
            ActionManager.EnqueueAction(targetPiece.Effects.Any(e => e.EffectName == "effect_bleeding")
                ? new ApplyEffect(new Poison(PoisonStack, targetPiece), makerPiece)
                : new ApplyEffect(new Bleeding(BleedingTurnToDie, targetPiece), makerPiece));
            ActionManager.EnqueueAction(new CooldownSkill(makerPiece));
        }
    }
}