using Game.Action.Skills;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    public class Hatchetfish : Commons.PieceLogic, IPieceWithSkill
    {
        public Hatchetfish(PieceConfig cfg) : base(cfg, PufferfishMoves.Quiets, PufferfishMoves.Captures)
        {
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                if (SkillCooldown > 0) return;
                if (isPlayer)
                {
                    var (rank, file) = RankFileOf(Pos);
                    for (var dr = -4; dr <= 4; dr++)
                    {
                        var trank = rank + dr;
                        if (!VerifyBounds(trank)) continue;
                        for (var df = -4; df <= 4; df++)
                        {
                            var fileOff = file + df;
                            if (!VerifyBounds(fileOff)) continue;
                            var tpos = IndexOf(trank, fileOff);
                            var pieceAt = PieceOn(tpos);
                            if (pieceAt == null || pieceAt.Color == Color) continue;

                            list.Add(new HatchetfishActive(Pos, tpos));
                        }
                    }
                }
                else
                {
                    //query for AI in here
                }
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}

