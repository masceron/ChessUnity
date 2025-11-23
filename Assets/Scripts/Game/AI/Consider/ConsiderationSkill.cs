using System.Collections.Generic;
using Game.Action.Skills;
using UnityEngine;
using Game.Piece.PieceLogic.Commons;

namespace Game.AI.Consider
{
    [CreateAssetMenu(menuName = "AI/Considerations/Skill")]
    public class SkillConsiderationSO : ConsiderationSO
    {
        public override float Score(Action.Action action, List<Action.Action> allyActions, List<Action.Action> enemyActions, int weight, PieceLogic maker)
        {
            if (action == null) return 0f;
            return action is ISkills ? 1f * weight : 0f;
        }
    }
}
