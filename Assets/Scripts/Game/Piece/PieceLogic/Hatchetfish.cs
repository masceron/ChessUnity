using System;
using System.Collections.Generic;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using UX.UI.Ingame;
using ZLinq;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    public class Hatchetfish : Commons.PieceLogic, IPieceWithSkill
    {
        private const int Range1 = 4;
        private const int Range2 = 2;
        
        public Hatchetfish(PieceConfig cfg) : base(cfg, PufferfishMoves.Quiets, PufferfishMoves.Captures)
        {
            
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown > 0) return;
                if (isPlayer)
                {
                    var (rank, file) = RankFileOf(Pos);
                    for (var dr = -Range1; dr <= Range1; dr++)
                    {
                        var trank = rank + dr;
                        if (!VerifyBounds(trank)) continue;
                        for (var df = -Range1; df <= Range1; df++)
                        {
                            var fileOff = file + df;
                            if (!VerifyBounds(fileOff)) continue;
                            var tpos = IndexOf(trank, fileOff);
                            var pieceAt = PieceOn(tpos);
                            if (pieceAt == null || pieceAt.Color == Color) continue;

                            list.Add(new HatchetfishActive(this, pieceAt));
                        }
                    }
                }
                else
                {
                    if (excludeEmptyTile)
                    {
                        var listPieces = new List<Commons.PieceLogic>();
                        var targets = SkillRangeHelper.GetActiveEnemyPieceInRadius(this, Range1);
                        foreach (var target in targets)
                            if (target.Effects.Any(e => e.EffectName == "effect_camouflage") &&
                                !target.Effects.Any(e => e.EffectName is "effect_blinded" or "effect_extremophile"))
                                listPieces.Add(target);


                        if (listPieces.Count == 0)
                        {
                            var targets1 = SkillRangeHelper.GetActiveEnemyPieceInRadius(this, Range2);
                            foreach (var target in targets1)
                                if (target.Effects.Any(e =>
                                        e.EffectName == "effect_marked" || e.EffectName == "effect_extremophile"))
                                    listPieces.Add(target);
                        }

                        if (listPieces.Count == 0) return;
                        var maxValue = listPieces.Max(p => p.GetValueForAI());
                        var bestPieces = listPieces.Where(p => p.GetValueForAI() == maxValue).ToList();
                        if (bestPieces.Count == 0) return;
                        var random = new Random();
                        var selectedPiece = bestPieces[random.Next(bestPieces.Count)];

                        list.Add(new HatchetfishActive(this, selectedPiece));
                    }
                    else
                    {
                        var (rank, file) = RankFileOf(Pos);

                        foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, Range1))
                        {
                            var index = IndexOf(rankOff, fileOff);
                            if (!IsActive(IndexOf(rankOff, fileOff))) continue;
                            list.Add(new HatchetfishActive(this, PieceOn(index)));
                        }
                    }
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}