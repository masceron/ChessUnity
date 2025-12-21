using System.Linq;
using Game.Action.Skills;
using Game.Common;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Snaggletooths : Commons.PieceLogic, IPieceWithSkill
    {
        public Snaggletooths(PieceConfig cfg) : base(cfg, VersatileDefenderMove.Quiets, VersatileDefenderMove.Captures)
        {
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                var flag1 = false;
                if (SkillCooldown != 0) return;
                if (isPlayer)
                {
                    var (rank, file) = RankFileOf(Pos);
                    foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 2))
                    {
                        var index = IndexOf(rankOff, fileOff);
                        var piece = PieceOn(index);
                        if (piece == null) continue;
                        if (piece.Effects.Any(e => e.EffectName == "effect_bleeding"))
                        {
                            list.Add(new SnaggletoothsActive(Pos, index, false));
                            flag1 = true;
                        }
                    }
                    if (!flag1)
                    {
                        list.Add(new SnaggletoothsActive(Pos, Pos, true));
                    }
                }
                else
                {
                    var listPieces = new List<Commons.PieceLogic>();
                    //query for AI in here
                    if (!excludeEmptyTile)
                    {
                        var (rank, file) = RankFileOf(Pos);
                        foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, 2))
                        {
                            var index = IndexOf(rankOff, fileOff);
                            var piece = PieceOn(index);
                            if (piece == null) continue;
                            if (piece.Effects.Any(e => e.EffectName == "effect_bleeding"))
                            {
                                list.Add(new SnaggletoothsActive(Pos, index, false));
                                listPieces.Add(piece);
                            }
                        }
                    }
                    int maxValue = listPieces.Max(p => p.GetValueForAI());
                    var bestPieces = listPieces.Where(p => p.GetValueForAI() == maxValue).ToList();
                    if (bestPieces.Count == 0) return;

                    var random = new System.Random();
                    var selectedPiece = bestPieces[random.Next(bestPieces.Count)];
                    list.Add(new SnaggletoothsActive(Pos, selectedPiece.Pos, true));
                    
                }
            };
        }
        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}