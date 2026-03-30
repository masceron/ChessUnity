using System.Collections.Generic;
using Game.Action;
using Game.Action.Quiets;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using ZLinq;

namespace Game.Tile.RealityDistortion
{
    public class RealityDistortionManager : Singleton<RealityDistortionManager>
    {
        private int _lastProcessedTurn = -1;

        public void OnTurnStart()
        {
            var gameState = MatchManager.Ins.GameState;
            var currentTurn = gameState.CurrentTurn;

            if (_lastProcessedTurn == currentTurn) return;
            _lastProcessedTurn = currentTurn;

            var allDistortions = new List<RealityDistortion>();
            foreach (var formation in gameState.Formations)
                if (formation is RealityDistortion distortion)
                    allDistortions.Add(distortion);

            if (allDistortions.Count < 2) return;

            var piecesOnDistortions = new List<(PieceLogic piece, int currentPos)>();
            var distortionPositions = new List<int>();

            foreach (var distortion in allDistortions)
            {
                distortionPositions.Add(distortion.Pos);
                var piece = gameState.PieceBoard[distortion.Pos];
                if (piece != null) piecesOnDistortions.Add((piece, distortion.Pos));
            }

            if (piecesOnDistortions.Count < 2) return;

            var availablePositions = new List<int>(distortionPositions);

            for (var i = availablePositions.Count - 1; i > 0; i--)
            {
                var j = Random.Range(0, i + 1);
                (availablePositions[i], availablePositions[j]) = (availablePositions[j], availablePositions[i]);
            }

            // Gán mỗi quân đến một vị trí ngẫu nhiên
            // 2 quân k cùng đến 1 vị trí
            var usedPositions = new HashSet<int>();

            foreach (var (_, currentPos) in piecesOnDistortions)
            {
                var targetPos = -1;

                foreach (var pos in availablePositions.Where(pos => !usedPositions.Contains(pos) && pos != currentPos))
                {
                    targetPos = pos;
                    break;
                }

                if (targetPos == -1)
                    foreach (var pos in availablePositions)
                        if (!usedPositions.Contains(pos))
                        {
                            targetPos = pos;
                            break;
                        }

                if (targetPos == -1) continue;

                usedPositions.Add(targetPos);

                if (targetPos == currentPos) continue;
                var pieceAtTarget = gameState.PieceBoard[targetPos];
                if (pieceAtTarget == null)
                    ActionManager.EnqueueAction(new NormalMove(BoardUtils.PieceOn(currentPos), targetPos));
                else
                    ActionManager.EnqueueAction(new NormalSwap(BoardUtils.PieceOn(currentPos), pieceAtTarget));
            }
        }
    }
}