using Game.Action.Internal;
using Game.Effects;
using Game.Piece.PieceLogic.Commons;
using MemoryPack;
using ZLinq;
using static Game.Common.BoardUtils;

namespace Game.Action.Skills
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [MemoryPackable]
    public partial class PlumedSeaFirActive : Action, ISkills
    {
        [MemoryPackConstructor]
        private PlumedSeaFirActive()
        {
        }

        public PlumedSeaFirActive(int maker, int target) : base(maker, target)
        {
        }

        public int AIPenaltyValue(PieceLogic p)
        {
            return 0;
        }

        protected override void Animate()
        {
        }

        protected override void ModifyGameState()
        {
            SetCooldown(GetMaker(), ((IPieceWithSkill)GetMaker()).TimeToCooldown);
            PieceLogic maker = GetMaker();
            PieceLogic target = GetTarget();
            if (maker.Color == target.Color)
            {
                var debuffs = maker.Effects.Where(kvp => kvp.Category == EffectCategory.Debuff).ToArray();
                if (debuffs.Length > 0)
                {
                    var random = new System.Random();
                    ActionManager.EnqueueAction(new RemoveEffect(debuffs[random.Next(debuffs.Length)]));
                }
                maker.SetStat(SkillStat.Counter, maker.GetStat(SkillStat.Counter) + 1);
            }
            else
            {
                EffectFactory.CreateRandomEffect(GetTarget());
                maker.SetStat(SkillStat.Counter, maker.GetStat(SkillStat.Counter) - 1);
            }
            
        }
    }
}