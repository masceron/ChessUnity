using System.Collections.Generic;
using Game.Action;
using Game.Action.Skills;
using UnityEngine;

namespace Game.AI
{
    [CreateAssetMenu(menuName = "AI/Considerations/Skill")]
    public class SkillConsiderationSO : ConsiderationSO
    {
        // Score 1.0 for skill actions, 0 otherwise.
        public override float Score(Action.Action action, List<Action.Action> allyActions, List<Action.Action> enemyActions)
        {
            if (action == null) return 0f;
            return action is ISkills ? 1f : 0f;
        }
    }
}
