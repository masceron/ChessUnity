using Game.Action.Skills;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using Game.Common;
using System.Linq;
using System.Collections.Generic;
using ZLinq;
namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SohalSurgeonfish : Commons.PieceLogic, IPieceWithSkill
    {
        public SohalSurgeonfish(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    var targets = SkillRangeHelper.GetActiveEnemyPieceInRadius(Pos, 6);
                    foreach (var target in targets)
                    {
                        var piece = PieceOn(target);
                        if (piece == null) continue;
                        if (piece.Effects.Any(e => e.EffectName == "effect_slow"))
                        {
                            list.Add(new SohalSurgeonfishActive(Pos, target));
                        }
                    }
                }
                else
                {
                    if (!excludeEmptyTile)
                    {
                        var (rank, file) = RankFileOf(Pos);
                        foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 6))
                        {
                            var index = IndexOf(rankOff, fileOff);
                            list.Add(new SohalSurgeonfishActive(Pos, index));
                        }
                    }
                    else
                    {
                        var listPieces = new List<Commons.PieceLogic>();
            
                        foreach (var (rank, file) in MoveEnumerators.AroundUntil(RankOf(Pos), FileOf(Pos), 6))
                        {
                            var idx = IndexOf(rank, file);
                            var pOn = PieceOn(idx);
                            if (pOn != null && pOn.Color != Color)
                            {
                                if (pOn.Effects != null && !pOn.Effects.Any(e => e.EffectName == "effect_leashed") 
                                                        && pOn.Effects.Any(e => e.EffectName == "effect_slow"))
                                {
                                    listPieces.Add(pOn);
                                }
                            }
                        }
                        if (listPieces.Count == 0) return;
                        int maxValue = listPieces.Max(p => p.GetValueForAI());
                        var bestPieces = listPieces.Where(p => p.GetValueForAI() == maxValue).ToList();
                        if (bestPieces.Count == 0) return;
                        var random = new System.Random();
                        var selectedPiece = bestPieces[random.Next(bestPieces.Count)];
                        list.Add(new SohalSurgeonfishActive(Pos, selectedPiece.Pos));

                    }
                }
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}
