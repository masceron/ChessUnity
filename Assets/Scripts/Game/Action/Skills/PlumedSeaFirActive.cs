using System.Collections.Generic;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Effects;
using Game.Effects.Debuffs;
using Game.Managers;
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

        public PlumedSeaFirActive(int maker, int target) : base(maker)
        {
            Target = target;
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
            SetCooldown(Maker, ((IPieceWithSkill)PieceOn(Maker)).TimeToCooldown);
            PieceLogic maker = PieceOn(Maker);
            PieceLogic target = PieceOn(Target);
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
                var debuffs = AssetManager.Ins.EffectData
                    .Where(kvp => kvp.Value.category == EffectCategory.Debuff)
                    .Select(kvp => kvp.Key)
                    .ToArray();
                    
                var random = new System.Random();
                var selectedEffectName = debuffs[random.Next(debuffs.Length)];
                EffectFactory.CreateEffect(selectedEffectName, 5, 1, PieceOn(Target));
                maker.SetStat(SkillStat.Counter, maker.GetStat(SkillStat.Counter) - 1);
            }
            
        }
    }
}