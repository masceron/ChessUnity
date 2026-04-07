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
            ActionManager.EnqueueAction(GetTargetAsPiece().Effects.Any(e => e.EffectName == "effect_bleeding")
                ? new ApplyEffect(new Poison(1, GetTargetAsPiece()), GetMakerAsPiece())
                : new ApplyEffect(new Bleeding(5, GetTargetAsPiece()), GetMakerAsPiece()));
            ActionManager.EnqueueAction(new CooldownSkill(GetMakerAsPiece()));
        }
    }
}