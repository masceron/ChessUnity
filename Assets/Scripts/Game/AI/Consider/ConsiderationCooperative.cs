using System.Collections.Generic;
using Game.Action;
using Game.Action.Quiets;
using UnityEngine;
using Game.Piece.PieceLogic.Commons;

namespace Game.AI.Consider
{
    [CreateAssetMenu(menuName = "AI/Considerations/Cooperative")]
    public class CooperativeConsiderationSO : ConsiderationSO
    {
        public override float Score(Action.Action action, List<Action.Action> allyActions, List<Action.Action> enemyActions, int weight, PieceLogic maker)
        {
            if (action == null) return 0f;
            return action is IQuiets ? 1f * weight : 0f;
        }
    }
}
