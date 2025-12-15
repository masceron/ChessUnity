using System.Collections.Generic;
using System.Linq;
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
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Archelon: Commons.PieceLogic, IPieceWithSkill
    {
        public Archelon(PieceConfig cfg) : base(cfg, RookMoves.Quiets, RookMoves.Captures)
        {
            ActionManager.ExecuteImmediately(new ApplyEffect(new HardenedShield(this, 3)));
            ActionManager.ExecuteImmediately(new ApplyEffect(new ArchelonDraw(this)));

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
                        if (pOn == null || pOn == this) continue;
                        if (pOn.Color == Color)
                        {
                            list.Add(new ArchelonShield(Pos, index));
                        }
                    }
                } else
                {
                    //query for AI in here
                    if (!excludeEmptyTile)
                    {
                        foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(RankOf(Pos), FileOf(Pos), 3))
                        {
                            var index = IndexOf(rankOff, fileOff);
                            var pOn = PieceOn(index);
                            if (pOn == this) continue;
                            list.Add(new ArchelonShield(Pos, index));
                        }
                    }
                    var rank = RankOf(Pos);
                    var file = FileOf(Pos);

                    // Gather allies within radius 3
                    var candidates = new List<Commons.PieceLogic>();
                    for (int dr = -3; dr <= 3; dr++)
                    {
                        for (int df = -3; df <= 3; df++)
                        {
                            int r = rank + dr;
                            int f = file + df;
                            if (!VerifyBounds(r) || !VerifyBounds(f)) continue;

                            var piece = PieceOn(IndexOf(r, f));
                            if (piece == null || piece.Color != this.Color) continue;

                            // Filter: no Shield, Hardened Shield, or Extremophile effect
                            if (piece.Effects.Any(e => e.EffectName == "effect_shield" || 
                                                        e.EffectName == "effect_hardened_shield" || 
                                                        e.EffectName == "effect_extremophile")) 
                            {
                                continue;
                            }

                            candidates.Add(piece);
                        }
                    }

                    if (candidates.Count == 0) return;

                    // Find minimum value
                    var minValue = candidates.Min(p => p.GetValueForAI());
                    var lowest = candidates.Where(p => p.GetValueForAI() == minValue).ToList();

                    // Pick one (random if multiple)
                    var chosen = lowest.Count == 1 ? lowest[0] : lowest[Random.Range(0, lowest.Count)];

                    list.Add(new ArchelonShield(Pos, chosen.Pos));
                }
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}