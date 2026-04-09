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

        public TimelessHourglassExecute(bool relicColor, int target) : base(null, target)
        {
            _relicColor = relicColor;
            _targetColor = GetTargetAsPiece().Color;
        }

        protected override void ModifyGameState()
        {
            var target = GetTargetAsPiece();
            if (_targetColor == _relicColor)
                target.SkillCooldown = Mathf.Max(0, target.SkillCooldown);
            else
                target.SkillCooldown += 2;
        }
    }
}