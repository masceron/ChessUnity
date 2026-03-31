using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    public class PencilUrchin : Commons.PieceLogic, IPieceWithSkill
    {
        private const int SkillRange = 3;

        public PencilUrchin(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new Adaptation(this)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new FreeMovement(this)));

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    var (rank, file) = RankFileOf(Pos);
                    foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, SkillRange))
                        list.Add(new PencilUrchinActive(this, IndexOf(rankOff, fileOff)));
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

                        foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, SkillRange))
                        {
                            var index = IndexOf(rankOff, fileOff);
                            var pOn = PieceOn(index);
                            if (pOn == null || pOn.Pos == Pos || pOn.Color == Color
                                || pOn.Effects.Any(effect => effect.EffectName == "effect_extremophiles")) continue;

                            var AIValue = pOn.GetValueForAI();
                            if (AIValue > maxPoint)
                            {
                                bestPieces.Clear();
                                bestPieces.Add(pOn);
                                maxPoint = AIValue;
                            }
                            else if (AIValue == maxPoint)
                            {
                                bestPieces.Add(pOn);
                            }
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

                        if (bestPiece != null) list.Add(new PencilUrchinActive(this, bestPiece.Pos));
                    }
                    else
                    {
                        var (rank, file) = RankFileOf(Pos);

                        foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, SkillRange))
                        {
                            var index = IndexOf(rankOff, fileOff);
                            list.Add(new PencilUrchinActive(this, index));
                        }
                    }
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }

        public SkillsDelegate Skills { get; }
    }
}