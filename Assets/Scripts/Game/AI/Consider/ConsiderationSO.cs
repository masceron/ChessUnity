using System.Collections.Generic;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;

namespace Game.AI.Consider
{
    // Base consideration ScriptableObject. Scores in [0,1].
    public abstract class ConsiderationSO : ScriptableObject
    {
        // Implement scoring logic using only read-only game state access.
        public abstract float Score(Action.Action action, List<Action.Action> allyActions, List<Action.Action> enemyActions, int weight, PieceLogic maker);
    }
}
