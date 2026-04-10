using System;
using System.Collections.Generic;
using Game.Action.Skills;
using Game.Common;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class HermitCrab : Commons.PieceLogic, IPieceWithSkill
    {
        public HermitCrab(PieceConfig cfg) : base(cfg, BishopMoves.Quiets, BishopMoves.Captures)
        {
            SetStat(SkillStat.Range, 3);
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown != 0) return;

                if (isPlayer)
                {
                    var (rank, file) = RankFileOf(Pos);
                    var range = GetStat(SkillStat.Range);

                    foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(rank, file, range))
                        MakeSkill(list, IndexOf(rankOff, fileOff));
                }
                else
                {
                    //query for AI in here
                    if (!excludeEmptyTile)
                    {
                        foreach (var (rankOff, fileOff) in MoveEnumerators.AroundUntil(RankOf(Pos), FileOf(Pos), GetStat(SkillStat.Range)))
                        {
                            var index = IndexOf(rankOff, fileOff);
                            if (index != Pos) list.Add(new HermitCrabSwap(this, PieceOn(index)));
                        }

                        return;
                    }

                    var rank = RankOf(Pos);
                    var file = FileOf(Pos);
                    var range = GetStat(SkillStat.Range);
                    List<int> candidates = new();
                    for (var i = rank - range; i < rank + range; ++i)
                    for (var j = file - range; j < file + range; ++j)
                    {
                        if (!VerifyBounds(file) || !VerifyBounds(rank)) continue;
                        if (PieceOn(IndexOf(i, j)) != null) candidates.Add(IndexOf(i, j));
                    }

                    var r = new Random();
                    list.Add(new HermitCrabSwap(this, PieceOn(candidates[r.Next(candidates.Count)])));
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }

        private void MakeSkill(List<Action.Action> list, int index)
        {
            var piece = PieceOn(index);
            if (piece == null || piece.Color != Color) return;
            list.Add(new HermitCrabSwap(this, piece));
        }
    }
}