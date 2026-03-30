using System.Collections.Generic;
using Game.Action.Captures;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;

namespace Game.AI.Consider
{
    [CreateAssetMenu(menuName = "AI/Considerations/Capture")]
    public class CaptureConsiderationSO : ConsiderationSO
    {
        public override float Score(Action.Action action, List<Action.Action> allyActions,
            List<Action.Action> enemyActions, int weight, PieceLogic maker)
        {
            if (action is not ICaptures) return 0f;

            var targetPiece = action.GetTargetAsPiece();
            if (targetPiece == null) return 0f;

            try
            {
                var value = targetPiece.GetValueForAI();
                return (float)value + weight;
            }
            catch
            {
                return 0f;
            }
        }
    }
}