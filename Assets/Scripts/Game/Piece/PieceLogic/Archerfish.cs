using Game.Action.Skills;
using Game.Movesets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Piece.PieceLogic
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Archerfish : Commons.PieceLogic, IPieceWithSkill
    {
        private const int SkillRange = 4;
        private const int Duration1 = 2;
        private const int Duration2 = 2;
        
        public Archerfish(PieceConfig cfg) : base(cfg, BishopMoves.Quiets, BishopMoves.Captures)
        {
            Skills = (list, isPlayer, excludeEmptyTile) =>
            {
                SetStat(SkillStat.Range, SkillRange);
                SetStat(SkillStat.Duration, Duration1, 1);
                SetStat(SkillStat.Duration, Duration2, 2);
                
                if (SkillCooldown > 0) return;

                if (isPlayer)
                {
                    //Find all enemy pieces within 4 cells
                    var (trank, file) = RankFileOf(Pos);
                    for (var i = -GetStat(SkillStat.Range); i <= GetStat(SkillStat.Range); i++)
                    {
                        var rankOff = trank + i;
                        if (!VerifyBounds(rankOff)) continue;
                        for (var j = -GetStat(SkillStat.Range); j <= GetStat(SkillStat.Range); j++)
                        {
                            var fileOff = file + j;
                            if (!VerifyBounds(fileOff)) continue;
                            var tpos = IndexOf(rankOff, fileOff);
                            var pieceAt = PieceOn(tpos);
                            if (pieceAt == null || pieceAt.Color == Color) continue;
                            list.Add(new ArcherfishActive(this, pieceAt));
                        }
                    }
                }
                //query for AI in here
            };
        }

        int IPieceWithSkill.TimeToCooldown { get; set; }
        public SkillsDelegate Skills { get; set; }
    }
}