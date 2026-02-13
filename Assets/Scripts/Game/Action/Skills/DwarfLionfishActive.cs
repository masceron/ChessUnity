using MemoryPack;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Game.Effects.Debuffs;
using Game.Common;
using Game.Action.Internal;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class DwarfLionfishActive: Action, ISkills
    {
        [MemoryPackConstructor]
        private DwarfLionfishActive() { }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return -30;
        }
        public DwarfLionfishActive(int maker) : base(maker)
        {
            Maker = maker;
            Target = maker;
        }
        protected override void ModifyGameState()
        {
            var targets = SkillRangeHelper.GetActiveEnemyPieceInRadius(Maker, 1);
            foreach (var target in targets)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Bleeding(5, PieceOn(target))));
            }
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
        }
    }
}   