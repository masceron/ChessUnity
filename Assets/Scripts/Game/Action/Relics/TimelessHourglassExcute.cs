using Game.Common;

namespace Game.Action.Relics
{
    public class TimelessHourglassExcute : Action, IRelicAction
    {
        private readonly bool relicColor;
        private readonly bool targetColor;
        public TimelessHourglassExcute(int maker, bool relicColor, int target) : base(maker)
        {
            this.relicColor = relicColor;
            Target = target;
            targetColor = BoardUtils.PieceOn(target).Color;
        }

        protected override void ModifyGameState()
        {
            if(targetColor == relicColor)
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
