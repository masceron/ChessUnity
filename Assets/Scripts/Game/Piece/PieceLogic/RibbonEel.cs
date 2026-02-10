using System;
using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Internal.Pending.Piece;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Buffs;
using Game.Managers;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Random = UnityEngine.Random;

namespace Game.Piece.PieceLogic
{
    public class RibbonEel : Commons.PieceLogic, IPieceWithSkill
    {
        private sbyte timeToCooldown;

        public RibbonEel(PieceConfig cfg) : base(cfg, AmbushPredatorMoves.Quiets, KingMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Camouflage(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new TrueBite(this)));

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (!MatchManager.Ins.GameState.IsDay) return;
                if (isPlayer)
                {
                    foreach (var markedPiece in FindPiecesWithEffectName(!Color, "effect_marked"))
                    {
                        if (markedPiece == null || markedPiece.Pos == Pos || markedPiece.Color == Color) continue;
                        list.Add(new RibbonEelPendingForChooseTarget(markedPiece.Pos, Pos));
                    }
                }
                else
                {
                    if (excludeEmptyTile)
                    {
                        List<Commons.PieceLogic> bestPieces = new List<Commons.PieceLogic>();
                        Commons.PieceLogic bestPiece = null;
                        int maxPoint = int.MinValue;
                        

                        foreach (var markedPiece in FindPiecesWithEffectName(!Color, "effect_marked"))
                        {
                            if (markedPiece == null || markedPiece.Pos == Pos || markedPiece.Color == Color) continue;
                
                            int AIValue = markedPiece.GetValueForAI();
                            if (AIValue > maxPoint)
                            {
                                bestPieces.Clear();
                                bestPieces.Add(markedPiece);
                                maxPoint = AIValue;
                            }
                            else if (AIValue == maxPoint) bestPieces.Add(markedPiece);
                        }

                        if (bestPieces.Count == 0)
                        {
                            //
                        }
                        else if (bestPieces.Count == 1)
                        {
                            bestPiece = bestPieces[0];
                        }
                        else
                        {
                            bestPiece = bestPieces[Random.Range(0, bestPieces.Count)];
                        }

                        if (bestPiece != null)
                        {
                            var rank = RankOf(bestPiece.Pos);
                            var file = FileOf(bestPiece.Pos);
                            List<int> movePositions = new List<int>();
                            
                            foreach (var (rankOf, fileOf) in MoveEnumerators.AroundUntil(rank, file, 1))
                            {
                                var pos = IndexOf(rankOf, fileOf);
                                var pieceOn = PieceOn(pos);
                                if (pieceOn != null) continue;
                                
                                movePositions.Add(pos);
                            }

                            if (movePositions.Count == 0) return;
                            var bestMovePosition = movePositions[Random.Range(0, movePositions.Count)];
                            list.Add(new RibbonEelActive(bestMovePosition, Pos, bestPiece.Pos));
                        }
                    }
                    else
                    {
                        foreach (var markedPiece in FindPiecesWithEffectName(!Color, "effect_marked"))
                        {
                            if (markedPiece == null || markedPiece.Pos == Pos || markedPiece.Color == Color) continue;
                            
                            var rank = RankOf(markedPiece.Pos);
                            var file = FileOf(markedPiece.Pos);
                            
                            foreach (var (rankOf, fileOf) in MoveEnumerators.AroundUntil(rank, file, 1))
                            {
                                var pos = IndexOf(rankOf, fileOf);
                                var pieceOn = PieceOn(pos);
                                if (pieceOn != null) continue;
                                
                                list.Add(new RibbonEelActive(pos, Pos, markedPiece.Pos));
                            }
                        }
                    }
                }
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown
        {
            get => timeToCooldown;
            set => timeToCooldown = value;
        }

        public SkillsDelegate Skills { get; }
    }
}