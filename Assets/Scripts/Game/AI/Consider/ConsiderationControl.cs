using System.Collections.Generic;
using Game.Action.Quiets;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;

namespace Game.AI.Consider
{
    [CreateAssetMenu(menuName = "AI/Considerations/Control")]
    public class ControlConsiderationSO : ConsiderationSO
    {
        public override float Score(Action.Action action, List<Action.Action> allyActions, List<Action.Action> enemyActions, int weight, PieceLogic maker)
        {
            if (action == null) return 0f;

            return action is IQuiets ? 1f * weight + TileManager.Ins.GetTileValue(action.Target) : 0f;
        }
    }
}
