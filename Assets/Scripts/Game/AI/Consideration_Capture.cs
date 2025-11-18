using System.Collections.Generic;
using System.Linq;
using Game.Action;
using Game.Action.Captures;
using Game.Common;
using UnityEngine;

namespace Game.AI
{
    [CreateAssetMenu(menuName = "AI/Considerations/Capture")]
    public class CaptureConsiderationSO : ConsiderationSO
    {
        // Score based on captured piece value normalized to [0,1]
        public override float Score(Action.Action action, List<Action.Action> allyActions, List<Action.Action> enemyActions)
        {
            if (action == null) return 0f;
            if (!(action is ICaptures)) return 0f;

            var targetPiece = BoardUtils.PieceOn(action.Target);
            if (targetPiece == null) return 0f;

            try
            {
                // Prefer using PieceRank (or map types to values via AssetManager)
                float value = Mathf.Max(1f, (int)targetPiece.PieceRank);
                // normalize approx: expect rank in 1..10 -> map to 0..1
                return Mathf.Clamp01(value / 10f);
            }
            catch
            {
                return 0.5f;
            }
        }
    }
}
