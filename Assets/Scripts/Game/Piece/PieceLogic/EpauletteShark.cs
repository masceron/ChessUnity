using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Condition;
using Game.Effects.Others;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
namespace Game.Piece.PieceLogic
{
    public class EpauletteShark : Commons.PieceLogic, IPieceWithSkill
    {
        private int timeToCooldown;
        public EpauletteShark(PieceConfig cfg) : base(cfg, QueenMoves.Quiets, QueenMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new EpauletteSharkPurify(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new DiurnalAmbush(this)));
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    var (rank, file) = RankFileOf(Pos);

                    foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 3))
                    {
                        var index = IndexOf(rankOff, fileOff);
                        var pOn = PieceOn(index);
                        if (pOn == null || pOn == this || pOn.PieceRank != PieceRank.Swarm) continue;
                        if (pOn.Color != Color)
                        {
                            list.Add(new EpauletteSharkActive(Pos, index));
                        }
                    }
                }
                else
                {
                    //query for AI in here
                    if (excludeEmptyTile)
                    {
                        var bestPieces = new List<Commons.PieceLogic>();
                        Commons.PieceLogic bestPiece = null;
                        var maxPoint = int.MinValue;
            
                        var (rank, file) = RankFileOf(Pos);

                        foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 3))
                        {
                            var index = IndexOf(rankOff, fileOff);
                            var pOn = PieceOn(index);
                            if (pOn == null || pOn.Pos == Pos || pOn.PieceRank != PieceRank.Swarm 
                                || pOn.Color == Color) continue;
                
                            var AIValue = pOn.GetValueForAI();
                            if (AIValue > maxPoint)
                            {
                                bestPieces.Clear();
                                bestPieces.Add(pOn);
                                maxPoint = AIValue;
                            }
                            else if (AIValue == maxPoint) bestPieces.Add(pOn);
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
                            bestPiece = bestPieces[UnityEngine.Random.Range(0, bestPieces.Count)];
                        }

                        if (bestPiece != null)
                        {
                            list.Add(new EpauletteSharkActive(Pos, bestPiece.Pos));
                        }
                    }
                    else
                    {
                        var (rank, file) = RankFileOf(Pos);
                        
                        foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 3))
                        {
                            var index = IndexOf(rankOff, fileOff);
                            if (!IsActive(IndexOf(rankOff, fileOff))) continue;
                            list.Add(new EpauletteSharkActive(Pos, index));
                        }
                    }
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown
        {
            get => timeToCooldown;
            set => timeToCooldown = value;
        }

        public SkillsDelegate Skills { get; }
    }
}