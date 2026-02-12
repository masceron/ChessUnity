using System.Collections.Generic;
using System.Linq;
using Game.Action.Internal.Pending;
using Game.Action.Internal.Pending.Relic;
using Game.Action.Relics;
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
                    var pending = new RottingScythePending(this, piece.Pos);
                    BoardViewer.ListOf.Add(pending);
                }
                BoardViewer.Selecting = -2;
                BoardViewer.SelectingFunction = 4;
            }
        }

        public override void ActiveForAI()
        {
            var bestPieces = new List<PieceLogic>();
            
            var piecesInfected = FindPiecesWithEffectName(MatchManager.Ins.GameState.OurSide, "effect_infected");
            var enemyPiecesInfected = FindPiecesWithEffectName(!MatchManager.Ins.GameState.OurSide, "effect_infected");
            piecesInfected.AddRange(enemyPiecesInfected);

            var bestPiece = Enumerable.FirstOrDefault(piecesInfected, piece => piece is { PieceRank: PieceRank.Commander } && piece.Color == !MatchManager.Ins.GameState.OurSide);

            // If no enemy commander found, evaluate based on surrounding pieces
            if (bestPiece == null)
            {
                var allOurPiecesAbove2 = true;
                
                foreach (var piece in piecesInfected)
                {
                    var (rank, file) = RankFileOf(piece.Pos);
                    
                    var ourPieceCount = 0;
                    var enemyPieceCount = 0;
                    const int maxEnemyPieceCount = int.MinValue;
                    
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
                    
                    switch (enemyPieceCount)
                    {
                        case > maxEnemyPieceCount and > 0:
                            bestPieces.Clear();
                            bestPieces.Add(piece);
                            break;
                        case maxEnemyPieceCount:
                            bestPieces.Add(piece);
                            break;
                    }
                    
                }
                
                if (allOurPiecesAbove2)
                {
                    return;
                }
                
                switch (bestPieces.Count)
                {
                    case 1:
                        bestPiece = bestPieces[0];
                        break;
                    case > 1:
                        bestPiece = bestPieces[UnityEngine.Random.Range(0, bestPieces.Count)];
                        break;
                    case 0:
                        break;
                }
            }

            if (bestPiece == null) return;

            var excute = new RottingScytheAction(bestPiece.Pos);
            BoardViewer.Ins.ExecuteAction(excute);
            
            // var pending = new RottingScythePending(this, bestPiece.Pos);
            // if (pending is PendingAction p)
            // {
            //     BoardViewer.Ins.ExecuteAction(await p.WaitForCompletion());
            // }
        }
    }
}