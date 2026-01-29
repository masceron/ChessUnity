using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Game.Effects.Debuffs;
using Game.Common;
using Game.Action.Internal;
using Game.Piece.PieceLogic;
namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class DwarfLionfishActive: Action, ISkills
    {
        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return -30;
        }
        public DwarfLionfishActive(int maker, int target) : base(maker)
        {
            Maker = (ushort)maker;
            Target = (ushort)target;
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