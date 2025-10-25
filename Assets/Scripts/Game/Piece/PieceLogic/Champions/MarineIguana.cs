using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Action.Captures;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Game.Piece.PieceLogic.Champions
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MarineIguana: PieceLogic, IPieceWithSkill
    {
        private bool isActiveSkill;
        private readonly System.Func<bool> getIsActiveSkill;
        private readonly System.Action<bool> setIsActiveSkill;
        public MarineIguana(PieceConfig cfg) : base(cfg, BluffingMoves.Quiets, BluffingMoves.Captures)
        {
            isActiveSkill = false;
            getIsActiveSkill = () => isActiveSkill;
            setIsActiveSkill = (v) => isActiveSkill = v;
            Skills = list =>
            {
                if (SkillCooldown != 0) return;
                if (getIsActiveSkill()) return;
                list.Add(new MarineIguanaActive(this.Pos, setIsActiveSkill));
            };
        }

        protected override void CustomBehaviors(List<Action.Action> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i] is ICaptures)
                    list[i] = new MarinelGuanaAttack(Pos, list[i].Target, getIsActiveSkill, setIsActiveSkill);
            }
        }
        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}