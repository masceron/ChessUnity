using Game.Action;
using Game.Action.Relics;
using Game.Common;
using UnityEngine;

public class TimelessHourglassExcute : Action, IRelicAction
{
    private bool relicColor;
    private bool targetColor;
    public TimelessHourglassExcute(int maker, bool relicColor, int target) : base(maker)
    {
        this.relicColor = relicColor;
        this.Target = target;
        this.targetColor = BoardUtils.PieceOn(target).Color;
    }

    protected override void ModifyGameState()
    {
         if(targetColor == relicColor)
            {
                BoardUtils.PieceOn(Target).SkillCooldown = (sbyte)UnityEngine.Mathf.Max(0, BoardUtils.PieceOn(Target).SkillCooldown);
            }
            else
            {
                BoardUtils.PieceOn(Target).SkillCooldown += 2;
            }
    }
}
