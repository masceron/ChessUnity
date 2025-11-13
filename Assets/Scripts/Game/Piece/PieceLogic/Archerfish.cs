using Game.Action.Skills;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Archerfish: PieceLogic, IPieceWithSkill
    {
        public Archerfish(PieceConfig cfg) : base(cfg, BishopMoves.Quiets, BishopMoves.Captures)
        {
            Skills = list =>
            {
                if (SkillCooldown > 0) return;
                //Find all enemy pieces within 4 cells
                var (trank, file) = RankFileOf(Pos);
                for (var i = -4; i <= 4; i++)
                {
                    var rankOff = trank + i;
                    if (!VerifyBounds(rankOff)) continue;
                    for (var j = -4; j <= 4; j++)
                    {
                        var fileOff = file + j;
                        if (!VerifyBounds(fileOff)) continue;
                        var tpos = IndexOf(rankOff, fileOff);
                        var pieceAt = PieceOn(tpos);
                        if (pieceAt == null || pieceAt.Color == Color) continue;
                        list.Add(new ArcherfishActive(Pos, tpos));
                    }
                }

            };
        }

        sbyte IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }

}

