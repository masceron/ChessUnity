using System.Collections.Generic;
using Game.Common;
using Game.Managers;
using UnityEngine;
using UnityEngine.UI;
using UX.UI.Ingame;

namespace Game.AI
{
    // Simple AI manager that finds BrainComponents for the side to move and picks the best action.
    public class AIManager : MonoBehaviour
    {
        public static AIManager Ins { get; private set; }
        [SerializeField] private Transform canvas;
        [SerializeField] private TileScore tileScorePrefab;
        [SerializeField] private List<TileScore> spawnedTileScores = new(); 
        [SerializeField] private bool showScoreOnHover;
        [SerializeField] private Button showScoreButton, playActionButton;

        private void Awake()
        {
            if (Ins != null && Ins != this) { Destroy(this); return; }
            Ins = this;
        }

        void Start()
        {
            playActionButton.onClick.AddListener(UIMethod_PlayBestAction);
            showScoreButton.onClick.AddListener(ShowScoreToggle);
        }

        /// <summary>
        /// [For UI Button] Triggers the AI to show scores for all possible actions.
        /// </summary>
        public void UIMethod_ShowAllScores()
        {
            // Assuming the current turn is the AI's turn.
            bool sideToMove = MatchManager.Ins.GameState.SideToMove;
            AIShowAllActionScores(sideToMove);
        }

        /// <summary>
        /// [For UI Button] Triggers the AI to find and execute its best action.
        /// </summary>
        public void UIMethod_PlayBestAction()
        {
            bool sideToMove = MatchManager.Ins.GameState.SideToMove;
            AIPlayAndExecuteBestAction(sideToMove);
        }

        private void AIPlayAndExecuteBestAction(bool sideToMove)
        {
            AIUseRelic(sideToMove);

            var enemySnapshot = GenerateEnemySnapshot(sideToMove);

            Action.Action bestAction = AIPlayBestAction(sideToMove, enemySnapshot, out float bestScore);

            if (bestAction == null)
            {
                Debug.LogWarning("AI could not find any action to play.");
                return;
            }

            // Optionally show score of the chosen action before executing
            AIShowScoreAction(bestAction, bestScore);

            // Execute action
            if (bestAction is IAIAction aiAction)
            {
                aiAction.CompleteActionForAI();
            }
            else
            {
                UX.UI.Ingame.BoardViewer.Ins.ExecuteAction(bestAction);
            }
        }

        /// <summary>
        /// #AIUseRelic: Checks if the current side to move can use a relic and activates it.
        /// </summary>
        /// <param name="sideToMove">The color of the side currently making a move.</param>
        private void AIUseRelic(bool sideToMove)
        {
            var relic = sideToMove ? MatchManager.Ins.GameState.BlackRelic : MatchManager.Ins.GameState.WhiteRelic;

            if (relic != null && relic.currentCooldown == 0)
            {
                Debug.Log("Use relic " + relic.type);
                relic.ActiveForAI();
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

        /// <summary>
        /// #AIPlayBestAction: Finds the best action for the given side by evaluating all possible moves
        /// from all BrainComponents.
        /// </summary>
        /// <param name="sideToMove">The color of the side currently making a move.</param>
        /// <param name="enemySnapshot">A list of all possible actions for the opposing side.</param>
        /// <param name="globalBestScore">Output parameter: The score of the globally best action found.</param>
        /// <returns>The best action found, or null if no valid actions are available.</returns>
        private Action.Action AIPlayBestAction(bool sideToMove, List<Action.Action> enemySnapshot, out float globalBestScore)
        {
            var brains = FindObjectsByType<BrainComponent>(FindObjectsSortMode.None);
            if (brains == null || brains.Length == 0)
            {
                globalBestScore = float.NegativeInfinity;
                return null;
            }

            Action.Action globalBest = null;
            globalBestScore = float.NegativeInfinity;

            foreach (var brain in brains)
            {
                if (brain.Maker == null) continue;
                if (brain.Maker.Color != sideToMove) continue;

                var actions = brain.GatherActionsForMaker();
                if (actions == null || actions.Count == 0) continue;

                var bestForBrains = brain.ChooseBest(actions, enemySnapshot);
                var bestForBrain = bestForBrains[Random.Range(0, bestForBrains.Count)]; // Randomly pick one if multiple have same best score
                if (bestForBrain == null) continue;

                float score = brain.Evaluate(bestForBrain, actions, enemySnapshot);
                if (score > globalBestScore)
                {
                    globalBestScore = score;
                    globalBest = bestForBrain;
                }
            }
            return globalBest;
        }

        /// <summary>
        /// #AIShowAllActionScores: Scans and displays scores for all possible actions for the given side.
        /// </summary>
        /// <param name="sideToMove">The color of the side to evaluate.</param>
        private void AIShowAllActionScores(bool sideToMove)
        {
            ClearTileScores();

            var brains = FindObjectsByType<BrainComponent>(FindObjectsSortMode.None);
            if (brains == null || brains.Length == 0) return;

            var enemySnapshot = GenerateEnemySnapshot(sideToMove);

            foreach (var brain in brains)
            {
                if (brain.Maker == null || brain.Maker.Color != sideToMove) continue;

                var actions = brain.GatherActionsForMaker();
                if (actions == null || actions.Count == 0) continue;

                foreach (var action in actions)
                {
                    float score = brain.Evaluate(action, actions, enemySnapshot);
                    ShowScoreForAction(action, score);
                }
            }
        }
        /// <summary>
        /// #AIShowScoreAction: Displays the score of a given action on the board using a TileScore prefab.
        /// Clears any previously displayed scores.
        /// </summary>
        /// <param name="action">The action whose score is to be displayed.</param>
        /// <param name="score">The score of the action.</param>
        private void AIShowScoreAction(Action.Action action, float score)
        {
            ClearTileScores(); 
            ShowScoreForAction(action, score);
        }

        /// <summary>
        /// Instantiates and displays a single TileScore at a given board position.
        /// </summary>
        /// <param name="position">The board position to display the score at.</param>
        /// <param name="score">The score to display.</param>
        private void ShowScoreAtPosition(int position, float score)
        {
            TileScore tileScoreInstance = Instantiate(tileScorePrefab, canvas);
            spawnedTileScores.Add(tileScoreInstance);

            var (rank, file) = BoardUtils.RankFileOf(position);
            tileScoreInstance.SetPosition(rank, file);
            tileScoreInstance.SetScore(Mathf.RoundToInt(score));
            tileScoreInstance.Show();
        }

        /// <summary>
        /// Instantiates and displays a single TileScore for a given action without clearing previous ones.
        /// </summary>
        /// <param name="action">The action to display the score for.</param>
        /// <param name="score">The score of the action.</param>
        private void ShowScoreForAction(Action.Action action, float score)
        {
            if (action == null) return;
            ShowScoreAtPosition(action.Target, score);
        }


        /// <summary>
        /// Clears all currently displayed TileScore objects from the board.
        /// </summary>
        public void ClearTileScores()
        {
            foreach (var ts in spawnedTileScores)
            {
                Destroy(ts.gameObject);
            }
            spawnedTileScores.Clear();
        }

        /// <summary>
        /// Shows the action scores for a specific piece when hovering over it.
        /// Controlled by the 'showScoreOnHover' flag.
        /// </summary>
        /// <param name="pos">The board position of the piece.</param>
        public void ShowPieceActionScore(int pos)
        {
            if (!showScoreOnHover) return;

            ClearTileScores();

            var piece = BoardUtils.PieceOn(pos);
            if (piece == null) return;

            var brains = FindObjectsByType<BrainComponent>(FindObjectsSortMode.None);
            BrainComponent targetBrain = null;
            foreach (var brain in brains)
            {
                if (brain.Maker != null && brain.Maker.Pos == pos)
                {
                    targetBrain = brain;
                    break;
                }
            }

            if (targetBrain == null) return;

            var actions = targetBrain.GatherActionsForMaker();
            if (actions == null || actions.Count == 0) return;

            var enemySnapshot = GenerateEnemySnapshot(piece.Color);
            var targetScores = new Dictionary<int, float>();

            foreach (var action in actions)
            {
                float score = targetBrain.Evaluate(action, actions, enemySnapshot);
                int targetPos = action.Target;

                if (targetScores.TryGetValue(targetPos, out float existingScore))
                {
                    if (score > existingScore)
                    {
                        targetScores[targetPos] = score;
                    }
                }
                else
                {
                    targetScores.Add(targetPos, score);
                }
            }

            foreach (var (targetPos, score) in targetScores)
            {
                ShowScoreAtPosition(targetPos, score);
            }
        }

        private void ShowScoreToggle()
        {
            showScoreOnHover = !showScoreOnHover;
            if (showScoreOnHover)
            {
                showScoreButton.GetComponentInChildren<Text>().text = "Hide Score";
                if (BoardViewer.Selecting != -1)
                {
                    ShowPieceActionScore(BoardViewer.Selecting);
                }
            }
            else
            {
                showScoreButton.GetComponentInChildren<Text>().text = "Show Score";   
                ClearTileScores(); 
            }
        }
    }
}
