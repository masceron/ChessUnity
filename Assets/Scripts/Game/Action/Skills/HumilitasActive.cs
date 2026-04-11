using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class HumilitasActive : Action, ISkills
    {
        private readonly int _firstTarget;
        private readonly int _secondTarget;

        [MemoryPackConstructor]
        private HumilitasActive()
        {
        }

        public HumilitasActive(PieceLogic maker, int firstTarget, int secondTarget) : base(maker)
        {
            _firstTarget = firstTarget;
            _secondTarget = secondTarget;
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            Debug.Log("Executing HumilitasActive");
            var first = PieceOn(_firstTarget);
            var second = PieceOn(_secondTarget);
            if (first != null)
                ActionManager.EnqueueAction(new ApplyEffect(new Taunted(2, first), GetMakerAsPiece()));
            if (second != null)
                ActionManager.EnqueueAction(new ApplyEffect(new Taunted(2, second), GetMakerAsPiece()));
            ActionManager.EnqueueAction(new CooldownSkill(GetMakerAsPiece()));
        }
    }
}