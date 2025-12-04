using System.Collections.Generic;
using Game.Common;
using Game.Managers;
using UnityEngine;

namespace Game.AI
{
    // Simple AI manager that finds BrainComponents for the side to move and picks the best action.
    public class AIManager : MonoBehaviour
    {
        public static AIManager Ins { get; private set; }

        private void Awake()
        {
            if (Ins != null && Ins != this) { Destroy(this); return; }
            Ins = this;
        }

        // Entry: ask AI to pick and execute single best action for sideToMove.
        public void PlayBestActionForSide(bool sideToMove)
        {
            var relic = sideToMove ? MatchManager.Ins.GameState.BlackRelic: MatchManager.Ins.GameState.WhiteRelic;

            if (relic != null && relic.currentCooldown == 0)
            {
                Debug.Log("Use relic " + relic.type);
                relic.ActiveForAI();
            }

            var brains = FindObjectsByType<BrainComponent>(FindObjectsSortMode.None);
            if (brains == null || brains.Length == 0) return;

            // Prepare enemy snapshot (all actions of opposite side)
            var enemySnapshot = GenerateEnemySnapshot(sideToMove);

            Action.Action globalBest = null;
            float globalBestScore = float.NegativeInfinity;
            BrainComponent selectedBrain = null;

            foreach (var brain in brains)
            {
                if (brain.Maker == null) continue;
                if (brain.Maker.Color != sideToMove) continue;

                // Gather actions for this maker
                var actions = brain.GatherActionsForMaker();
                if (actions == null || actions.Count == 0) continue;

                var bestForBrains = brain.ChooseBest(actions, enemySnapshot);
                var bestForBrain = bestForBrains[Random.Range(0, bestForBrains.Count)];
                if (bestForBrain == null) continue;

                float score = brain.Evaluate(bestForBrain, actions, enemySnapshot);
                if (score > globalBestScore)
                {
                    globalBestScore = score;
                    globalBest = bestForBrain;
                    selectedBrain = brain;
                }
            }

            if (globalBest == null) return;

            // Execute action: handle pending-able actions or normal actions
            if (globalBest is IAIAction aiAction)
            {
                // Complete pending immediately for AI (many skills implement CompleteAction)
                aiAction.CompleteActionForAI();
            }
            else
            {
                // Use BoardViewer to execute to preserve UI / turn flow
                UX.UI.Ingame.BoardViewer.Ins.ExecuteAction(globalBest);
            }
        }

        // Build snapshot of enemy actions (opposite of side)
        private List<Action.Action> GenerateEnemySnapshot(bool side)
        {
            var list = new List<Action.Action>();
            var state = MatchManager.Ins.GameState;
            if (state == null) return list;

            for (int i = 0; i < BoardUtils.BoardSize; i++)
            {
                var p = state.PieceBoard[i];
                if (p == null) continue;
                if (p.Color == side) continue;
                try
                {
                    p.MoveList(list, isPlayer: false, excludeEmptyTile: false);
                }
                catch { }
            }

            return list;
        }
    }
}
