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
                    if (listPieces.Count > 0)
                    {
                        int randomIndex = Random.Range(0, listPieces.Count);
                        list.Add(new SnaggletoothsActive(Pos, listPieces[randomIndex].Pos, true));
                    }
                    
                }
            };
        }
        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}