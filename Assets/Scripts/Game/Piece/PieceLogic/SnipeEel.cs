using Game.Action;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Traits;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using System.Collections.Generic;
using System.Linq;
using ZLinq;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SnipeEel : Commons.PieceLogic, IPieceWithSkill
    {
        public SnipeEel(PieceConfig cfg) : base(cfg, RangerMove.Quiets, RangerMove.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new SnipeEelPassive(this)));
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    if (SkillCooldown == 0)
                    {
                        var (rank, file) = RankFileOf(Pos);
                        foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 5))
                        {
                            var index = IndexOf(rankOff, fileOff);

                            var pOn = PieceOn(index);
                            if (pOn == null || pOn.Color == Color) continue;
                            list.Add(new SnipeEelActive(Pos, index));
                        }
                    }
                }
                else
                {
                    //query for AI in here
                    if (!excludeEmptyTile)
                    {
                        var (rank, file) = RankFileOf(Pos);
                        foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 5))
                        {
                            var index = IndexOf(rankOff, fileOff);
                            list.Add(new SnipeEelActive(Pos, index));
                        }
                    }
                    var listPieces = new List<Commons.PieceLogic>();
            
                    foreach (var (rank, file) in MoveEnumerators.AroundUntil(RankOf(Pos), FileOf(Pos), 3))
                    {
                        var idx = IndexOf(rank, file);
                        var pOn = PieceOn(idx);
                        if (pOn != null && pOn.Color != Color)
                        {
                            if(pOn.Effects != null && pOn.Effects.Any(e => e.EffectName == "effect_extremophile") 
                                                    && pOn.Effects.Any(e => e.EffectName == "effect_bound")) continue;
                            listPieces.Add(pOn);
                        }
                    }
                    if (listPieces.Count == 0) return;
                    int maxValue = listPieces.Max(p => p.GetValueForAI());
                    var bestPieces = listPieces.Where(p => p.GetValueForAI() == maxValue).ToList();
                    if (bestPieces.Count == 0) return;

                    var random = new System.Random();
                    var selectedPiece = bestPieces[random.Next(bestPieces.Count)];
                    list.Add(new SnipeEelActive(Pos, selectedPiece.Pos));
                }
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}