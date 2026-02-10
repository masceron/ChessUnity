using System.Collections.Generic;
using Game.Action;
using Game.Action.Quiets;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;

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
            for (var pos = 0; pos < gameState.formations.Length; pos++)
            {
                var formation = gameState.formations[pos];
                if (formation != null && formation is RealityDistortion distortion)
                {
                    allDistortions.Add(distortion);
                }
            }
            
            if (allDistortions.Count < 2) return;
            
            var piecesOnDistortions = new List<(PieceLogic piece, int currentPos)>();
            var distortionPositions = new List<int>();
            
            foreach (var distortion in allDistortions)
            {
                distortionPositions.Add(distortion.Pos);
                var piece = gameState.PieceBoard[distortion.Pos];
                if (piece != null)
                {
                    piecesOnDistortions.Add((piece, distortion.Pos));
                }
            }
            
            if (piecesOnDistortions.Count < 2) return;
            
            var availablePositions = new List<int>(distortionPositions);
            
            for (var i = availablePositions.Count - 1; i > 0; i--)
            {
                var j = UnityEngine.Random.Range(0, i + 1);
                var temp = availablePositions[i];
                availablePositions[i] = availablePositions[j];
                availablePositions[j] = temp;
            }
            
            // Gán mỗi quân đến một vị trí ngẫu nhiên
            // 2 quân k cùng đến 1 vị trí
            var usedPositions = new HashSet<int>();
            
            for (var i = 0; i < piecesOnDistortions.Count; i++)
            {
                var (piece, currentPos) = piecesOnDistortions[i];
                
                var targetPos = -1;
                
                foreach (var pos in availablePositions)
                {
                    if (!usedPositions.Contains(pos) && pos != currentPos)
                    {
                        targetPos = pos;
                        break;
                    }
                }
                
                if (targetPos == -1)
                {
                    foreach (var pos in availablePositions)
                    {
                        if (!usedPositions.Contains(pos))
                        {
                            targetPos = pos;
                            break;
                        }
                    }
                }
                
                if (targetPos == -1) continue;
                
                usedPositions.Add(targetPos);
                
                if (targetPos != currentPos)
                {
                    var pieceAtTarget = gameState.PieceBoard[targetPos];
                    if (pieceAtTarget == null)
                    {
                        ActionManager.EnqueueAction(new NormalMove(currentPos, targetPos));
                    }
                    else
                    {
                        ActionManager.EnqueueAction(new NormalSwap(currentPos, targetPos));
                    }
                }
            }
        }
    }
}