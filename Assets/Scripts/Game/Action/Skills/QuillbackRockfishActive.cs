using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Effects.States;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class QuillbackRockfishActive : Action, ISkills
    {

        [MemoryPackConstructor]
        private QuillbackRockfishActive()
        {
        }

        public QuillbackRockfishActive(PieceLogic maker, PieceLogic targetEnemy) : base(maker, targetEnemy)
        {
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return -25;
        }

        protected override void ModifyGameState()
        {
            var makerPiece = GetMakerAsPiece();
            var targetEnemy = GetTargetAsPiece();
            if (makerPiece == null || targetEnemy == null) return;
            if (targetEnemy.Color == makerPiece.Color) return;

            var bleedingStack = makerPiece.GetStat(SkillStat.Stack);
            ActionManager.EnqueueAction(new ApplyEffect(new Bleeding(bleedingStack, targetEnemy), makerPiece));
            ActionManager.EnqueueAction(new ApplyEffect(new Petrified(-1, makerPiece), makerPiece));
            ActionManager.EnqueueAction(new CooldownSkill(makerPiece));
        }
    }
}

