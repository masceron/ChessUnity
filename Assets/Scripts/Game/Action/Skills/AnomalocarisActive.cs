using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class AnomalocarisActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private AnomalocarisActive()
        {
        }

        public AnomalocarisActive(int maker, int target) : base(maker)
        {
            Maker = maker;
            Target = target;
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            if (PieceOn(Maker) == null || pieceAI == null) return 0;
            if (pieceAI.Color != PieceOn(Maker).Color) return -10;
            return 0;
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            ActionManager.EnqueueAction(new ApplyEffect(new Bound(1, PieceOn(Target)), PieceOn(Maker)));
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}