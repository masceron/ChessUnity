using System.Collections.Generic;
using Game.Action.Captures;
using Game.Common;
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
            if (!(action is ICaptures)) return 0f;

            var targetPiece = BoardUtils.PieceOn(action.Target);
            if (targetPiece == null) return 0f;

            try
            {
                var value = (float)targetPiece.GetValueForAI();
                return value + weight;
            }
            catch
            {
                return 0f;
            }
        }
    }
}