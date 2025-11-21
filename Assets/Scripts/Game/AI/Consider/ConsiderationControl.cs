using System.Collections.Generic;
using Game.Action;
using Game.Action.Quiets;
using UnityEngine;

namespace Game.AI.Consider
{
    [CreateAssetMenu(menuName = "AI/Considerations/Control")]
    public class ControlConsiderationSO : ConsiderationSO
    {
        public override float Score(Action.Action action, List<Action.Action> allyActions, List<Action.Action> enemyActions, int weight)
        {
            if (action == null) return 0f;
            return action is IQuiets ? 1f * weight : 0f;
        }
    }
}
