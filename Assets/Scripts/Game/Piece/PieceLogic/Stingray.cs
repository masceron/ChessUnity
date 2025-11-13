using Game.Action.Skills;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Stingray: PieceLogic, IPieceWithSkill
    {
        public Stingray(PieceConfig cfg) : base(cfg, KingMoves.Quiets, KingMoves.Captures)
        {
            Skills = list =>
            {
                if (SkillCooldown != 0) return;

                var (rank, file) = RankFileOf(Pos);

                var board = PieceBoard();
                var active = ActiveBoard();

                for (var rankTo = rank - 2; rankTo <= rank + 2; rankTo += 2)
                {
                    if (!VerifyBounds(rankTo)) continue;
                    for (var fileTo = file - 2; fileTo <= file + 2; fileTo += 2)
                    {
                        if (!VerifyBounds(fileTo)) continue;
                        if (rankTo == rank && fileTo == file) continue;
                        var posTo = IndexOf(rankTo, fileTo);

                        if (board[posTo] == null && active[posTo])
                        {
                            list.Add(new StingrayDash(Pos, posTo));
                        }
                    }
                }
            };
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}