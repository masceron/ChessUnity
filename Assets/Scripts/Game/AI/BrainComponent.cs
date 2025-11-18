using System.Collections.Generic;
using System.Linq;
using Game.Action;
using Game.Common;
using Game.Managers;
using UnityEngine;

namespace Game.AI
{
    // Per-piece Brain component. Assign a BrainConfig in inspector and set MakerIndex (board index).
    public class BrainComponent : MonoBehaviour
    {
        [Tooltip("Configurable list of considerations + weights")]
        public BrainConfig Config;

        [Tooltip("Maker index for the piece this Brain controls (board index)")]
        public int MakerIndex = -1;

        // Evaluate action as weighted average of configured considerations.
        public float Evaluate(Action.Action action, List<Action.Action> allyActions, List<Action.Action> enemyActions)
        {
            if (Config == null || Config.Considerations == null || action == null) return float.NegativeInfinity;

            float weightedSum = 0f;
            float weightTotal = 0f;
            foreach (var cw in Config.Considerations)
            {
                if (cw.Consideration == null || cw.Weight <= 0f) continue;
                float s = Mathf.Clamp01(cw.Consideration.Score(action, allyActions, enemyActions));
                weightedSum += s * cw.Weight;
                weightTotal += cw.Weight;
            }

            if (weightTotal <= 0f) return 0f;
            return weightedSum / weightTotal;
        }

        // Choose best action from a given list using this BrainConfig.
        public List<Action.Action> ChooseBest(List<Action.Action> actions, List<Action.Action> enemyActions = null)
        {
            if (actions == null || actions.Count == 0) return null;

            var allyActions = actions;
            float bestScore = float.NegativeInfinity;

            // Danh sách các action tốt nhất
            List<Action.Action> bestList = new List<Action.Action>();

            foreach (var a in actions)
            {
                if (MakerIndex >= 0 && a.Maker != MakerIndex) continue;

                float score = Evaluate(a, allyActions, enemyActions);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestList.Clear();
                    bestList.Add(a);
                }
                else if (Mathf.Approximately(score, bestScore))
                {
                    bestList.Add(a);
                }
            }

            return bestList;
        }


        // Gather actions for the piece this brain controls (calls Piece.MoveList)
        public List<Action.Action> GatherActionsForMaker()
        {
            var list = new List<Action.Action>();
            if (!BoardUtils.VerifyIndex(MakerIndex)) return list;
            var piece = BoardUtils.PieceOn(MakerIndex);
            if (piece == null) return list;
            try { piece.MoveList(list); } catch { }
            return list;
        }
    }
}
