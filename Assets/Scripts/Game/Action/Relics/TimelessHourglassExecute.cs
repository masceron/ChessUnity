using MemoryPack;
using Game.Common;

namespace Game.Action.Relics
{
    [MemoryPackable]
    public partial class TimelessHourglassExecute : Action, IRelicAction
    {
        [MemoryPackInclude]
        private readonly bool _relicColor;
        [MemoryPackInclude]
        private readonly bool _targetColor;
        public TimelessHourglassExecute(int maker, bool relicColor, int target, bool targetColor = false) : base(maker)
        {
            _relicColor = relicColor;
            Target = target;
            _targetColor = BoardUtils.PieceOn(target).Color;
        }

        protected override void ModifyGameState()
        {
            if(_targetColor == _relicColor)
            {
                BoardUtils.PieceOn(Target).SkillCooldown = UnityEngine.Mathf.Max(0, BoardUtils.PieceOn(Target).SkillCooldown);
            }
            else
            {
                BoardUtils.PieceOn(Target).SkillCooldown += 2;
            }
        }
    }
}
