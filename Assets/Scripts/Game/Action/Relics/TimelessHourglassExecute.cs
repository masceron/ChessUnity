using MemoryPack;
using UnityEngine;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class TimelessHourglassExecute : Action, IRelicAction
    {
        [MemoryPackInclude] private bool _relicColor;

        [MemoryPackInclude] private bool _targetColor;

        [MemoryPackConstructor]
        private TimelessHourglassExecute()
        {
        }

        public TimelessHourglassExecute(int maker, bool relicColor, int target) : base(maker, target)
        {
            _relicColor = relicColor;
            _targetColor = GetTarget().Color;
        }

        protected override void ModifyGameState()
        {
            if (_targetColor == _relicColor)
                GetTarget().SkillCooldown = Mathf.Max(0, GetTarget().SkillCooldown);
            else
                GetTarget().SkillCooldown += 2;
        }
    }
}