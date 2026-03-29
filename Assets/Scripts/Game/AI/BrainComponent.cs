using System.Collections.Generic;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;

namespace Game.AI
{
    // Per-piece Brain component. Set Maker (the piece this brain controls) in the inspector.
    // AI behavior is determined by considerations defined in the piece's PieceInfo asset.
    public class BrainComponent : MonoBehaviour
    {
        [Tooltip("The piece this Brain controls")]
        public PieceLogic Maker;

        // Evaluate action as weighted average of configured considerations.
        public float Evaluate(Action.Action action, List<Action.Action> allyActions, List<Action.Action> enemyActions)
        {
            if (Maker == null || action == null) return float.NegativeInfinity;
            var pieceInfo = AssetManager.Ins.PieceData[Maker.Type];

            var bestScoreConsideration = float.NegativeInfinity;

            foreach (var cw in pieceInfo.considerations)
            {
                if (cw.Consideration == null) continue;
                var score = cw.Consideration.Score(action, allyActions, enemyActions, cw.Weight, Maker);
                if (score > bestScoreConsideration) bestScoreConsideration = score;
            }

            return bestScoreConsideration;
        }

        // Choose best action from a given list using this BrainConfig.
        public List<Action.Action> ChooseBest(List<Action.Action> actions, List<Action.Action> enemyActions = null)
        {
            if (actions == null || actions.Count == 0) return null;

            var allyActions = actions;
            var bestScore = float.NegativeInfinity;

            // Danh sách các action tốt nhất
            var bestList = new List<Action.Action>();

            foreach (var a in actions)
            {
                if (Maker != null && a.GetMaker() as PieceLogic != Maker) continue;

                var score = Evaluate(a, allyActions, enemyActions);

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
            if (Maker == null) return list;
            try
            {
                Maker.MoveList(list, false);
            }
            catch
            {
            }

            return list;
        }
    }
}