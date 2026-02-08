using System.Collections.Generic;
using Game.Action;
using Game.Action.Quiets;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;
using static Game.Common.BoardUtils;
using Game.Action.Internal;

namespace Game.Tile.RealityDistortion
{
    public class RealityDistortionManager : Singleton<RealityDistortionManager>
    {
        private int lastProcessedTurn = -1;
        
        public void OnTurnStart(bool color)
        {
            var gameState = MatchManager.Ins.GameState;
            var currentTurn = gameState.CurrentTurn;
            
            if (lastProcessedTurn == currentTurn) return;
            lastProcessedTurn = currentTurn;
            
            var allDistortions = new List<RealityDistortion>();
            for (int pos = 0; pos < gameState.formations.Length; pos++)
            {
                var formation = gameState.formations[pos];
                if (formation != null && formation is RealityDistortion distortion)
                {
                    allDistortions.Add(distortion);
                }
            }
            
            if (allDistortions.Count < 2) return;
            
            var piecesOnDistortions = new List<(PieceLogic piece, int currentPos)>();
            
            foreach (var distortion in allDistortions)
            {
                var piece = gameState.PieceBoard[distortion.Pos];
                if (piece != null)
                {
                    piecesOnDistortions.Add((piece, distortion.Pos));
                }
            }
            
            foreach (var (piece, currentPos) in piecesOnDistortions)
            {
                var availableDistortions = new List<RealityDistortion>();
                foreach (var distortion in allDistortions)
                {
                    if (distortion.Pos != currentPos && gameState.PieceBoard[distortion.Pos] == null)
                    {
                        availableDistortions.Add(distortion);
                    }
                }
                
                if (availableDistortions.Count == 0) continue;
                
                var randomIndex = UnityEngine.Random.Range(0, availableDistortions.Count);
                var targetDistortion = availableDistortions[randomIndex];
                var targetPos = targetDistortion.Pos;
                
                ActionManager.EnqueueAction(new NormalMove(currentPos, targetPos));
            }
        }
    }
}