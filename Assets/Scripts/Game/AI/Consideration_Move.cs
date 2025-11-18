using System.Collections.Generic;
using Game.Action;
using Game.Action.Quiets;
using UnityEngine;

namespace Game.AI
{
    [CreateAssetMenu(menuName = "AI/Considerations/Move")]
    public class MoveConsiderationSO : ConsiderationSO
    {
        // Score 1.0 for move (quiet) actions, 0 otherwise
        public override float Score(Action.Action action, List<Action.Action> allyActions, List<Action.Action> enemyActions)
        {
            if (action == null) return 0f;
            return action is IQuiets ? 1f : 0f;
        }
    }
}
