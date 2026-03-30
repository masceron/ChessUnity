using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Condition;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ChamberedNautilus : Commons.PieceLogic, IPieceWithSkill
    {
        public ChamberedNautilus(PieceConfig cfg) : base(cfg, BishopMoves.Quiets, BarracudaMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new ChamberedNautilusHunger(this)));

            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;

                if (isPlayer)
                {
                    var (rank, file) = RankFileOf(Pos);

                    foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 2))
                    {
                        var index = IndexOf(rankOff, fileOff);
                        var pOn = PieceOn(index);
                        if (pOn == null || pOn == this) continue;
                        if (pOn.Color != Color) list.Add(new ChamberedNautilusActive(this, pOn));
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

                        foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 2))
                        {
                            var index = IndexOf(rankOff, fileOff);
                            var pOn = PieceOn(index);
                            if (pOn == null || pOn.Pos == Pos || pOn.Color == Color
                                || pOn.Effects.Any(effect =>
                                    effect.EffectName == "effect_bound" || effect.EffectName == "effect_extremophiles"))
                                continue;

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

                        if (bestPiece != null) list.Add(new ChamberedNautilusActive(this, bestPiece));
                    }
                    else
                    {
                        var (rank, file) = RankFileOf(Pos);

                        foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 2))
                        {
                            var index = IndexOf(rankOff, fileOff);
                            var pOn = PieceOn(index);
                            if (pOn == null || pOn == this) continue;
                            list.Add(new ChamberedNautilusActive(this, pOn));
                        }
                    }
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }

        public SkillsDelegate Skills { get; }
    }
}