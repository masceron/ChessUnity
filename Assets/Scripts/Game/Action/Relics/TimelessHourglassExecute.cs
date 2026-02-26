using Game.Common;
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

        public TimelessHourglassExecute(int maker, bool relicColor, int target, bool targetColor = false) : base(maker)
        {
            _relicColor = relicColor;
            Target = target;
            _targetColor = BoardUtils.PieceOn(target).Color;
        }

        protected override void ModifyGameState()
        {
            if (_targetColor == _relicColor)
                BoardUtils.PieceOn(Target).SkillCooldown = Mathf.Max(0, BoardUtils.PieceOn(Target).SkillCooldown);
            else
                BoardUtils.PieceOn(Target).SkillCooldown += 2;
        }
    }
}