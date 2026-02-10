using System.Collections.Generic;
using Game.Action.Internal.Pending;
using Game.Action.Internal.Pending.Relic;
using Game.Common;
using Game.Managers;
using Game.Piece;
using Game.Piece.PieceLogic.Commons;
using Game.Relics.Commons;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;

namespace Game.Relics
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RottingScythe : RelicLogic
    {
        public RottingScythe(RelicConfig cfg) : base(cfg)
        {
            CurrentCooldown = 0;
        }

        public override void Activate()
        {
            if (CurrentCooldown == 0)
            {
                foreach (var piece in MatchManager.Ins.GameState.PieceBoard)
                {
                    if (piece == null || piece.Effects.All(e => e.EffectName != "effect_infected")) continue;
                    TileManager.Ins.MarkAsMoveable(piece.Pos);
                    var pending = new RottingScythePending(this, piece.Pos, piece.Color);
                    BoardViewer.ListOf.Add(pending);
                }
                BoardViewer.Selecting = -2;
                BoardViewer.SelectingFunction = 4;
            }
        }

        public override void ActiveForAI()
        {
            PieceLogic bestPiece = null;
            var bestPieces = new List<PieceLogic>();
            
            var piecesInfected = FindPiecesWithEffectName(MatchManager.Ins.GameState.OurSide, "effect_infected");
            var enemyPiecesInfected = FindPiecesWithEffectName(!MatchManager.Ins.GameState.OurSide, "effect_infected");
            piecesInfected.AddRange(enemyPiecesInfected);

            foreach (var piece in piecesInfected)
            {
                if (piece != null && piece.PieceRank == PieceRank.Commander && piece.Color == !MatchManager.Ins.GameState.OurSide)
                {
                    bestPiece = piece;
                    break;
                }
            }
            
            // If no enemy commander found, evaluate based on surrounding pieces
            if (bestPiece == null)
            {
                var allOurPiecesAbove2 = true;
                
                foreach (var piece in piecesInfected)
                {
                    var (rank, file) = RankFileOf(piece.Pos);
                    
                    var ourPieceCount = 0;
                    var enemyPieceCount = 0;
                    var maxEnemyPieceCount = int.MinValue;
                    
                    foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 1))
                    {
                        var index = IndexOf(rankOff, fileOff);
                        var pOn = PieceOn(index);
                        
                        if (pOn != null && pOn.Color == MatchManager.Ins.GameState.OurSide 
                                        && pOn.Pos != piece.Pos)
                        {
                            ourPieceCount++;
                        }
                        else if (pOn != null && pOn.Color == !MatchManager.Ins.GameState.OurSide 
                                             && pOn.Pos != piece.Pos)
                        {
                            enemyPieceCount++;
                        }
                    }
                    
                    if (ourPieceCount < 2) allOurPiecesAbove2 = false;
                    
                    if (enemyPieceCount > maxEnemyPieceCount && enemyPieceCount > 0)
                    {
                        bestPieces.Clear();
                        bestPieces.Add(piece);
                        maxEnemyPieceCount = enemyPieceCount;
                    }
                    else if (enemyPieceCount == maxEnemyPieceCount)
                    {
                        bestPieces.Add(piece);
                    }
                    
                }
                
                if (allOurPiecesAbove2)
                {
                    return;
                }
                
                if (bestPieces.Count == 1)
                {
                    bestPiece = bestPieces[0];
                }
                else if (bestPieces.Count > 1)
                {
                    bestPiece = bestPieces[UnityEngine.Random.Range(0, bestPieces.Count)];
                }
                else if (bestPieces.Count == 0)
                {
                    //
                }
            }
            
            if (bestPieces != null)
            {
                var pending = new RottingScythePending(this, bestPiece.Pos, bestPiece.Color);
                if (pending is PendingAction p)
                {
                    p.CompleteAction();
                }
            }
            
        }
    }
}