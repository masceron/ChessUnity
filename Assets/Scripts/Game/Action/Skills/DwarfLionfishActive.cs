using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class DwarfLionfishActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private DwarfLionfishActive()
        {
        }

        public DwarfLionfishActive(PieceLogic maker) : base(maker)
        {
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return -30;
        }

        protected override void ModifyGameState()
        {
            var targets = SkillRangeHelper.GetActiveEnemyPieceInRadius(GetFrom(), 1);
            foreach (var target in targets)
                ActionManager.EnqueueAction(new ApplyEffect(new Bleeding(5, PieceOn(target))));
            SetCooldown(GetMaker() as PieceLogic, ((IPieceWithSkill)GetMaker()).TimeToCooldown);
        }
    }
}