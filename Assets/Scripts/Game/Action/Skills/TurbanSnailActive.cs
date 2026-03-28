using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class TurbanSnailActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private TurbanSnailActive()
        {
        }

        public TurbanSnailActive(int maker) : base(maker)
        {
        }

        public int AIPenaltyValue(PieceLogic pieceAI)
        {
            return 0;
        }

        protected override void ModifyGameState()
        {
            PieceLogic p = GetMaker();
            ActionManager.EnqueueAction(new ApplyEffect(new Shield(p, p.GetStat(SkillStat.Duration, 1))));
            ActionManager.EnqueueAction(new ApplyEffect(new Pacified(p.GetStat(SkillStat.Duration, 2), p)));
            SetCooldown(GetMaker(), ((IPieceWithSkill)GetMaker()).TimeToCooldown);
        }
    }
}